using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSensivity = 3;
    [SerializeField]
    private float thrusterForce = 1000f;


    [Header("Joint settings")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    // Przypisywanie komponentów
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSetting(jointSpring);
    }

    void Update()
    {
        // Calculate movement velocity
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMovement;  // (1, 0, 0) rigth // (-1, 0, 0) left
        Vector3 movVertical = transform.forward * zMovement;  // (0, 0, 1) forward // (0, 0, -1) back

        // Final movement vector
        Vector3 velocity = (movHorizontal + movVertical) * speed;

        // Animation movement 
        animator.SetFloat("ForwardVelocity", zMovement);

        // Apply movement
        motor.Move(velocity);

        // Calculate rotation (turning around) no looking up and down
        float vRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, vRot, 0f) * mouseSensivity;


        // Apply rotation
        motor.Rotate(rotation);

        // Calculate camera rotation (turning around) no looking up and down
        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRot * mouseSensivity;

        // Apply camera rotation
        motor.RotateCamera(cameraRotationX);


        // Calculate thruster force
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSetting(0f);
        }
        else
        {
            SetJointSetting(jointSpring);
        }

        motor.ApplyThruster(_thrusterForce);

    }

    private void SetJointSetting(float _jointSpring)
    {
        joint.yDrive = new JointDrive 
        { 
            positionSpring = _jointSpring, 
            maximumForce = jointMaxForce
        };
    }
}
