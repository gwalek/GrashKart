using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public GameObject[] Waypoints;

    public void OnDrawGizmos()
    {
        DrawGizmos(false);
    }

    public void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }

    public void DrawGizmos(bool selected)
    {
        if (!selected || (Waypoints == null))
        {
            return; 
        }

        if (Waypoints.Length > 1)
        {
            Vector3 previous = Waypoints[0].transform.position; 
            for (int i = 1; i < Waypoints.Length; i++)
            {
                Vector3 next = Waypoints[i].transform.position;
                Gizmos.DrawLine(previous, next);
                previous = next; 

            }
            Gizmos.DrawLine(previous, Waypoints[0].transform.position);

        }
    }
}
