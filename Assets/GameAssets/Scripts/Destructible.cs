using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject destroyedVersion; // Reference to the shattered version of the object
    float force = 6f;
    public int doorHP;

    // If the player clicks on the object
    private void OnCollisionEnter(Collision collisionInfo)
    {
        //if (collisionInfo.collider.tag == "Object" && gameObject.tag == "Door" || collisionInfo.collider.tag == "ObjectStrong" && gameObject.tag == "Door")
        //{
        //    OnDestroy();
        //    Instantiate(destroyedVersion, transform.position, transform.rotation);
        //}

        //if (collisionInfo.collider.tag == "Door" && gameObject.tag == "Object" || collisionInfo.collider.tag == "DoorStrong" && gameObject.tag == "Object")
        //{
        //    OnDestroy();
        //    Instantiate(destroyedVersion, transform.position, transform.rotation);
        //}

        //if (collisionInfo.collider.tag == "DoorStrong" && gameObject.tag == "ObjectStrong")
        //{
        //    OnDestroy();
        //    Instantiate(destroyedVersion, transform.position, transform.rotation);
        //}

        //if (collisionInfo.collider.tag == "ObjectStrong" && gameObject.tag == "DoorStrong")
        //{
        //    OnDestroy();
        //    Instantiate(destroyedVersion, transform.position, transform.rotation);
        //}

        
    }

    public void damageDoor()
    {
        doorHP = doorHP - 10;

        if (doorHP == 0)
        {
            OnDestroy();
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            gameManager.GameLost();
        }
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(100, 100, 100) * force);
    }

}
