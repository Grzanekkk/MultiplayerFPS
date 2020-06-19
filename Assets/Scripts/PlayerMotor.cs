using System;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;




    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Gets a movement vector 
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // Gets a rotation vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Get a force vector for thruster
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    // Gets a rotation vector for the camera
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);    // Ignoruje mase obiektu
        }

    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            // New Rotation code
            currentCameraRotationX -= cameraRotationX;
            // Ustawianie maksymalnego i minimalnego nachylenia kamery
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

            // Old Roation code without any limits
            // cam.transform.Rotate(-cameraRotation);
        }
    }
}
