using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}