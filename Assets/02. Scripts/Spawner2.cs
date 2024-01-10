using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct Spawnable
{
    public GameObject prefab;
    public float probability; // 이 프리팹이 생성될 확률
    public ObjectPool.ObjectType objectType;
}

[System.Serializable]
public struct Wave
{
    public float duration; // 웨이브의 지속 시간
    public Spawnable[] spawnables; // 해당 웨이브에서 사용될 Spawnable 배열
}

public class Spawner2 : MonoBehaviour
{
    private bool gameOver = false;
    public Vector3 center { get; private set; }
    public Vector3 mapSize { get; private set; }
    public float height { get; private set; }
    public float width { get; private set; }
    public ObjectPool Pool;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float delay = 1.0f;
    [SerializeField]
    private float range = 20.0f;
    [SerializeField, Range(1, 2)]
    private float mul = 1.5f;

    [SerializeField]
    private Wave[] waves; // 웨이브 배열
    private int currentWaveIndex = 0; // 현재 웨이브 인덱스
    private float waveTimer; // 웨이브 타이머
    private int cnt = 0;

    private Vector3 point;

    [HideInInspector]
    public bool isMax;
    [HideInInspector]
    public float maxRangeRadius = 50.0f;
    [HideInInspector]
    public float entityRadius = 0.5f;
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Spawner2))]
    public class SpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Spawner2 myTarget = (Spawner2)target;
            myTarget.isMax = EditorGUILayout.Toggle("Is Max", myTarget.isMax);

            if (myTarget.isMax)
            {
                myTarget.maxRangeRadius = EditorGUILayout.FloatField("Max Range Radius", myTarget.maxRangeRadius);
                myTarget.entityRadius = EditorGUILayout.FloatField("Entity Radius", myTarget.entityRadius);
                
                if (GUI.changed) EditorUtility.SetDirty(target);
            }
        }
    }
    #endif

    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    private void Update()
    {
        UpdateSpawnArea();
    }

    private IEnumerator StartWaves()
    {
        foreach (Wave wave in waves)
        {
            StartCoroutine(Spawn(wave));
            StartWave(wave);
            // 웨이브의 지속 시간만큼 기다림
            yield return new WaitForSeconds(wave.duration);
        }
    }

    private void StartWave(Wave wave)
    {
        // 웨이브 시작 관련 로직
        // 예: 적 생성, 타이머 시작 등
    }


    private IEnumerator Spawn(Wave wave)
    {
        Vector3 staticPos = transform.position;
        while (!gameOver)
        {
            yield return new WaitForSeconds(delay);

            Color color = Color.white;
                
            float minRandomRangeX = -(mapSize.x * 0.5f) + center.x;
            float maxRandomRangeX = (mapSize.x * 0.5f) + center.x;
            float minRandomRangeZ = -(mapSize.z * 0.5f) + center.z;
            float maxRandomRangeZ = (mapSize.z * 0.5f) + center.z;
                
            float x = UnityEngine.Random.Range(minRandomRangeX, maxRandomRangeX);
            float z = UnityEngine.Random.Range(minRandomRangeZ, maxRandomRangeZ);
            float selectX = width * mul * 0.5f;
            float selectZ = height * mul * 0.5f;
            float spawnRadius = maxRangeRadius - entityRadius;
                
            Vector3 point = new Vector3(x, staticPos.y, z);
            bool isInsideRedBox = -selectX + center.x <= x && x <= selectX + center.x && 
                                -selectZ + center.z <= z && z <= selectZ + center.z;
            bool isOutOfMaxCircle = Vector3.Distance(staticPos, point) > spawnRadius;

            if (isInsideRedBox || isOutOfMaxCircle)
            {
                float minSideX = selectX + center.x;
                if (Mathf.Abs(-selectX + center.x - x) < Mathf.Abs(selectX + center.x - x))
                    minSideX = -selectX + center.x;
                    float minSideZ = selectZ + center.z;
                    
                if (Mathf.Abs(-selectZ + center.z - z) < Mathf.Abs(selectZ + center.z - z))
                    minSideZ = -selectZ + center.z;
                    float maxSideX = mapSize.x * 0.5f;

                if (Mathf.Abs(-maxSideX + center.x - minSideX) > Mathf.Abs(maxSideX + center.x - minSideX))
                        minRandomRangeX = minSideX;
                    else
                        maxRandomRangeX = minSideX;

                    float maxSideZ = mapSize.z * 0.5f;

                if (Mathf.Abs(-maxSideZ + center.z - minSideZ) > Mathf.Abs(maxSideZ + center.z - minSideZ))
                    minRandomRangeZ = minSideZ;
                else
                    maxRandomRangeZ = minSideZ;
                    
                if (isInsideRedBox)
                {
                    float newX = x;
                    float newZ = z;
                    color = Color.blue;
                    if (Mathf.Abs(minSideX - x) < Mathf.Abs(minSideZ - z))
                        newX = UnityEngine.Random.Range(minRandomRangeX, maxRandomRangeX);
                    else{
                        newZ = UnityEngine.Random.Range(minRandomRangeZ, maxRandomRangeZ);
                    
                        point = new Vector3(newX, staticPos.y, newZ);
                        // print(point);
                        SpawnEnemy(wave, point);
                    }
                }
            }

            if (isMax && isOutOfMaxCircle)
            {
                color = color == Color.blue ? Color.yellow : Color.red;
                    
            }
        }
        Debug.Log("Wave 종료"); // 웨이브 종료 로그

        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            StartWave(wave); // 다음 웨이브 시작
        }
        else
        {
            // 모든 웨이브 완료 후 처리 로직 (예: 게임 종료, 승리 화면 등)
        }
    }

    private void SpawnEnemy(Wave wave, Vector3 point)
    {
        Spawnable selectedSpawnable = SelectPrefabBasedOnProbability(wave.spawnables);
        GameObject enemyPrefab = selectedSpawnable.prefab;
        if (enemyPrefab != null)
        {
            // Vector3 point = CalculateSpawnPosition();
            // print(point);
            GameObject enemy = Pool.Pop(selectedSpawnable.objectType, point); // 여기 수정
            // 나머지 코드...
        }
    }
    private Spawnable SelectPrefabBasedOnProbability(Spawnable[] spawnables)
    {
        float totalProbability = 0;
        foreach (var spawnable in spawnables)
        {
            totalProbability += spawnable.probability;
        }

        float randomPoint = UnityEngine.Random.Range(0, totalProbability);
        float currentProbability = 0;

        foreach (var spawnable in spawnables)
        {
            currentProbability += spawnable.probability;
            if (randomPoint <= currentProbability)
            {
                return spawnable;
            }
        }

        return default; // 기본값 반환
    }

    // private Vector3 CalculateSpawnPosition()
    // {
    //     // 적 생성 위치 계산 로직
    //     return point;
    // }

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
        mapSize = new Vector3(width * mul + range, 0, height * mul + range);
    }

     public void OnDrawGizmosSelected()
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
        mapSize = new Vector3(width * mul + range, 0, height * mul + range);
        Vector3 boxSize = new Vector3(width * mul, 0, height * mul);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftDownPoint, rightDownPoint);
        Gizmos.DrawLine(leftDownPoint,leftTopPoint);
        Gizmos.DrawLine(rightDownPoint, rightTopPoint);
        Gizmos.DrawLine(leftTopPoint, rightTopPoint);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, mapSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, boxSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(playerCamera.transform.position, center);
        Gizmos.DrawWireCube(center, boxSize / mul);
        
        if (isMax)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxRangeRadius);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, maxRangeRadius - entityRadius);
        }
    }
}
