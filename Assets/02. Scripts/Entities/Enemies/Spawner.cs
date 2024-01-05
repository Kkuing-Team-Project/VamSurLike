using System.Collections;
using UnityEngine;

namespace Enmey
{
    public class Spawner : MonoBehaviour
    {
        private bool gameOver = false;
        private Vector3 center;
        private Vector3 mapSize;
        private float height;
        private float width;

        public Camera playerCamera;
        public float delay = 1.0f;
        public float range = 20.0f;
        [Range(1, 2)] public float mul = 1.5f;

        public GameObject testPrefab;

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

            height = (maxZ - minZ) * mul;
            width = (maxX - minX) * mul;
            center = new Vector3((maxX - minX) * 0.5f + minX, 0, (maxZ - minZ) * 0.5f + minZ);
            mapSize = new Vector3(width + range, 0, height + range);
        }

        private IEnumerator Spawn()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(delay);

                float x = UnityEngine.Random.Range(-(mapSize.x * 0.5f) + center.x, (mapSize.x * 0.5f) + center.x);
                float z = UnityEngine.Random.Range(-(mapSize.z * 0.5f) + center.z, (mapSize.z * 0.5f) + center.z);

                float selectX = width * 0.5f;
                float selectZ = height * 0.5f;

                Vector3 point = new Vector3(x, 1.0f, z);
                if (-selectX + center.x <= x && x <= selectX + center.x &&
                    -selectZ + center.z <= z && z <= selectZ + center.z)
                {
                    float minX = selectX + center.x;
                    if (Mathf.Abs(-selectX + center.x - x) < Mathf.Abs(selectX + center.x - x))
                        minX = -selectX + center.x;
                    float minZ = selectZ + center.z;
                    if (Mathf.Abs(-selectZ + center.z - z) < Mathf.Abs(selectZ + center.z - z))
                        minZ = -selectZ + center.z;

                    if (Mathf.Abs(minX - x) < Mathf.Abs(minZ - z))
                        minZ = z;
                    else
                        minX = x;

                    point = new Vector3(minX, 1.0f, minZ);
                }

                // change obj pool
                Instantiate(testPrefab, point, Quaternion.identity);
            }
        }

        private void OnDrawGizmosSelected()
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

            height = (maxZ - minZ) * mul;
            width = (maxX - minX) * mul;
            center = new Vector3((maxX - minX) * 0.5f + minX, 0, (maxZ - minZ) * 0.5f + minZ);
            mapSize = new Vector3(width + range, 0, height + range);
            Vector3 boxSize = new Vector3(width, 0, height);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, mapSize);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, boxSize);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(playerCamera.transform.position, center);
            Gizmos.DrawWireCube(center, boxSize / mul);
        }
    }
}