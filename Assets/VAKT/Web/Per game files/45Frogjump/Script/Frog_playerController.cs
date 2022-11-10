using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog_playerController : MonoBehaviour
{
    GameObject G_Player;
    public float F_Speed;
    bool B_CanMove;
    public bool B_SideWays, B_TopDown, Movethere;
    public float F_JumpSpeed;
    Rigidbody2D RB;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
       // F_Speed = 5f;
        G_Player = this.gameObject;
        RB = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(B_TopDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.gameObject.GetComponent<Animator>().Play("frogjumping");
                B_CanMove = true;
                Invoke(nameof(THI_Jump), .02f);
            }
        }
       
        if(B_SideWays)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.gameObject.GetComponent<Animator>().Play("Frog_jumpingnew");
                B_CanMove = true;
                RB.velocity = Vector2.up * F_JumpSpeed;
                float temp = this.transform.position.x;
                temp += 5f;
                pos = new Vector3(temp, this.transform.position.y);
                Movethere = true;
                // G_Player.transform.Translate(Vector2.right * F_Speed * Time.deltaTime);
                Invoke(nameof(Late), 0.5f);
            }
        }
    }
    private void LateUpdate()
    {
        if(Movethere)
        {
            transform.position = Vector3.Lerp(this.transform.position, pos, 2f * Time.deltaTime);
        }
        
    }
    void Late()
    {
        Movethere = false;
    }

    void THI_Jump()
    {
        if (B_CanMove)
        {
            B_CanMove = false;
            this.transform.Translate(Vector3.up * F_Speed * Time.deltaTime);
        }
    }
}
