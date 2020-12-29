using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetMouseButton(0)) {
            MoveToCursor();
        }

        // GetComponent<NavMeshAgent>().destination = target.position;
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        // show click target
        // Debug.DrawRay(ray.origin, ray.direction * 100);

        // if ray intersects with a collider => move to click target
        if(hasHit) {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}
