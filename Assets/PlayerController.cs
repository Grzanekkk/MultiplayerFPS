using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSensivity = 3;

    private PlayerMotor motor;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        // Calculate movement velocity
        float xMovement = Input.GetAxisRaw("Horizontal");
        float zMovement = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMovement;  // (1, 0, 0) rigth // (-1, 0, 0) left
        Vector3 movVertical = transform.forward * zMovement;  // (0, 0, 1) forward // (0, 0, -1) back

        // Final movement vector
        Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

        // Apply movement
        motor.Move(velocity);

        // Calculate rotation (turning around) no looking up and down
        float vRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, vRot, 0f) * mouseSensivity;


        // Apply rotation
        motor.Rotate(rotation);

        // Calculate camera rotation (turning around) no looking up and down
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * mouseSensivity;

        // Apply camera rotation
        motor.RotateCamera(cameraRotation);
    }
}
