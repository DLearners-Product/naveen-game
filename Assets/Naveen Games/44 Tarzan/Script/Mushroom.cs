using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator Anim;
    bool B_CallOnce;
    public int I_questionID;
    // Start is called before the first frame update
    void Start()
    {
        I_questionID = 0;
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

    public void SetQuestion(int questionID){
        I_questionID = questionID;
    }

    void OffAnim()
    {
       // Debug.Log("BOol Calling Off");
        B_CallOnce = true;
        Anim.Play("New State");
        //  Anim.enabled = false;
    }
}
