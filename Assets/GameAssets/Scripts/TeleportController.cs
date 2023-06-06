using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportController : MonoBehaviour
{
    public string levelName;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Player")
        {
            SceneManager.LoadScene(levelName);
        }
        
    }
}
