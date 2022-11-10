using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbor_Container : MonoBehaviour
{
    Rigidbody2D RB2D;
    Vector3 tmpPos;
    // Start is called before the first frame update
    void Start()
    {
        RB2D = this.GetComponent<Rigidbody2D>();
        RB2D.bodyType = RigidbodyType2D.Static;
    }
    private void Update()
    {
        tmpPos = this.transform.position;
        tmpPos.x = Mathf.Clamp(tmpPos.x, -8f, 8f);
        tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 3f);
        this.transform.position = tmpPos;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RB2D.bodyType = RigidbodyType2D.Dynamic;
        RB2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        RB2D.bodyType = RigidbodyType2D.Static;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.transform.parent.name=="Bg")
        {
            Debug.Log("Hitting = " + collision.gameObject.name);
            Harbour_Main.Instance.STR_currentSelectedAnswer = collision.gameObject.name;
            Harbour_Main.Instance.B_CanClick = true;
        }
       

    }
   /* private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.name == "Bg")
        {
            Harbour_Main.Instance.STR_currentSelectedAnswer = "";
            Harbour_Main.Instance.B_CanClick = false;
        }
           
    }*/
}
