using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_movement : MonoBehaviour
{
    
    public Joystick FixedJoystick;
    float X, Y;
    public GameObject G_Player, G_Player2;
    public float F_Speed;
    public bool B_MoveForward, B_MoveBackward;
    
    public GameObject G_Box1, G_Box2;
    public bool B_Lerp1, B_Lerp2;
    public GameObject G_Button;
    Vector3 tmpPos, tmpPos1;
    
    // Start is called before the first frame update
    void Start()
    {
        FixedJoystick = FindObjectOfType<Joystick>();
        G_Player = this.transform.GetChild(0).gameObject;
        G_Player2 = this.transform.GetChild(1).gameObject;
        B_Lerp1 = true;
        B_Lerp2 = false;
        Invoke(nameof(Offlerp), 3f);
        this.gameObject.transform.GetChild(1).transform.position = G_Box1.transform.position;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        FixedJoystick.gameObject.SetActive(false);
        G_Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        X = FixedJoystick.Horizontal;
        // X = FixedJoystick.Horizontal;
        if (FixedJoystick.Horizontal > 0 || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            B_MoveForward = true;
            B_MoveBackward = false;
        }
        else if (FixedJoystick.Horizontal < 0 || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            B_MoveForward = false;
            B_MoveBackward = true;
        }
        else
        {
            B_MoveBackward = false;
            B_MoveForward = false;
        }



        if (B_MoveForward)
        {
            G_Player.transform.Translate(Vector2.right * F_Speed * Time.deltaTime);
            G_Player2.transform.Translate(Vector2.right * F_Speed * Time.deltaTime);
            THI_Poscheck();
            if (!G_Player.GetComponent<AudioSource>().isPlaying)
            G_Player.GetComponent<AudioSource>().Play();

        }else
        if (B_MoveBackward)
        {
            G_Player.transform.Translate(Vector2.left * F_Speed * Time.deltaTime);
            G_Player2.transform.Translate(Vector2.left * F_Speed * Time.deltaTime);


            THI_Poscheck();

            if (!G_Player.GetComponent<AudioSource>().isPlaying)
                G_Player.GetComponent<AudioSource>().Play();
        }else
        {
            G_Player.GetComponent<AudioSource>().Stop();
        }

        if(B_Lerp1)
        {
          //  Debug.Log("Calling Lepr1 Start");
            this.gameObject.transform.GetChild(1).transform.position = Vector3.Lerp(this.gameObject.transform.GetChild(1).transform.position, G_Box2.transform.position, 2f*Time.deltaTime);
           
          //  Debug.Log("Calling Lepr1 End");
        }

        if (B_Lerp2)
        {
          //  Debug.Log("Calling Lepr2 Start");

            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).transform.position = Vector3.Lerp(this.gameObject.transform.GetChild(0).transform.position, G_Box2.transform.position, 2f * Time.deltaTime);
            

           
          //  Debug.Log("Calling Lepr2 End");
        }

       
    }

    void THI_Poscheck()
    {
        tmpPos = G_Player.transform.position;
        tmpPos.x = Mathf.Clamp(tmpPos.x, -5f, 8f);
        // tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 2f);
        G_Player.transform.position = tmpPos;

        tmpPos1 = G_Player2.transform.position;
        tmpPos1.x = Mathf.Clamp(tmpPos.x, -5f, 8f);
        // tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 2f);
        G_Player2.transform.position = tmpPos;
    }

    void Offlerp()
    {
        B_Lerp1 = false;
        B_Lerp2 = true;
        Invoke(nameof(Offlerp2), 3f);
    }

    void Offlerp2()
    {
        B_Lerp2 = false;
        Debug.Log(B_Lerp2);
        B_Lerp1 = false;
        
        Offlerp3();
    }

    void Offlerp3()
    {
        Debug.Log("Before =" + this.gameObject.transform.GetChild(1).transform.position.y);
        this.gameObject.transform.GetChild(1).transform.position = new Vector3(this.gameObject.transform.GetChild(1).transform.position.x, this.gameObject.transform.GetChild(1).transform.position.y + 1f, this.gameObject.transform.GetChild(1).transform.position.z);
        this.gameObject.transform.GetChild(0).transform.position = new Vector3(this.gameObject.transform.GetChild(0).transform.position.x, this.gameObject.transform.GetChild(0).transform.position.y + 1f, this.gameObject.transform.GetChild(0).transform.position.z);
        Debug.Log("After =" + this.gameObject.transform.GetChild(1).transform.position.y);

        FixedJoystick.gameObject.SetActive(true);
        G_Button.SetActive(true);
    }
}
