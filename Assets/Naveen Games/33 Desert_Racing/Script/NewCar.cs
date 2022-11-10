using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCar : MonoBehaviour
{
    public float moveSpeed;
    public float rotationAngle;

    public TrailRenderer[] Trails;
    Vector3 direction;
    float angle,Current_Angle;
    Rigidbody2D rb;
    public float Value_X, Value_Y;
    Vector3 tmpPos;
    public Joystick JS_Control;
    public bool B_CanMove;
    public bool B_CallOnce1, B_CallOnce2;
    public AudioSource AS_Moving, AS_Drift;
    // Start is called before the first frame update
    void Start()
    {
        B_CanMove = true;
        rb = GetComponent<Rigidbody2D>();
        B_CallOnce2 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (B_CanMove)
        {
           
               //Changing the X and Y values for math the requirements
            Value_Y = JS_Control.Vertical;
            Value_X = JS_Control.Horizontal;
          
            if (JS_Control.Vertical == 0 && JS_Control.Horizontal == 0)
            {
                Value_Y = Input.GetAxis("Vertical");
                Value_X = Input.GetAxis("Horizontal");
            }

            direction = new Vector3(Value_X, Value_Y, 0f);
            angle = Mathf.Atan2(Value_Y, Value_X) * Mathf.Rad2Deg;

            movement();

           
        }
        

        tmpPos = this.transform.position;
        tmpPos.x = Mathf.Clamp(tmpPos.x, -8f, 8f);
        tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 3f);
        this.transform.position = tmpPos;
    }

    private void FixedUpdate()
    {
       // if (B_CanMove)
       // {
            rb.velocity = direction * moveSpeed * Time.fixedDeltaTime;
            for (int i = 0; i < Trails.Length; i++)
            {
                Trails[i].emitting = true;
            }
            AS_Moving.Play();
       // }

    }

    void movement()
    {
        if (Value_Y != 0 || Value_X != 0)
        {
            transform.rotation = Quaternion.AngleAxis(angle - rotationAngle, new Vector3(0, 0, 1));
            Current_Angle = transform.rotation.z;
        }
        /*else
        {
            transform.rotation.z = new Vector3.eulerangel
        }*/
        
        rb.position = transform.position;
        
        /*for (int i = 0; i < Trails.Length; i++)
        {
            Trails[i].emitting = false;
        }*/
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.name == "Options")
        {
            if (B_CallOnce2)
            {
                B_CallOnce2 = false;
                 Debug.Log(collision.name + "1"+ collision.name);
                DesertRacing_Main.Instance.THI_OptionStay(collision.gameObject);
            }
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.name == "Options")
        {
            B_CallOnce2 = true;
            DesertRacing_Main.Instance.B_CheckOnce = true;
        }

        if (collision.gameObject.name == "Dune")
        {
            this.transform.localScale = new Vector2(0.25f, 0.25f);
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
