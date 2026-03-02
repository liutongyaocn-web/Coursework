using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction fireAction;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Set up the movement action
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
            
        // Set up the firing action
        fireAction = new InputAction("Fire", binding: "<Mouse>/leftButton");
        fireAction.performed += ctx => Shoot();
        
        moveAction.Enable();
        fireAction.Enable();
    }

    void OnDestroy()
    {
        fireAction.performed -= ctx => Shoot();
        moveAction?.Disable();
        fireAction?.Disable();
    }

    void Update()
    {
        // 1. Handle Movement
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * speed * Time.deltaTime);

        // 2. Handle Rotation (Look at Mouse)
        if (Mouse.current != null && Camera.main != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 hitPoint = ray.GetPoint(rayDistance);
                Vector3 lookDirection = hitPoint - transform.position;
                lookDirection.y = 0;

                if (lookDirection.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                }
            }
        }
    }
    
    void Shoot()
    {
        if (bulletPrefab != null)
        {
            // Calculate a spawn position slightly in front of the player to avoid colliding with themselves immediately
            Vector3 spawnPos = transform.position + transform.forward * 1.5f;
            
            // Instantiate the bullet
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
            
            // Apply forward velocity
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * bulletSpeed;
            }
            
            // Destroy after 3 seconds
            Destroy(bullet, 3f);
        }
    }
}