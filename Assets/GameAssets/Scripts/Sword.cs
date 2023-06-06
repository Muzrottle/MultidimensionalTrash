using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Gate Door")
        {
            var door = collider.transform.root.GetComponent<Destructible>();
            door.damageDoor();
        }
    }
}
