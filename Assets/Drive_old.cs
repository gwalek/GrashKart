using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive_old : MonoBehaviour
{
    public WheelCollider[] WheelColliders;
    public GameObject[] WheelMeshes;
    public float torque = 200;
    public float MaxSteering = 30;
    public float MaxBreakTorque = 500;

    public AudioSource SkidSound;
    public Transform SkidTrailPrefab;
    public float SkidDestroyTime = 30f; 
    Transform[] SkidTrails = new Transform[4];

    public ParticleSystem SmokePrefab;
    ParticleSystem[] Smoke = new ParticleSystem[4];
    public GameObject BreakLight;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Smoke[i] = Instantiate(SmokePrefab);
            Smoke[i].Stop(); 
        }
    }
    

    public void CheckForSkid()
    {
        int numSkidding = 0;
        for (int i = 0; i < 4; i++)
        {
            WheelHit wHit;
            WheelColliders[i].GetGroundHit(out wHit);

            if ( (Mathf.Abs(wHit.forwardSlip) >= 0.4f ) || (Mathf.Abs(wHit.sidewaysSlip) >= 0.4f ) )
            {
                numSkidding++; 
                if (!SkidSound.isPlaying)
                {
                    SkidSound.Play(); 
                }
                StartSkidTrails(i);
                Smoke[i].transform.position = WheelColliders[i].transform.position - WheelColliders[i].transform.up * WheelColliders[i].radius; 
                Smoke[i].Emit(1);
            }
            else
            {
                EndSkidTrails(i);
            }

            if ((numSkidding == 0) && (SkidSound.isPlaying))
            {
                SkidSound.Stop(); 
            }
        }

    }

    public void StartSkidTrails(int i)
    {
        if (SkidTrails[i] == null)
        {
            SkidTrails[i] = Instantiate(SkidTrailPrefab); 
        }

        SkidTrails[i].parent = WheelColliders[i].transform;
        SkidTrails[i].rotation = Quaternion.Euler(90, 0, 0);
        SkidTrails[i].localPosition = -Vector3.up * WheelColliders[i].radius; 
    }

    public void EndSkidTrails(int i)
    {
        if (SkidTrails[i] == null)
        {
            return;
        }

        Transform Holder = SkidTrails[i];

        SkidTrails[i] = null;
        Holder.parent = null;
        Holder.rotation = Quaternion.Euler(0, 90, 0);
        Destroy(Holder.gameObject, SkidDestroyTime);
    }


    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * MaxSteering;
        brake = Mathf.Clamp(brake, 0, 1) * MaxBreakTorque;

        if (brake != 0)
        {
            BreakLight.SetActive(true);
        }
        else
        {
            BreakLight.SetActive(false);
        }

        float thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        { 
            WheelColliders[i].motorTorque = thrustTorque;
            if (i < 2)
            {
                WheelColliders[i].steerAngle = steer;
            }
            else
            {
                WheelColliders[i].brakeTorque = brake;
            }

            // Align Mesh to Collider
            Quaternion quat;
            Vector3 positon;
            WheelColliders[i].GetWorldPose(out positon, out quat);
            WheelMeshes[i].transform.position = positon;
            WheelMeshes[i].transform.rotation = quat;
        }
    }


   
}
