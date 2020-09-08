using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Drive DriveScript;
    // Start is called before the first frame update
    void Start()
    {
        DriveScript = gameObject.GetComponent<Drive>(); 
    }

    void Update()
    {
        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");
        DriveScript.Go(a, s, b);

        DriveScript.CheckForSkid();
        DriveScript.CalculateEngineSound();
    }
}
