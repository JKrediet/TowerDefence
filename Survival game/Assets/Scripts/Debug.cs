using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class Debug : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // Use this for initialization
    public void OnPointerDown(PointerEventData eventData)
    {
        print("Pointer is Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Pointer is Up");
    }
}