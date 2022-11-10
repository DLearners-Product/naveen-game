using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerdeath : MonoBehaviour
{
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            HC_Controller.Instance.THI_PlayerDead();
            Debug.Log("Player Death");
        }
    }
}
