using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.Find("Player");
            if (p != null) player = p.transform;
        }
        
        // Calculate initial offset
        if (player != null)
            offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Follow the player on the X and Z axes, but keep the camera's fixed height
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.position = targetPosition;
        }
    }
}
