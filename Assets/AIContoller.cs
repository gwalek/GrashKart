using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIContoller : MonoBehaviour
{

    public Circuit circuit;
    public Drive drive;
    public float steeringSensitivity = 0.01f;
    Vector3 target;
    int currentWayPoint = 0; 


    // Start is called before the first frame update
    void Start()
    {
        drive = gameObject.GetComponent<Drive>();
        target = circuit.Waypoints[currentWayPoint].transform.position; 
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localTarget = drive.gameObject.transform.InverseTransformDirection(target);
        float distanceToTarget = Vector3.Distance(target, drive.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drive.currentSpeed);
        float accel = 0.5f;
        float brake = 0;

        if (distanceToTarget < 5) { brake = 0.8f; accel = 0.1f; }

        drive.Go(accel, steer, brake);

        if (distanceToTarget < 2) //threshold, make larger if car starts to circle waypoint
        {
            currentWayPoint++;
            if (currentWayPoint >= circuit.Waypoints.Length)
                currentWayPoint = 0;
            target = circuit.Waypoints[currentWayPoint].transform.position;
        }

        drive.CheckForSkid();
        drive.CalculateEngineSound();
    }
}

