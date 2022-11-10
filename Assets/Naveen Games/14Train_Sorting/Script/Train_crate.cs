using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_crate : MonoBehaviour
{
    public GameObject G_Blast,G_Coins;
    GameObject G_Dummy;
    bool B_CallOnce;
    private void Start()
    {
        B_CallOnce = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.collider!=null && collision.collider.name!="TS_Text")
       {
           // Debug.Log(collision.collider.name);
            if(collision.collider.transform.GetChild(0).gameObject!=null)
            {
               // Debug.Log(collision.collider.transform.GetChild(0).gameObject.name);
                Main_trainsorting.OBJ_Main_trainsorting.STR_currentSelectedAnswer = collision.collider.transform.GetChild(0).name;
            }
          
            if (collision.collider.transform.GetChild(0).gameObject.name == Main_trainsorting.OBJ_Main_trainsorting.STR_currentQuestionAnswer)
            {
                if(B_CallOnce)
                {
                    GameObject coins = Instantiate(G_Coins);
                    coins.transform.position = collision.collider.transform.position;
                    this.GetComponent<AudioSource>().Play();
                    Main_trainsorting.OBJ_Main_trainsorting.THI_Correct();
                    B_CallOnce = false;
                }
               
            }
            else
           // if (collision.collider.name != Main_trainsorting.OBJ_Main_trainsorting.STR_currentQuestionAnswer)
            {
                if (B_CallOnce)
                {
                    Main_trainsorting.OBJ_Main_trainsorting.THI_Wrong();
                    this.GetComponent<SpriteRenderer>().enabled = false;
                    G_Dummy = Instantiate(G_Blast);
                    G_Dummy.transform.position = this.transform.position;
                    Invoke("THI_Destroy", 3f);
                    B_CallOnce = false;
                }
                
            }
       }
    }

    void THI_Destroy()
    {
        Destroy(G_Dummy);
        Destroy(this.gameObject);
    }    
}
