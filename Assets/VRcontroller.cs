using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerController : MonoBehaviour
{
    public XRController leftController;
    public XRController rightController;

    public float moveSpeed = 3f;
    public float rotationSpeed = 100f;

    void Update()
    {
        // Movement
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Translate(new Vector3(inputAxis.x, 0, inputAxis.y) * moveSpeed * Time.deltaTime);

        // Rotation
        float rotationInput = Input.GetAxis("Rotation");
        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
    }
}
