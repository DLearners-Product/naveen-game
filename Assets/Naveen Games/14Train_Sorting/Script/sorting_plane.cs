using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorting_plane : MonoBehaviour
{
    public float F_Speed,F_Movement;
    public bool B_CanFly, B_MoveUp, B_MoveDown, B_CanDrop, B_CallOnce;
    public GameObject G_Crate;
    public Vector3 V3_up, V3_down;
    public AudioSource AS_in;
    GameObject G_Dummy;
   // bool B_ClickOnce;
    private void Start()
    {
        B_CallOnce = true;
        B_MoveUp = true;
        B_MoveDown = false;
        B_CanDrop = false;
        B_CanFly = true;
      //  B_ClickOnce = true;
        AS_in.Play();
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D Hit = Physics2D.Raycast(worldpoint, Vector2.zero);
           
            if(Hit.collider!=null)
            {
                if(!Main_trainsorting.OBJ_Main_trainsorting.G_instructionPage.activeInHierarchy)
                {
                    if (Hit.collider.name == "TS_Plane(Clone)")
                    {
                        if(Main_trainsorting.OBJ_Main_trainsorting.B_Spawn)
                        {
                            // if(B_ClickOnce)
                            // {
                            //   B_ClickOnce = false;
                            this.GetComponent<Collider2D>().enabled = false;
                                Hit.collider.GetComponent<sorting_plane>().B_CanDrop = true;
                           // }
                           
                        }
                       
                        if (B_CanDrop)
                        {
                            string check = Main_trainsorting.OBJ_Main_trainsorting.STR_currentQuestionAnswer;
                            G_Dummy = Instantiate(G_Crate);
                            G_Dummy.transform.position = this.transform.position;
                            G_Dummy.name = check;
                            B_CanFly = false;
                            Invoke("THI_Destroy", 8f);
                        }
                    }

                    if (Hit.collider.name == "TS_Text")
                    {
                        Hit.collider.GetComponent<AudioSource>().Play();
                    }
                }
               
            }
        }
        
        if (B_CanFly)
        {
            transform.Translate(Vector3.left * F_Speed * Time.deltaTime);
          
            if(B_MoveUp)
            {
                if (this.transform.position.y < V3_up.y)
                {
                    transform.Translate(Vector3.up * F_Movement * Time.deltaTime);
                }
                else
                {
                    B_MoveUp = false;
                    B_MoveDown = true;
                }
            }
            
            if(B_MoveDown)
            {
                if(this.transform.position.y > V3_down.y)
                {
                    transform.Translate(Vector3.down * F_Movement * Time.deltaTime);
                }
                else
                {
                    B_MoveUp = true;
                    B_MoveDown = false;
                }
            }
        }
        else
        {
            if(B_CallOnce)
            {
                THI_planeaway();
                B_CallOnce = false;
            }
            transform.Translate(Vector3.left * 8 * Time.deltaTime);
        }
    }

    void THI_Destroy()
    {
        Destroy(G_Dummy);
    }
    void THI_planeaway()
    {
        Invoke("THI_OFFAudioSource", 0.5f);
       // AS_in.Stop();
       // AS_out.Play();
        B_CanDrop = false;
    }
    void THI_OFFAudioSource()
    {
        AS_in.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Destroy_plane")
        {
            Destroy(G_Dummy);
            Destroy(this.gameObject);
        }
        if (collision.name == "Out_of_screen")
        {
            if(B_CallOnce)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
