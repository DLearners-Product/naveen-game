using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HC_Controller : MonoBehaviour
{
    public static HC_Controller Instance;
    public float F_speed,F_Torque;
    public bool B_left, B_right, B_CanMove;
    public Vector3 Initial_pos, Dead_pos;
    public GameObject G_Restart;
    public GameObject F_wheel,B_wheel;
    public GameObject G_fill;
    public float F_Maxfuel, F_CurFuel, F_Distance;
    public bool B_CallOnce;
    public TextMeshProUGUI TEX_Distance;
    public AudioSource AS_collect;
    public AudioSource AS_CarSound;
    public AudioSource AS_Carcrash;

    private void Start()
    {
        Instance = this;
        //F_Distance = 0;
        TEX_Distance.text = "0 " + "m";
        F_Maxfuel = 100;
        THI_ReFill();
        F_Torque = 100;
        G_Restart.SetActive(false);
        B_CanMove = true;
        Initial_pos = this.transform.position;
    }
    public void THI_ReFill()
    {
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        B_CallOnce = true;
        F_CurFuel = 100;
        G_fill.GetComponent<Image>().fillAmount = F_CurFuel / F_Maxfuel;
    }
    void Update()
    {
        if (B_left)
        {
            if(F_CurFuel>0)
            {
                if (B_CanMove)
                {
                  //  Debug.Log("Move Backward");
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * F_speed * Time.deltaTime);
                    //this.gameObject.GetComponent<Rigidbody2D>().velocity=(Vector2.left * F_speed * Time.deltaTime);
                    this.gameObject.GetComponent<Rigidbody2D>().AddTorque(F_Torque * Time.deltaTime);
                    F_CurFuel = F_CurFuel - 0.1f;
                    G_fill.GetComponent<Image>().fillAmount = F_CurFuel / F_Maxfuel;
                   // F_Distance -= 0.5f;
                   // TEX_Distance.text = F_Distance.ToString();
                }
            }
        }
        if (B_right)
        {
           // Debug.Log("Fuel eruku right poo");
            if (F_CurFuel > 0)
            {
               // Debug.Log("Fuel eruku");
                if (B_CanMove)
                {
                  //  Debug.Log("Move forward");
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * F_speed * Time.deltaTime);
                   // this.gameObject.GetComponent<Rigidbody2D>().velocity=(Vector2.right * F_speed * Time.deltaTime);
                    this.gameObject.GetComponent<Rigidbody2D>().AddTorque(-F_Torque * Time.deltaTime);
                    F_CurFuel = F_CurFuel - 0.1f;
                    G_fill.GetComponent<Image>().fillAmount = F_CurFuel / F_Maxfuel;
                   // F_Distance += 0.5f;
                   // TEX_Distance.text = F_Distance.ToString();
                }
            }
        }
        if(F_CurFuel<0)
        {
            if (HCR_Main.Instance.I_currentQuestionCount < HCR_Main.Instance.STRL_questions.Count)
            {
                G_Restart.SetActive(true);
            }
        }
        else 
        if(F_CurFuel<30)
        {
            if(B_CallOnce)
            {
                AS_CarSound.Stop();
                
                this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                HCR_Main.Instance.THI_ShowQuestion();
                B_CallOnce = false;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
               B_left = false;
               B_right = true;
        }else

        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            B_right = false;
            B_left = true;
        }
        if(Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)|| Input.GetKeyUp(KeyCode.LeftArrow))
        {
            B_left = false;
            B_right = false;
        }
       
    }
  

    public void THI_restartInsameplace()
    {
        HCR_Main.Instance.THI_pointReduce();
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        HCR_Main.Instance.THI_ShowQuestion();
        B_CallOnce = false;
         G_Restart.SetActive(false);
         Dead_pos = this.transform.position;
         
        Invoke(nameof(AddSpeed), 0.5f);
    }

    public void THI_restartInStart()
    {
        HCR_Main.Instance.THI_pointReduce();
        Vector3 angle = this.gameObject.transform.eulerAngles;
        angle.z = 0;
        this.gameObject.transform.eulerAngles = angle;
        G_Restart.SetActive(false);
        this.transform.position = Initial_pos;

        this.gameObject.transform.eulerAngles = angle;
        B_wheel.transform.eulerAngles = angle;
        F_wheel.transform.eulerAngles = angle;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        F_speed = 1000;

        // Invoke(nameof(AddSpeed), 2f);
        TEX_Distance.text="0 m";
        // this.transform.rotation.z = 0f;
    }
    void AddSpeed()
    {
        Vector3 temp_pos = Dead_pos;
        temp_pos.y = temp_pos.y + 5f;
        this.transform.position = temp_pos;
        Vector3 angle = this.gameObject.transform.eulerAngles;
        angle.z = 0;
        this.gameObject.transform.eulerAngles = angle;
        B_wheel.transform.eulerAngles = angle;
        F_wheel.transform.eulerAngles = angle;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        F_speed = 1000;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            
            //  Debug.Log("polam poo");
            B_CanMove = true;
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            B_CanMove = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.name == "Coins")
        {
            AS_collect.Play();
            collision.gameObject.SetActive(false);
            HCR_Main.Instance.THI_Collectcoins();
        }
        if (collision.gameObject.transform.parent.name == "Distance")
        {
            TEX_Distance.text = collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text;
        }

        if (collision.gameObject.tag == "Ground")
        {
            if(!AS_CarSound.isPlaying)
            {
                AS_CarSound.Play();
            }
        }
            
    }

    public void THI_PlayerDead()
    {
        AS_Carcrash.Play();
        F_speed = 0;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        B_left = B_right = B_CanMove = false;
        G_Restart.SetActive(true);
    }
}
