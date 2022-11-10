using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_DR : MonoBehaviour
{
    public TrailRenderer[] Trails;
    public float move, movespeed, rotation, rotationspeed;
    public float Value_X, Value_Y;
    Vector3 tmpPos;
    public Joystick JS_Control;
   // public  bool B_Keyboard, B_Joystick;
    public AudioSource AS_Moving, AS_Drift;
    public bool B_CallOnce1, B_CallOnce2;
    public bool B_CanMove;

    void Start()
    {
        B_CanMove = true;
        movespeed = 5f;
        rotationspeed = 200f;
        B_CallOnce1 = B_CallOnce2= true;
    }

    // Update is called once per frame
    void Update()
    {
        if (B_CanMove)
        {
            
       
       // THI_FindInput();
       // if (B_Joystick)
       // {
            Value_X = JS_Control.Vertical;
            Value_Y = JS_Control.Horizontal;
       // Debug.Log(JS_Control.Vertical);
       // Debug.Log(JS_Control.Horizontal);
       // }

        if (JS_Control.Vertical==0 && JS_Control.Horizontal==0)
        {
            Value_X = Input.GetAxis("Vertical");
            Value_Y = Input.GetAxis("Horizontal");
        }



        move = Value_X * movespeed * Time.deltaTime;
        rotation = Value_Y * -rotationspeed * Time.deltaTime;
        if (rotation != 0)
        {
            if (B_CallOnce1)
            {
                B_CallOnce1 = false;
                if (!AS_Drift.isPlaying && move != 0)
                {
                    AS_Drift.Play();
                }
            }

            for (int i = 0; i < Trails.Length; i++)
            {
                Trails[i].emitting = true;
            }
        }
        else
        {
            for (int i = 0; i < Trails.Length; i++)
            {
                Trails[i].emitting = false;
            }
        }

        tmpPos = this.transform.position;
        tmpPos.x = Mathf.Clamp(tmpPos.x, -8f, 8f);
        tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 3f);
        this.transform.position = tmpPos;

            /* float zAxis = Mathf.Atan2(Value_X, Value_Y) * Mathf.Rad2Deg;
             if(Value_X>0||Value_Y<0)
             {
                 transform.eulerAngles = new Vector3(0, 0, -zAxis);
             }

             transform.position = new Vector2(
                Mathf.Clamp(transform.position.x, -Value_X, Value_Y),
                Mathf.Clamp(transform.position.y, -Value_X, Value_Y)
                );*/
        }

    }


    private void LateUpdate()
    {
        if (B_CanMove)
        {
            transform.Translate(0, move, 0);
            transform.Rotate(0, 0, rotation);
            if (move != 0)
            {
                if (!AS_Moving.isPlaying)
                    AS_Moving.Play();
            }
            else
            {
                AS_Moving.Stop();
            }

            if (!AS_Drift.isPlaying)
                B_CallOnce1 = true;
        }else
        {
            AS_Moving.Stop();
            AS_Drift.Stop();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.parent.gameObject.name=="Options")
        { 
            if(B_CallOnce2)
            {
                B_CallOnce2 = false;
               // Debug.Log(collision.name);
                DesertRacing_Main.Instance.THI_OptionStay(collision.gameObject);
            }
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.name == "Options")
        {
            B_CallOnce2 = true;
        }

        if (collision.gameObject.name == "Dune")
        {
            this.transform.localScale = new Vector2(0.25f, 0.25f);
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
