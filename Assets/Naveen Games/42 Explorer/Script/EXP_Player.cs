using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP_Player : MonoBehaviour
{
    public static EXP_Player OBJ_exp_Player;
    GameObject G_Player;
    public bool B_MoveForward, B_MoveBackward, B_Jump, B_Run ;
    public bool B_CanMove;
    public float F_Speed, F_JumpSpeed;
    Rigidbody2D RB;
    public Joystick Joystick;
    public AudioSource AS_Jump;
    // Start is called before the first frame update
    void Start()
    {
        OBJ_exp_Player = this;
        B_CanMove = true;
        B_Jump = false;
        B_Run = true;
        G_Player = this.gameObject;
        RB = this.GetComponent<Rigidbody2D>();
        Joystick = FindObjectOfType<Joystick>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(B_CanMove)
        {
            if (Joystick.Horizontal > 0 || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                B_MoveForward = true;
                B_MoveBackward = false;
            }
            else if (Joystick.Horizontal < 0 || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                B_MoveForward = false;
                B_MoveBackward = true;
            }
            else
            {
                B_Run = false;
                if(B_Jump)
                {
                   // AS_Running.Stop();
                    G_Player.GetComponent<Animator>().Play("Idle");
                }
                
                B_MoveBackward = false;
                B_MoveForward = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                BUT_Jump();
            }


            if (B_MoveForward)
            {
                
                Running();
                B_Run = true;
                G_Player.transform.Translate(Vector2.right * F_Speed * Time.deltaTime);
                G_Player.transform.localScale = new Vector3(0.69f, 0.69f, 0.69f);
            }
            if (B_MoveBackward)
            {
               
                Running();
                B_Run = true;
                G_Player.transform.Translate(Vector2.left * F_Speed * Time.deltaTime);
                G_Player.transform.localScale = new Vector3(-0.69f, 0.69f, 0.69f);
            }
        }
       

       
    }

    void Running()
    {
        if (B_Run && B_Jump)
        {
           
            G_Player.GetComponent<Animator>().Play("Running");
        }
       
    }
   public void BUT_Jump()
    {
        if(B_Jump && B_CanMove)
        {
            AS_Jump.Play();
            B_Run = false;
            G_Player.GetComponent<Animator>().Play("Jump");
            RB.velocity = Vector2.up * F_JumpSpeed;
            G_Player.transform.Translate(Vector2.right * F_Speed * Time.deltaTime);
            B_Jump = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BG" || collision.gameObject.name == "Square" && !B_Jump)
        {
           // AS_Jump.Play();
            G_Player.GetComponent<Animator>().Play("Idle");
            // Debug.Log(collision.gameObject.name);
            B_Jump = true;
           // B_Run = true;
        }
       
        if(collision.gameObject.name=="Map")
        {
            B_Jump = true;
            G_Player.GetComponent<Animator>().Play("Idle");

            B_CanMove = false;
            B_MoveBackward = false;
            B_MoveForward = false;
            Joystick.transform.parent.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            // EXP_Main.Instance.THI_ShowQuestion();
             EXP_Main.Instance. THI_MAPFxOn();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.name == "Coins")
        {
            EXP_Main.Instance.THI_pointCoinFxOn();
            Destroy(collision.gameObject);
        }
    }

}
