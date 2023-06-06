using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : MonoBehaviour
{
    public GameObject bowString;
    public Transform idlePos;
    public Transform drawPos;

    // Start is called before the first frame update
    public void stringPull()
    {
        bowString.transform.position = drawPos.position;
        bowString.transform.SetParent(drawPos);
    }

    // Update is called once per frame
    public void stringNotPull()
    {
        bowString.transform.position = idlePos.position;
        bowString.transform.SetParent(idlePos);
    }
}
