using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACI_BG : MonoBehaviour
{
    void Update()
    {
        if(ACI_Main.Instance!=null)
        {
            if (ACI_Main.Instance.B_MoveBG) { transform.Translate(Vector3.left * 1.5f * Time.deltaTime); }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="Stop_BG")
        {
            ACI_Main.Instance.THI_BGStop();
        }
    }
}
