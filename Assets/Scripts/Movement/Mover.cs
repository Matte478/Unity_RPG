using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void LateUpdate()
    {   
        if(Input.GetMouseButton(0)) {
            MoveToCursor();
        }
        UpdateAnimator();

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

    private void UpdateAnimator()
    {
        // convert global to local velocity
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
}
