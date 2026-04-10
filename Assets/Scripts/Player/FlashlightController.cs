using UnityEngine;

// In Unity 2D, this controls the Light2D component (Requires Universal Render Pipeline / 2D Lights setup)
public class FlashlightController : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D flashlight;
    public bool isEnabled = true;

    private void Start()
    {
        flashlight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    private void Update()
    {
        // Toggle the flashlight with 'F'
        if (Input.GetKeyDown(KeyCode.F))
        {
            isEnabled = !isEnabled;
            if (flashlight != null)
            {
                flashlight.enabled = isEnabled;
            }
        }

        // Make the light point towards the mouse cursor
        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction; // Assuming top-down 2D
    }
}
