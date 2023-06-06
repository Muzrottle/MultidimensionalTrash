using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    EnemyController enemyController;
    public GameObject enemyWeapon;
    Rigidbody rb;
    BoxCollider weaponCollider;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        rb = enemyWeapon.GetComponent<Rigidbody>();
        weaponCollider = enemyWeapon.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    public void ApplyDamage(int Damage, int Health)
    {
        enemyController.HP = Health - Damage;
        print(enemyController.HP);

        if (enemyController.HP == 0) 
        {
            enemyController.movementType = EnemyController.MovementType.Ragdoll;
            enemyWeapon.transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            StartCoroutine(weaponFreeze());
        }
    }

    IEnumerator weaponFreeze()
    {
        yield return new WaitForSeconds(4f);
        rb.isKinematic = true;
        weaponCollider.enabled = false;
    }
}
