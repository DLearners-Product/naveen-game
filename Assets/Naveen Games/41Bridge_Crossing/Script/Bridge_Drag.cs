using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bridge_Drag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 mousePos;
    public Vector2 initalPos;

    [SerializeField]
    GameObject otherGameObject;

    Camera mainCam;
   // public AudioSource AS_Effect;

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("gameCam").GetComponent<Camera>();
    }

    private void Start()
    {
        initalPos = this.transform.position;
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (otherGameObject != null)
        {
            if (otherGameObject.transform.childCount == 0)
            {
                if (this.transform.parent.name == "G_optParent")
                {
                    Bridge_Main.Instance.THI_DroppedValue(true);
                }
               
                this.transform.SetParent(otherGameObject.transform, false);
                this.transform.position = otherGameObject.transform.position;
              //  Debug.Log(this.name + " " + otherGameObject.name);
               
               
            }
            else
            {
                if (this.transform.parent.name != "G_optParent")
                {
                    Bridge_Main.Instance.THI_DroppedValue(false);
                }
                this.transform.SetParent(Bridge_Main.Instance.G_Options_Parent.transform, false);
                Bridge_Main.Instance.THI_ONLayout();
              //  Debug.Log("NO OtherGameObject");
            }
        }
        else
        {
            if (this.transform.parent.name != "G_optParent")
            {
                Bridge_Main.Instance.THI_DroppedValue(false);
            }
            this.transform.SetParent(Bridge_Main.Instance.G_Options_Parent.transform,false);
            Bridge_Main.Instance.THI_ONLayout();
           // Debug.Log("NO OtherGameObject");
           
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.name == "Placeholder")
        {
            otherGameObject = other.gameObject;
           // Debug.Log(otherGameObject.name);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.name == "Placeholder")
        {
            otherGameObject = null;
        }
    }
}
