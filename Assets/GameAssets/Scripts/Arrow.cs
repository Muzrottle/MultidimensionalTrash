using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float torque;

    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Collider arrowHead;

    [SerializeField]
    private Collider arrowbody;


    private bool didHit = false;

    public void arrowFly(Vector3 force)
    {
        StartCoroutine(arrowCollider(force));
    }


    IEnumerator arrowCollider(Vector3 force)
    {

        transform.position.Set(0,0,0);
        transform.rotation.Set(0,0,0,0);
        //rigidbody.inertiaTensorRotation = Quaternion.identity;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force, ForceMode.Impulse);
        rigidbody.AddTorque(transform.forward * torque);
        transform.SetParent(null);
        yield return new WaitForSeconds(0.01f);
        arrowHead.enabled = true;
        arrowbody.enabled = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        //if (didHit)
        //    return;
        //else
        //    didHit = true;

        //if (!didHit)
        //{
        //    // Check if the arrow collided with the enemy's body part
        //    if (collider.gameObject.CompareTag("EnemyBodyPart"))
        //    {

        //    }
        //}

        // Disable physics simulation for the arrow
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        if (collider.CompareTag("RagdollBodyPart")) 
        {
            var health = collider.transform.root.GetComponent<HealthController>();
            var character = collider.transform.root.GetComponent<EnemyController>();
            health.ApplyDamage(10, character.HP);
            //Rigidbody hitRigidbody = character.rigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, collider.transform.position)).First();
            //print(hitRigidbody);
            //transform.SetParent(hitRigidbody.transform);

            //Get a reference to the body part's rigidbody
            Rigidbody bodyPartRigidbody = collider.GetComponent<Rigidbody>();

            //Reparent the arrow to the body part
            transform.SetParent(bodyPartRigidbody.transform);


            ////Get a reference to the rigidbody of the closest ragdoll body part
            //Rigidbody closestBodyPartRigidbody = null;
            //float closestDistance = Mathf.Infinity;
            //foreach (var bodyPartRigidbody in collider.transform.root.GetComponentsInChildren<Rigidbody>())
            //{
            //    float distance = Vector3.Distance(transform.position, bodyPartRigidbody.position);
            //    if (distance < closestDistance)
            //    {
            //        closestBodyPartRigidbody = bodyPartRigidbody;
            //        closestDistance = distance;
            //    }
            //}

            //if (closestBodyPartRigidbody != null)
            //{
            //    // Reparent the arrow to the closest ragdoll body part
            //    transform.SetParent(closestBodyPartRigidbody.transform);
            //}


            // Set the flag to prevent further collision handling
            didHit = true;
        }

        
        
        


        //hitRigidbody.AddForceAtPosition(10, transform.position, ForceMode.Impulse);

        arrowHead.enabled = false;
        arrowbody.enabled = false;
    }
}
