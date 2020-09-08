using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAt : MonoBehaviour
{

    public GameObject LookatObject; 

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(LookatObject.transform); 
    }
}
