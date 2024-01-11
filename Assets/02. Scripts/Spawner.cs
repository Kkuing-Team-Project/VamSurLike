using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnObject
{
    public ObjectPool.ObjectType type;

    public float percent;
    public float percentAmpl;
    private float realPercentMin;
    private float realPercentMax;

    public void SetRealPercent(float min, float max)
    {
        realPercentMin = min;
        realPercentMax = max;
        // Debug.Log($"{type}: {realPercentMin}, {realPercentMax}");
    }

    public bool IsSelected(float value)
    {
        return realPercentMin <= value && realPercentMax > value;
    }
}

[System.Serializable]
public class Wave
{
    public float duration;
    public SpawnObject[] spawnObjects;
    public float maxPercent = 100.0f;
}

public class Spawner : MonoBehaviour
{
    private bool gameOver = false;
    public Vector3 center { get; private set; }
    public Vector3 mapSize { get; private set; }
    public float height { get; private set; }
    public float width { get; private set; }

    public ObjectPool Pool;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float delay = 1.0f;

    public bool stop;
    private float currentTime;
    private float lastSpawnTime;
    private float lastChangeWaveTime;

    [HideInInspector] public bool isWave;
    [HideInInspector] public SpawnObject[] spawnObjects;
    [HideInInspector] public float maxPercent;
    [HideInInspector] public Wave[] waves;
    private int currentWaveIndex;

    [HideInInspector] public bool isStatic;
    [HideInInspector] public float maxRangeRadius = 50.0f;
    [HideInInspector] public float entityRadius = 0.5f;
    [HideInInspector] public float notSpawnRadius = 5.0f;
    [HideInInspector] public float range = 20.0f;
    [HideInInspector] public float mul = 1.5f;

    private Vector3 minRandomRange;
    private Vector3 maxRandomRange;

    private void Start()
    {
        currentWaveIndex = 0;
        currentTime = 0;
        if (isWave)
            SetPercent(waves[0].spawnObjects, waves[0].maxPercent);
        else
            SetPercent(spawnObjects, maxPercent);
    }

    private void Update()
    {
        UpdateSpawnArea();

        if (!stop)
        {
            currentTime += Time.deltaTime;

            if (lastSpawnTime + delay < currentTime)
            {
                lastSpawnTime = currentTime;
                Spawn();
            }

            if (isWave && lastChangeWaveTime + waves[currentWaveIndex].duration < currentTime)
            {
                lastChangeWaveTime = currentTime;
                ChangeWave();
            }
        }
    }

    private void SetPercent(SpawnObject[] objects, float percentMax)
    {
        float total = 0;
        foreach (SpawnObject spawnObject in objects)
        {
            total += spawnObject.percentAmpl;
        }

        float min = 0;
        foreach (SpawnObject spawnObject in objects)
        {
            float currentPercent;
            if (total > percentMax)
                currentPercent = spawnObject.percentAmpl / total * percentMax;
            else
                currentPercent = spawnObject.percentAmpl;
            spawnObject.percent = currentPercent;

            spawnObject.SetRealPercent(min, min + currentPercent);
            min += currentPercent;
        }
    }

    private void UpdateSpawnArea()
    {
        Ray rightTopRay = playerCamera.ViewportPointToRay(Vector2.one);
        Ray leftTopRay = playerCamera.ViewportPointToRay(new Vector2(1, 0));
        Ray rightDownRay = playerCamera.ViewportPointToRay(new Vector2(0, 1));
        Ray leftDownRay = playerCamera.ViewportPointToRay(Vector2.zero);
        RaycastHit hit;

        Vector3 rightTopPoint =
            Physics.Raycast(rightTopRay, out hit, playerCamera.farClipPlane, LayerMask.GetMask("FLOOR"))
                ? hit.point
                : Vector3.zero;
        Vector3 leftTopPoint =
            Physics.Raycast(leftTopRay, out hit, playerCamera.farClipPlane, LayerMask.GetMask("FLOOR"))
                ? hit.point
                : Vector3.zero;
        Vector3 rightDownPoint =
            Physics.Raycast(rightDownRay, out hit, playerCamera.farClipPlane, LayerMask.GetMask("FLOOR"))
                ? hit.point
                : Vector3.zero;
        Vector3 leftDownPoint =
            Physics.Raycast(leftDownRay, out hit, playerCamera.farClipPlane, LayerMask.GetMask("FLOOR"))
                ? hit.point
                : Vector3.zero;

        float maxZ = Mathf.Max(leftDownPoint.z,
            Mathf.Max(leftTopPoint.z, Mathf.Max(rightDownPoint.z, rightTopPoint.z)));
        float minZ = Mathf.Min(leftDownPoint.z,
            Mathf.Min(leftTopPoint.z, Mathf.Min(rightDownPoint.z, rightTopPoint.z)));
        float maxX = Mathf.Max(leftDownPoint.x,
            Mathf.Max(leftTopPoint.x, Mathf.Max(rightDownPoint.x, rightTopPoint.x)));
        float minX = Mathf.Min(leftDownPoint.x,
            Mathf.Min(leftTopPoint.x, Mathf.Min(rightDownPoint.x, rightTopPoint.x)));

        height = maxZ - minZ;
        width = maxX - minX;
        center = new Vector3(width * 0.5f + minX, 0, height * 0.5f + minZ);

        if (isStatic)
            mapSize = new Vector3((maxRangeRadius - entityRadius) * 2.0f, 0, (maxRangeRadius - entityRadius) * 2.0f);
        else
            mapSize = new Vector3(width * mul + range, 0, height * mul + range);
    }

    private Vector3 GetMinSide(Vector3 original, Vector3 target, Vector3 size)
    {
        Vector3 minSide = new Vector3(size.x + target.x, 0, size.z + target.z);
        if (Mathf.Abs(-size.x + target.x - original.x) < Mathf.Abs(size.x + target.x - original.x))
            minSide.x = -size.x + target.x;
        if (Mathf.Abs(-size.z + target.z - original.z) < Mathf.Abs(size.z + target.z - original.z))
            minSide.z = -size.z + target.z;

        return minSide;
    }

    private Vector3 SaeHan(Vector3 point, float x, float z, Vector3 minSide, Vector3 maxSide, Vector3 target)
    {
        if (Mathf.Abs(-maxSide.x + target.x - minSide.x) > Mathf.Abs(maxSide.x + target.x - minSide.x))
            minRandomRange.x = minSide.x;
        else
            maxRandomRange.x = minSide.x;

        if (Mathf.Abs(-maxSide.z + target.z - minSide.z) > Mathf.Abs(maxSide.z + target.z - minSide.z))
            minRandomRange.z = minSide.z;
        else
            maxRandomRange.z = minSide.z;

        float newX = x;
        float newZ = z;
        if (Mathf.Abs(minSide.x - x) < Mathf.Abs(minSide.z - z))
            newX = UnityEngine.Random.Range(minRandomRange.x, maxRandomRange.x);
        else
            newZ = UnityEngine.Random.Range(minRandomRange.z, maxRandomRange.z);

        return new Vector3(newX, point.y, newZ);
    }

    private void ChangeWave()
    {
        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            SetPercent(waves[currentWaveIndex].spawnObjects, waves[currentWaveIndex].maxPercent);
            // Debug.Log($"wave change... current wave index: {currentWaveIndex}");
        }
    }

    private void Spawn()
    {
        Vector3 staticPos = transform.position;

        minRandomRange = new Vector3(-(mapSize.x * 0.5f) + center.x, 0, -(mapSize.z * 0.5f) + center.z);
        maxRandomRange = new Vector3((mapSize.x * 0.5f) + center.x, 0, (mapSize.z * 0.5f) + center.z);
        if (isStatic)
        {
            minRandomRange.x = -(mapSize.x * 0.5f) + staticPos.x;
            maxRandomRange.x = (mapSize.x * 0.5f) + staticPos.x;
            minRandomRange.z = -(mapSize.z * 0.5f) + staticPos.z;
            maxRandomRange.z = (mapSize.z * 0.5f) + staticPos.z;
        }

        float x = UnityEngine.Random.Range(minRandomRange.x, maxRandomRange.x);
        float z = UnityEngine.Random.Range(minRandomRange.z, maxRandomRange.z);

        float spawnRadius = maxRangeRadius - entityRadius;

        Vector3 point = new Vector3(x, staticPos.y, z);
        bool isOutOfMaxCircle = Vector3.Distance(staticPos, point) > spawnRadius;
        Vector3 select = new Vector3(width * mul * 0.5f, 0, height * mul * 0.5f);

        if (isStatic)
        {
            minRandomRange.x = -(mapSize.x * 0.5f) / Mathf.Sqrt(2) + staticPos.x;
            maxRandomRange.x = (mapSize.x * 0.5f) / Mathf.Sqrt(2) + staticPos.x;
            minRandomRange.z = -(mapSize.z * 0.5f) / Mathf.Sqrt(2) + staticPos.z;
            maxRandomRange.z = (mapSize.z * 0.5f) / Mathf.Sqrt(2) + staticPos.z;

            if (isOutOfMaxCircle)
            {
                if (Vector3.Distance(staticPos, point) > spawnRadius)
                {
                    point.x = UnityEngine.Random.Range(minRandomRange.x, maxRandomRange.x);
                    point.z = UnityEngine.Random.Range(minRandomRange.z, maxRandomRange.z);
                }
            }

            if (Vector3.Distance(center, point) < notSpawnRadius)
            {
                float leftRandomRange = center.x;
                float rightRandomRange = center.x;
                float topRandomRange = center.z;
                float downRandomRange = center.z;
                if (-notSpawnRadius + center.x > minRandomRange.x)
                {
                    leftRandomRange = -notSpawnRadius + center.x < maxRandomRange.x
                        ? -notSpawnRadius + center.x
                        : maxRandomRange.x;
                }

                if (notSpawnRadius + center.x < maxRandomRange.x)
                {
                    rightRandomRange = notSpawnRadius + center.x > minRandomRange.x
                        ? notSpawnRadius + center.x
                        : minRandomRange.x;
                }

                if (notSpawnRadius + center.z < maxRandomRange.z)
                {
                    topRandomRange = notSpawnRadius + center.z > minRandomRange.z
                        ? notSpawnRadius + center.z
                        : minRandomRange.z;
                }

                if (-notSpawnRadius + center.z > minRandomRange.z)
                {
                    downRandomRange = -notSpawnRadius + center.z < maxRandomRange.z
                        ? -notSpawnRadius + center.z
                        : maxRandomRange.z;
                }

                List<Vector3> randomPos = new List<Vector3>();
                Vector3 temp = staticPos;
                if (leftRandomRange != center.x)
                {
                    temp.x = UnityEngine.Random.Range(minRandomRange.x, leftRandomRange);
                    temp.z = UnityEngine.Random.Range(minRandomRange.z, maxRandomRange.z);

                    randomPos.Add(temp);
                }

                if (rightRandomRange != center.x)
                {
                    temp.x = UnityEngine.Random.Range(rightRandomRange, maxRandomRange.x);
                    temp.z = UnityEngine.Random.Range(minRandomRange.z, maxRandomRange.z);

                    randomPos.Add(temp);
                }

                if (topRandomRange != center.z)
                {
                    temp.x = UnityEngine.Random.Range(minRandomRange.x, maxRandomRange.x);
                    temp.z = UnityEngine.Random.Range(maxRandomRange.z, topRandomRange);

                    randomPos.Add(temp);
                }

                if (downRandomRange != center.z)
                {
                    temp.x = UnityEngine.Random.Range(minRandomRange.x, maxRandomRange.x);
                    temp.z = UnityEngine.Random.Range(downRandomRange, minRandomRange.z);

                    randomPos.Add(temp);
                }

                point = randomPos[UnityEngine.Random.Range(0, randomPos.Count)];
            }
        }
        else if (-select.x + center.x <= x && x <= select.x + center.x &&
                 -select.z + center.z <= z && z <= select.z + center.z)
        {
            Vector3 minSide = GetMinSide(new Vector3(x, 0, z), center, select);
            point = SaeHan(point, x, z, minSide, new Vector3(mapSize.x * 0.5f, 0, mapSize.z * 0.5f), center);
        }

        ObjectPool.ObjectType type =
            isWave
                ? GetRandomSpawnObjectType(waves[currentWaveIndex].spawnObjects, waves[currentWaveIndex].maxPercent)
                : GetRandomSpawnObjectType(spawnObjects, maxPercent);
        GameObject enemy = Pool.Pop(type, point);
    }

    private ObjectPool.ObjectType GetRandomSpawnObjectType(SpawnObject[] objects, float percentMax)
    {
        float percent = UnityEngine.Random.Range(0, percentMax);
        foreach (SpawnObject spawnObject in objects)
        {
            if (spawnObject.IsSelected(percent))
            {
                return spawnObject.type;
            }
        }

        return ObjectPool.ObjectType.None;
    }

    public void OnDrawGizmosSelected()
    {
        foreach (Wave wave in waves)
        {
            SetPercent(wave.spawnObjects, wave.maxPercent);
        }

        SetPercent(spawnObjects, maxPercent);

        UpdateSpawnArea();

        Vector3 staticPos = transform.position;
        if (isStatic)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(staticPos, maxRangeRadius);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(staticPos, maxRangeRadius - entityRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, notSpawnRadius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, mapSize);

            Vector3 boxSize = new Vector3(width * mul, 0, height * mul);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, boxSize);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(playerCamera.transform.position, center);
            Gizmos.DrawWireCube(center, boxSize / mul);
        }
    }
}