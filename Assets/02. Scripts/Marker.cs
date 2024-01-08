using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Marker : MonoBehaviour
    {
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Spawner spawner;
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private Text meterText;

        private void Update()
        {
            Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
            Vector3 markPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 point = new Vector3(playerPos.x, 0, markPos.z);
            float a = Vector2.Distance(point, playerPos) * 2.0f;
            float b = Vector3.Distance(point, markPos) * 2.0f;
            float distance = Vector3.Distance(playerPos,  markPos);
            
            if (meterText)
                meterText.text = distance.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void OnDrawGizmosSelected()
        {
            if (player)
            {
                Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
                Vector3 markPos = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 point = new Vector3(playerPos.x, 0, markPos.z);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(point, playerPos);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(point, markPos);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(playerPos, markPos);
                
                if (spawner)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireCube(spawner.center, new Vector3(spawner.width, 0, spawner.height));
                    float a = Vector2.Distance(point, playerPos);
                    float b = Vector3.Distance(point, markPos);
                    
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(Vector3.ClampMagnitude(point, spawner.width), 0.5f);
                    // Gizmos.DrawWireSphere(Vector3.ClampMagnitude(point, spawner.height), 0.5f);
                }
            }
        }
    }
}