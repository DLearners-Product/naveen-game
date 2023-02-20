using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator Anim;
    bool B_CallOnce;
    public bool B_questionALlocated;
    public Transform spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        B_questionALlocated = false;
        Anim = this.GetComponent<Animator>();
        OffAnim();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      //  Debug.Log("BOol Calling Outside");
        if (B_CallOnce)
        {
            collision.gameObject.GetComponent<Tarzan_Player>().AS_Jump.Play();
           // Debug.Log("BOol Calling");
            B_CallOnce = false;
           // Anim.enabled = true;
            Anim.Play("effect");
            Invoke(nameof(OffAnim), 2f);
        }
    }

    void OffAnim()
    {
       // Debug.Log("BOol Calling Off");
        B_CallOnce = true;
        Anim.Play("New State");
        //  Anim.enabled = false;
    }
}
