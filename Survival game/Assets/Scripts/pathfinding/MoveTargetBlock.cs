using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveTargetBlock : MonoBehaviour
{
    // public
    public LayerMask hitlayers;
    void Update()
    {
        if (Input.GetButtonDown("fire1"))
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if(Physics.Raycast(castPoint,out hit, Mathf.Infinity, hitlayers))
            {
                this.transform.position = hit.point;
            }
        }
    }
}