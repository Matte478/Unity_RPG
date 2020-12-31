using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour 
    {
        void Update()
        {   
            if(Input.GetMouseButton(0)) {
                MoveToCursor();
            }
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
                GetComponent<Mover>().MoveTo(hit.point);
            }
        }
    }
}