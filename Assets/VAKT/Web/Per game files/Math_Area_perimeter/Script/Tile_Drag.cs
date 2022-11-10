using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile_Drag : MonoBehaviour
{
    Vector2 mousePos;
    public Vector2 initalPos;

    float F_diff_X, F_diff_Y;
    bool isdrag;
    GameObject otherGameObject=null;

    [SerializeField] Camera Cam;
    bool B_Drag = false;
    private void Awake()
    {
        Cam = GameObject.FindGameObjectWithTag("gameCam").GetComponent<Camera>();
    }

    private void Start()
    {
        
           initalPos = this.transform.position;
    }

    private void Update()
    {
        if(B_Drag)
        {
            Vector2 worldPoint = Cam.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(worldPoint.x, worldPoint.y, -10f);
        }
    }

    private void OnMouseDown()
    {
        B_Drag = true;
        Debug.Log("Move" + B_Drag);
    }
    private void OnMouseUp()
    {
         B_Drag = false;
        Debug.Log("drop" + B_Drag);
        Place();
    }
    void Place()
    {
        if (otherGameObject == null)
        {
            this.transform.position = initalPos;
        }
        else
        {
            if (otherGameObject.transform.parent.name == "GridManager" && otherGameObject.transform.childCount == 1)
            {
                //  if(Grid_Manager.Instance.I_Count==0 && !B_Drag)
                //  {
                if (this.transform.parent.name == "DragElements")
                {
                    Grid_Manager.Instance.IncreaseArea();
                }
                this.transform.SetParent(otherGameObject.transform, false);
                this.transform.position = otherGameObject.transform.position;
                Grid_Manager.Instance.G_LastObject = otherGameObject;
            }
            else
            {
                this.transform.SetParent(otherGameObject.transform, false);
                this.transform.position = otherGameObject.transform.position;
                Grid_Manager.Instance.G_LastObject = otherGameObject;
            }


        }
            
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // if(collision.ga)
        otherGameObject = collision.gameObject;
        Debug.Log(otherGameObject.name);
    }
   
}
