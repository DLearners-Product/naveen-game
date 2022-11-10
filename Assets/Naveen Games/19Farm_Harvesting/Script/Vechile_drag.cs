using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Vechile_drag : MonoBehaviour
{
    Vector2 mousePos;
    public Vector2  PreviousPos;
    Rigidbody2D RB;
    GameObject G_Boundry;
    public Sprite[] SPR_Vechiles;
    Camera mainCam;
    bool B_CanMove;
    public AudioSource AS_Cutting;
    public GameObject SPR_Farmer;
    private void Awake()
    {
        mainCam = Camera.main;
        RB = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SPR_Farmer.SetActive(true);
        G_Boundry = null;
    }


    private void Update()
    {
        /*if(Input.GetMouseButton(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.name == "Drag")
                {
                    G_player = hit.collider.gameObject;
                    G_player.transform.position = worldPoint;
                    Vector3 tmpPos = G_player.transform.position;
                   // tmpPos.x = Mathf.Clamp(tmpPos.x, -3.0f, 6f);  -1.55
                   // tmpPos.y = Mathf.Clamp(tmpPos.y, -5.0f, 5.0f);
                   // G_player.transform.position = tmpPos;
                }
            }
        }*/
        if(B_CanMove)
        {
            if(G_Boundry==null)
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Vector2 temp = PreviousPos - worldPoint;

                if (PreviousPos.x < worldPoint.x && PreviousPos.y > worldPoint.y)
                {
                    // this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = SPR_Vechiles[0];
                    this.GetComponent<SpriteRenderer>().sprite = SPR_Vechiles[0];
                }
                else

                if (PreviousPos.x > worldPoint.x && PreviousPos.y < worldPoint.y)
                {
                    this.GetComponent<SpriteRenderer>().sprite = SPR_Vechiles[1];
                }
                else

                if (PreviousPos.x > worldPoint.x && PreviousPos.y > worldPoint.y)
                {
                    this.GetComponent<SpriteRenderer>().sprite = SPR_Vechiles[3];
                }
                else

                if (PreviousPos.x < worldPoint.x && PreviousPos.y < worldPoint.y)
                {
                    this.GetComponent<SpriteRenderer>().sprite = SPR_Vechiles[2];
                }



                this.transform.position = new Vector3(worldPoint.x, worldPoint.y, -10f);
                PreviousPos = this.transform.position;
            }
           
        }
        
    }

    private void OnMouseDown()
    {
        SPR_Farmer.SetActive(false);
        if (G_Boundry != null)
        {
            B_CanMove = false;
          //  Debug.Log("Cannot Move");
        }
        else
        {
            B_CanMove = true;
          //  Debug.Log("Can Move");
        }

    }
    private void OnMouseUp()
    {
        B_CanMove = false;
        G_Boundry = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Boundry")
        {
            G_Boundry = collision.gameObject;
            B_CanMove = false;
        }
        else
        if (collision.collider.name == "Vegtables")
        {
            AS_Cutting.Play();
           // Debug.Log("Hitting veg");
            collision.collider.gameObject.SetActive(false);
            Farm_Main.Instance.THI_Counter();
        }
    }
   

}
