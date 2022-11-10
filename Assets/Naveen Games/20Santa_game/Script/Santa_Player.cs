using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa_Player : MonoBehaviour
{
    public static Santa_Player Instance;
    public bool B_Right, B_Left;
    // public float movementspeed;
    Vector3 tmpPos;

    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            B_Right = true;
        }
        else
        if (Input.GetAxis("Horizontal") < 0)
        {
            B_Left = true;
        }

        if (B_Right)
        {
            transform.Translate(Vector3.right * 10f * Time.deltaTime);
            B_Right = false;
        }
        if (B_Left)
        {
            transform.Translate(Vector3.left * 10f * Time.deltaTime);
            B_Left = false;
        }
        tmpPos = this.transform.position;
        tmpPos.x = Mathf.Clamp(tmpPos.x, -6f, 6f);
        this.transform.position = tmpPos; 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Santa_coins(Clone)")
        {
            Santa_main.Instance.THI_Addpoints();
           Destroy( collision.gameObject);
        }
       else
       {
            Santa_main.Instance.STR_currentSelectedAnswer = collision.gameObject.name;
            Santa_main.Instance.THI_OptionCollect();

            Destroy(collision.gameObject);
        }
    }
}
