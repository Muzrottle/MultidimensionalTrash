using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 100f;
    public float rotationSpeed = 10f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
}
