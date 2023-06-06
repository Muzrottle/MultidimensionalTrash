using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLook : MonoBehaviour
{
    Animator anim;
    Camera mainCam;

    float weight = 1f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(weight, 1f, 0.8f, 0.5f, 0.7f);
        Ray lookAtRay = new Ray(transform.position, mainCam.transform.forward);

        anim.SetLookAtPosition(lookAtRay.GetPoint(25));
    }

    public void minimalize()
    {
        weight = Mathf.Lerp(weight, 0, Time.deltaTime);
    }

    public void maximize()
    {
        weight = Mathf.Lerp(weight, 1f, Time.deltaTime);
    }
}
