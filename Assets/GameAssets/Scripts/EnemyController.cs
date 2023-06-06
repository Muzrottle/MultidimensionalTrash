using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    [Header("Metrics")]
    public float damp;
    [Range(1, 20)]
    public float rotationSpeed;
    [Range(1, 20)]
    public float strafeTurnSpeed;

    public float lookAtSpeed = 5f;

    //float normalFOV;
    //public float sprintFOV;
    public int HP;
    public float rotation = 5f;
    float inputX;
    float inputY;
    float maxSpeed;
    public float sensitivity = -1f;
    public bool isRunner;
    bool isStrafe = false;
    bool isEquipped = true;
    bool isFacing = false;


    public Transform model;
    Transform targetObjectTransform;

    public GameObject targetObject;

    //Camera mainCam;
    Animator anim;
    EnemyController enemyController;
    public Rigidbody[] rigidbodies;
    Vector3 stickDirection;
    Vector3 rotate;

    //public KeyCode SprintButton = KeyCode.LeftShift;

    public enum MovementType
    {
        Directional,
        Strafe,
        Ragdoll
    };

    public MovementType movementType;

    void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        DisableRagdoll();
    }

    // Start is called before the first frame update
    void Start()
    {
        //mainCam = Camera.main;
        //normalFOV = mainCam.fieldOfView;
        //Cursor.lockState = CursorLockMode.Locked;
        targetObject = GameObject.Find("Gate Door");

        targetObjectTransform = targetObject.transform;

        if (isRunner)
        {
            GetComponent<EnemyController>().movementType = EnemyController.MovementType.Directional;
        }
        else
        {
            GetComponent<EnemyController>().movementType = EnemyController.MovementType.Strafe;
        }
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Debug.Log(inputX);
        Debug.Log("Y: " + inputY);

        if (isFacing)
        {
            Vector3 direction = targetObjectTransform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lookAtSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.name == "TargetDoor")
        {
            Debug.Log("Collided");
            isFacing = true;
            isStrafe = true;
            inputX = 0f;
            inputY = 1f;
            GetComponent<EnemyController>().movementType = EnemyController.MovementType.Strafe;
        }

        if (collider.name == "AttackDoor")
        {
            Debug.Log("Collided");
            inputX = 0f;
            inputY = 0f;
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            }
            //GetComponent<EnemyController>().movementType = EnemyController.MovementType.Directional;
        }
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        anim.enabled = true;
        enemyController.enabled = true;
    }

    void Movement()
    {
        anim.SetBool("isStrafe", isStrafe);
        anim.SetBool("isEquipped", isEquipped);

        if (movementType == MovementType.Strafe)
        {
            //float cameraRotation = mainCam.transform.rotation.eulerAngles.y;

            //Quaternion targetRotation = Quaternion.Euler(0f, cameraRotation, 0f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //inputX = Input.GetAxis("Horizontal");
            //inputY = Input.GetAxis("Vertical");


            anim.SetFloat("inputX", inputX, damp, Time.deltaTime * 10);
            anim.SetFloat("inputY", inputY, damp, Time.deltaTime * 10);

            var moving = inputX != 0 || inputY != 0;

            if (moving)
            {
                //float yawCamera = mainCam.transform.rotation.eulerAngles.y;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), strafeTurnSpeed * Time.fixedDeltaTime);

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
            //InputRotation();

            stickDirection = new Vector3(inputX, 0, inputY);

            if (isRunner && !isFacing/*Input.GetKey(SprintButton)*/)
            {
                //mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, sprintFOV, Time.deltaTime * 2);

                maxSpeed = 0.9f;
                inputX = 0.9f * 1;
                inputY = 0.9f * 1;
            }
            else
            {
                //mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFOV, Time.deltaTime * 2);

                maxSpeed = 0f;
                inputX = 0f * 1;
                inputY = 0f * 1;
            }
        }

        if (movementType == MovementType.Ragdoll)
        {
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.isKinematic = false;
            }

            anim.enabled = false;
            enemyController.enabled = false;
        }

    }

    void InputMove()
    {
        anim.SetFloat("speed", Vector3.ClampMagnitude(stickDirection, maxSpeed).magnitude, damp, Time.deltaTime * 10);
    }

    //void InputRotation()
    //{
    //    Vector3 rotOfset = mainCam.transform.TransformDirection(stickDirection);
    //    rotOfset.y = 0;

    //    model.forward = Vector3.Slerp(model.forward, rotOfset, Time.deltaTime * rotationSpeed);
    //}
}
