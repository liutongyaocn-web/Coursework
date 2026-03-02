using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    private InputAction moveAction;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Set up the new Input System action for basic WASD movement
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        
        moveAction.Enable();
    }

    void OnDestroy()
    {
        moveAction?.Disable();
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
            
            // Create a mathematical plane at the player's height (y position)
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 hitPoint = ray.GetPoint(rayDistance);
                
                // Determine the direction to face
                Vector3 lookDirection = hitPoint - transform.position;
                lookDirection.y = 0; // Keep rotation strictly on the Y-axis

                // Rotate if there's a valid direction
                if (lookDirection.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                }
            }
        }
    }
}