using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DA_Drag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 mousePos;
    public Vector2 initalPos;

   
    GameObject otherGameObject;

    Camera mainCam;
    public AudioSource AS_Effect;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        initalPos = this.transform.position;
    }
    

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
      //
      //Debug.Log("Dragging = "+ this.name);
        DA_Main.Instance.G_Optionpos.GetComponent<HorizontalLayoutGroup>().enabled = false;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (otherGameObject != null)
        {
            if(otherGameObject.transform.childCount==0)
            {
                this.gameObject.GetComponent<AudioSource>().Play();
                if (this.transform.parent.name == "Sorting")
                {
                    DA_Main.Instance.THI_Counter();
                   
                }
                this.transform.SetParent(otherGameObject.transform, false);
                this.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                this.transform.position = otherGameObject.transform.position;
                AS_Effect.Play();
               
                // otherGameObject.GetComponent<Collider2D>().enabled = false;
                // this.GetComponent<DA_Drag>().enabled = false;
            }
            else
            {
                if(this.transform.parent.transform.parent.name=="Group")
                {
                    DA_Main.Instance.I_DroppedCount--;
                   // DA_Main.Instance.G_Check.GetComponent<Button>().interactable = false;
                }
                this.transform.position = initalPos;
                this.transform.SetParent(DA_Main.Instance.G_Optionpos.transform, false);
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                DA_Main.Instance.G_Optionpos.GetComponent<HorizontalLayoutGroup>().enabled = true;
            }
            
        }
        else
        {
            if (this.transform.parent.transform.parent.name == "Group")
            {
                DA_Main.Instance.I_DroppedCount--;
                DA_Main.Instance.G_Check.GetComponent<Button>().interactable = false;
            }
            this.transform.position = initalPos;
            this.transform.SetParent(DA_Main.Instance.G_Optionpos.transform, false);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            DA_Main.Instance.G_Optionpos.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.transform.parent.name == "Drop")
        {
            otherGameObject = other.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.transform.parent.name == "Drop")
        {
            otherGameObject = null;
        }
    }



}