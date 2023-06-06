using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField]
    private Arrow arrowPrefab;

    [SerializeField]
    private Transform spawnPoint;

    private Arrow currentArrow;

    bool isStrafe = false;
    bool isEquipped = false;
    bool canDraw = true;
    public bool isBowDrawed = false;
    bool canShoot = false;
    bool canDodge = true;
    public bool IsInputEnabled = true;

    public float firePower;
    float normalFOV;
    public float focusFOV;
    public float spineRotation;

    Camera mainCam;
    Animator anim;
    public GameObject weaponOnHand;
    public GameObject weaponOnBack;
    public GameObject spine;
    public GameObject crosshair;
    //public GameObject arrow;

    private void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
        normalFOV = mainCam.fieldOfView;
    }

    void Update()
    {

        Debug.Log(canDodge);
        Debug.Log(canDraw);
        anim.SetBool("isStrafe", isStrafe);
        anim.SetBool("isEquipped", isEquipped);
        anim.SetBool("isBowDrawed", isBowDrawed);
        anim.SetBool("canShoot", canShoot);

        if (Input.GetKeyDown(KeyCode.Q) && !isStrafe)
        {
            isEquipped = !isEquipped;
        }

        if (isEquipped && Input.GetKeyDown(KeyCode.F) && IsInputEnabled && !isBowDrawed)
        {
            isStrafe = !isStrafe;
            strafe();
        }

        if (isStrafe && Input.GetKeyDown(KeyCode.Mouse0) && canDraw && IsInputEnabled)
        {
            isBowDrawed = !isBowDrawed;
            canDraw = !canDraw;

            if (isBowDrawed)
            {
                canShoot = !canShoot;
                currentArrow = Instantiate(arrowPrefab, spawnPoint);
                crosshair.SetActive(true);
                //spine.transform.rotation = Quaternion.Slerp(spine.transform.rotation, Quaternion.Euler(spine.transform.rotation.x, spine.transform.rotation.y + spineRotation, spine.transform.rotation.z), Time.fixedDeltaTime);

                //currentArrow.transform.SetParent(rightHand);
                //currentArrow.transform.localPosition = Vector3.zero;
                //arrow.SetActive(true);
            }
            else
            {
                crosshair.SetActive(false);
            }

        }

        if (isStrafe && Input.GetKeyDown(KeyCode.Mouse1) && isBowDrawed && IsInputEnabled)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, focusFOV, Time.deltaTime * 2);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFOV, Time.deltaTime * 2);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            if (isStrafe)
            {
                GetComponent<CharacterController>().movementType = CharacterController.MovementType.Directional;
                //GetComponent<IKLook>().maximize();
            }

            anim.SetTrigger("hasDodged");

            canDraw = !canDraw;
            IsInputEnabled = !IsInputEnabled;

            //if (isBowDrawed)
            //{
            //    canShoot = !canShoot;
            //    anim.SetBool("canShoot", canShoot);
            //}

            anim.SetBool("canDodge", canDodge);
            canDodge = !canDodge;
        }

        
    }

    void strafe()
    {
        if (isStrafe)
        {
            GetComponent<CharacterController>().movementType = CharacterController.MovementType.Strafe;
            //GetComponent<IKLook>().minimalize();
        }
        else
        {
            GetComponent<CharacterController>().movementType = CharacterController.MovementType.Directional;
            //GetComponent<IKLook>().maximize();
        }
    }

    void equip()
    {
        weaponOnBack.SetActive(false);
        weaponOnHand.SetActive(true);
    }

    void unequip()
    {
        weaponOnBack.SetActive(true);
        weaponOnHand.SetActive(false);
    }

    void dodge()
    {
        if (isStrafe)
        {
            GetComponent<CharacterController>().movementType = CharacterController.MovementType.Strafe;
            //GetComponent<IKLook>().minimalize();
        }
        
        anim.SetBool("canDodge", canDodge);
        canDodge = !canDodge;

        canDraw = !canDraw;
        IsInputEnabled = !IsInputEnabled;

        //if (isBowDrawed)
        //{
        //    canShoot = !canShoot;
        //    anim.SetBool("canShoot", canShoot);
        //}

    }

    void shoot()
    {
        canShoot = !canShoot;
        anim.SetBool("canShoot", canShoot);

        

        var force = spawnPoint.TransformDirection(Vector3.up * firePower);
        currentArrow.arrowFly(force);
        currentArrow = null;
        //arrow.SetActive(false);
    }

    void drawBow()
    {
        canDraw = !canDraw;
    }
}
