using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trazan_coin : MonoBehaviour
{
    public GameObject G_Effect;
    GameObject Dummy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            // Debug.Log("Clone Effect");
            Tarzan_Main.Instance.THI_CoinCollect();
            Dummy = Instantiate(G_Effect, this.transform.position,Quaternion.identity);
            Dummy.transform.SetParent(this.transform.parent.transform, false);
            Dummy.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }


}
