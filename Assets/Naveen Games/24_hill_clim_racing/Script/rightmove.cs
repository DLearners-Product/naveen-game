using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class rightmove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        HC_Controller.Instance.B_right = true;
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        HC_Controller.Instance.B_right = false;
    }
}
