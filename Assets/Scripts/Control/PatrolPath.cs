using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDrawGizmos()
        {
            // loops through all the waypoint and draws a sphere and lines between them
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextIndex(i)));
            }
        }

        public Vector3 GetWaypointPosition(int index)
        {
            return transform.GetChild(index).position;
        }

        public int GetNextIndex(int index)
        {
            index++;
            return index < transform.childCount ? index : 0;
        }
    }
}
