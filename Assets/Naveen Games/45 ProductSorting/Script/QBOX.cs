using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBOX : MonoBehaviour
{
    public Crane_movement Crane;
    public string STR_Selected;
    public AudioSource AS_Droping;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log("Colliding");
        if (collision.gameObject != null)
        {
          //  Debug.Log("Colliding = "+ collision.gameObject.name);
            STR_Selected = collision.gameObject.name;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        STR_Selected = "";

    }

    public void BUT_DropDown()
    {
        if (STR_Selected  == "")
        {
            PS_Main.Instance.THI_Wrong();
            THIDropDown();
        }
        else
        {
            if (STR_Selected == PS_Main.Instance.STR_currentQuestionAnswer)
            {
                PS_Main.Instance.THI_Correct();
                THIDropDown();
            }
            else
            {
                PS_Main.Instance.THI_Wrong();
                THIDropDown();
            }
        }
    }

    void THIDropDown()
    {
        Crane.FixedJoystick.gameObject.SetActive(false);
        Crane.G_Button.SetActive(false);
        PS_Main.Instance.STR_currentSelectedAnswer = STR_Selected;
        this.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name=="Opt 2"|| collision.gameObject.name=="Opt 3")
        {
            Debug.Log(collision.gameObject.name);
            AS_Droping.Play();
        }
    }
}
