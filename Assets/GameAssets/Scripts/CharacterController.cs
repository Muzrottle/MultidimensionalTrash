using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Metrics")]
    public float damp;
    [Range(1, 20)]
    public float rotationSpeed;
    [Range(1, 20)]
    public float strafeTurnSpeed;

    float normalFOV;
    public float sprintFOV;
    public int HP;
    public float rotation = 5f;
    float inputX;
    float inputY;
    float maxSpeed;
    public float sensitivity = -1f;

    public Transform model;
    
    Camera mainCam;
    Animator anim;
    CharacterController _characterController;
    WeaponController _weaponController;
    public Rigidbody[] rigidbodies;
    Vector3 stickDirection;
    Vector3 rotate;
    
    public KeyCode SprintButton = KeyCode.LeftShift;

    public enum MovementType
    {
        Directional,
        Strafe,
        Ragdoll
    };

    public MovementType movementType;

    void Awake()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
        }
        anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        DisableRagdoll();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        normalFOV = mainCam.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        anim.enabled = true;
        _characterController.enabled = true;
    }

    void Movement()
    {
        if (movementType == MovementType.Strafe)
        {
            float cameraRotation = mainCam.transform.rotation.eulerAngles.y;

            Quaternion targetRotation = Quaternion.Euler(0f, cameraRotation, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");

            anim.SetFloat("inputX", inputX, damp, Time.deltaTime * 10);
            anim.SetFloat("inputY", inputY, damp, Time.deltaTime * 10);

            var moving =inputX != 0 || inputY != 0;

            if (moving)
            {
                float yawCamera = mainCam.transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,yawCamera,0), strafeTurnSpeed * Time.fixedDeltaTime);

                anim.SetBool("isStrafeMoving", true);
            }
            else
            {
                anim.SetBool("isStrafeMoving", false);
            }
        }

        if (movementType == MovementType.Directional)
        {
            InputMove();
            InputRotation();

            stickDirection = new Vector3(inputX, 0, inputY);

            if (Input.GetKey(SprintButton))
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, sprintFOV, Time.deltaTime * 2);

                maxSpeed = 1.2f;
                inputX = 1.2f * Input.GetAxis("Horizontal");
                inputY = 1.2f * Input.GetAxis("Vertical");
            }
            else
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFOV, Time.deltaTime * 2);

                maxSpeed = 0.6f;
                inputX = 0.6f * Input.GetAxis("Horizontal");
                inputY = 0.6f * Input.GetAxis("Vertical");
            }
        }

        if (movementType == MovementType.Ragdoll) 
        {
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
            }

            anim.enabled = false;
            _characterController.enabled = false;
        }

    }

    void InputMove() 
    { 
        anim.SetFloat("speed", Vector3.ClampMagnitude(stickDirection, maxSpeed).magnitude, damp, Time.deltaTime * 10);
    }

    void InputRotation()
    {
        Vector3 rotOfset = mainCam.transform.TransformDirection(stickDirection);
        rotOfset.y = 0;

        model.forward = Vector3.Slerp(model.forward, rotOfset, Time.deltaTime * rotationSpeed);
    }
}
