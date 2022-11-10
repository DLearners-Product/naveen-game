using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike_Rotate : MonoBehaviour
{
   // public float Angle;
    public bool B_FinishRace;

    private void Start()
    {
        B_FinishRace = false;
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Stop _race")
        {
            if(this.transform.parent.name=="Player")
            {
                Racing_Main.Instance.I_Player++;
            }
            else
            if (this.transform.parent.name == "Enemy")
            {
                Racing_Main.Instance.I_Enemy++;
            }
           // I_Race++;
            if (B_FinishRace)
            {
               // if()
                Racing_Main.Instance.Race_Complete(this.transform.parent.name);
            }
            
        }
    }
}
