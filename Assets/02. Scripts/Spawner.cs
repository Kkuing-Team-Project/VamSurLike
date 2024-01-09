using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SpawnObject
{
    public GameObject objectPrefab;
    public float percent;
    private float realPercentMin;
    private float realPercentMax;

    public void SetRealPercent(float min, float max)
    {
        realPercentMin = min;
        realPercentMax = max;
        Debug.Log($"{objectPrefab.name}: {realPercentMin}, {realPercentMax}");

    }

    public bool IsSelected(float value)
    {
        Debug.Log($"{value}, {realPercentMin}, {realPercentMax} = {realPercentMin <= value && realPercentMax > value}");
        return realPercentMin <= value && realPercentMax > value;
    }
}

public class Spawner : MonoBehaviour
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
    private ObjectPool.ObjectType objectType;
    [SerializeField]
    private SpawnObject[] spawnObjects;

    [SerializeField]
    private float maxPercent;
    
    [HideInInspector]
    public bool isMax;
    [HideInInspector]
    public float maxRangeRadius = 50.0f;
    [HideInInspector]
    public float entityRadius = 0.5f;
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Spawner myTarget = (Spawner)target;
            myTarget.isMax = EditorGUILayout.Toggle("Is Max", myTarget.isMax);

            if (myTarget.isMax)
            {
                myTarget.maxRangeRadius = EditorGUILayout.FloatField("Max Range Radius", myTarget.maxRangeRadius);
                myTarget.entityRadius = EditorGUILayout.FloatField("Entity Radius", myTarget.entityRadius);
            }

            if (GUI.changed)
            {
                myTarget.maxPercent = 0;
                foreach (SpawnObject spawnObject in myTarget.spawnObjects)
                {
                    float max = myTarget.maxPercent + spawnObject.percent;
                    spawnObject.SetRealPercent(myTarget.maxPercent, max);

                    myTarget.maxPercent = max;
                }
                
                EditorUtility.SetDirty(myTarget);
            }
        }
    }
    #endif
    private void Start()
    {
        StartCoroutine(nameof(Spawn));
        
        
    }

    private void Update()
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

    private IEnumerator Spawn()
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
                    else
                        newZ = UnityEngine.Random.Range(minRandomRangeZ, maxRandomRangeZ);
                
                    point = new Vector3(newX, staticPos.y, newZ);
                    //Debug.Log(Vector3.Distance(staticPos, point));
                    //Debug.Log(spawnRadius);
                    // point.x -= pos.x + spawnRadius;
                    // point.z -= pos.z + spawnRadius;
                }
                
                if (isMax && isOutOfMaxCircle)
                {
                    color = color == Color.blue ? Color.yellow : Color.red;
                    
                }
            }

            float spawnObjectPercent = UnityEngine.Random.Range(0, maxPercent);
            GameObject selectedObj = null;
            // Debug.Log(spawnObjectPercent);

            foreach (SpawnObject spawnObject in spawnObjects)
            {
                if (spawnObject.IsSelected(spawnObjectPercent))
                {
                    selectedObj = spawnObject.objectPrefab;
                    Debug.Log(selectedObj.name);
                    break;
                }
            }
            
            // Debug.Log($"풀 하기전 {testPrefab} {point} {Quaternion.identity}");
            // change obj pool
            GameObject enemy = Pool.Pop(objectType, point);
            if (enemy.GetComponent<SpriteRenderer>())
                enemy.GetComponent<SpriteRenderer>().material.color = color;
            // Instantiate(testPrefab, point, Quaternion.identity);
        }
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
