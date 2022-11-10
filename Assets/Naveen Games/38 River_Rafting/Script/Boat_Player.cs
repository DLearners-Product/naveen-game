using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_Player : MonoBehaviour
{
   public GameObject Object;
 
    Vector3 tmpPos;
    public float F_MovementSpeed ;
    private void Start()
    {
        F_MovementSpeed = 3;


    }
    void LateUpdate()
    {
        if (RiverRafting_Main.Instance.B_MoveUp)
        {
            transform.Translate(Vector3.up * 3f * Time.deltaTime); 
            RiverRafting_Main.Instance.B_MoveUp = false;
        }
        if (RiverRafting_Main.Instance.B_MoveDown)
        {
            transform.Translate(Vector3.down * 3f * Time.deltaTime); 
            RiverRafting_Main.Instance.B_MoveDown = false;
        }
       if(RiverRafting_Main.Instance.B_MoveForward)
        {
            transform.Translate(Vector3.right * F_MovementSpeed * Time.deltaTime);
        }
       
       // G_Camera.transform.Translate(Vector3.right * 1f * Time.deltaTime);
        tmpPos = this.transform.position;
        //tmpPos.x = Mathf.Clamp(tmpPos.x, -3f, -3f);
        tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 1.75f);
        this.transform.position = tmpPos;


       // Debug.Log(this.GetComponent<Rigidbody2D>().velocity.magnitude);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Object = collision.gameObject;
       
        if (Object.name == "Rock")
        {
            F_MovementSpeed = 0;
         //   RiverRafting_Main.Instance.B_MoveForward = false;
            this.transform.GetComponent<Animator>().Play("HitRock");
            //AS_hit sound
        }
        if (Object.name == "Q_Stop")
        {
           
            Object.GetComponent<Collider2D>().enabled = false;
            this.transform.GetComponent<Animator>().Play("HitRock");
            RiverRafting_Main.Instance.B_MoveForward = false;
            RiverRafting_Main.Instance.THI_ShowQuestion();
        }
        if (Object.transform.parent.name == "Coins")
        {
            RiverRafting_Main.Instance.THI_CoinCollected();
            Destroy(Object);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Object = collision.gameObject;
        Debug.Log(Object.name);
        if (Object.name == "Rock")
        {
            this.transform.GetComponent<Animator>().Play("HitRock");
            //AS_hit sound
        }
        if (Object.name == "Q_Stop")
        {
            Object.GetComponent<Collider2D>().enabled = false;
            this.transform.GetComponent<Animator>().Play("HitRock");
            RiverRafting_Main.Instance.B_MoveForward = false;
            RiverRafting_Main.Instance.THI_ShowQuestion();
        }
    }*/

    private void OnCollisionExit2D(Collision2D collision)
    {

        Object = collision.gameObject;
        if (Object.name == "Rock")
        {
            F_MovementSpeed = 3;
            this.transform.GetComponent<Animator>().Play("Rowing");
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        Object = collision.gameObject;
        if (Object.name == "Rock")
        {
            this.transform.GetComponent<Animator>().Play("Rowing");
        }
    }*/

    public void OpenDam()
    {
        Object.GetComponent<Animator>().Play("DamOpen");
    }
}
   