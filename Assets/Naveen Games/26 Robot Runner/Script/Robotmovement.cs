using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robotmovement : MonoBehaviour
{
    public static Robotmovement OBJ_robotmovement;
    [SerializeField] private float movementsped;
    [SerializeField]
    private float offset;

    private Vector2 startpostion;
    private float newXpostion;
    public Rigidbody2D RB2D_robot;
    public float jumpspeed;

    //public Sprite SPR_jump, SPR_land;
    public bool B_canjump;
    public bool B_reducelife;

    public AnimationClip AC_blast;
    public GameObject blast;
    public GameObject Local_blastanim;
    public GameObject Robot;
    public GameObject G_10, G_5;
    //public GameObject G_portal;
    public ParticleSystem smokeeffect;
    public ParticleSystem stareffect;
    GameObject G_portal;
   // public bool B_portalopen;

    public AudioSource AS_falling;
    public AudioSource AS_Walking;
    public AudioSource AS_Jumping;
    public AudioSource AS_Portal;

    public GameObject play;
    public AnimationClip AC_portaldisapears;

    public void Awake()
    {
        OBJ_robotmovement = this;
        Robot = this.gameObject;
        RB2D_robot = this.GetComponent<Rigidbody2D>();
       // FollowingCamera.OBJ_followingCamera.B_canfollow = false;
        startpostion = transform.position;
      //  B_portalopen = false;
       // startfunction();
    }
    public void startfunction()
    {
        this.GetComponent<Animator>().Play("land");
        
        movementsped = 0f;
        RB2D_robot.gravityScale = 0;
        transform.position = startpostion;
        B_reducelife = true;
        play.SetActive(true);
        offscoreeffect();
    }
    public void offscoreeffect()
    {
        G_10.SetActive(false);
        G_5.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        // OBJ_robotmovement = this;

        this.GetComponent<Animator>().Play("land");
       
    }
    public void BUT_Play()
    {
        RB2D_robot.gravityScale = 1.5f;
        play.SetActive(false);
        FollowingCamera.OBJ_followingCamera.B_canfollow = true;
        AS_falling.Play();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * movementsped);
    }
    public void jump()
    {
        if (B_canjump)
        {
            this.GetComponent<Animator>().Play("jump");
            AS_Walking.Stop(); 
            AS_Jumping.Play(); 
            // this.GetComponent<SpriteRenderer>().sprite = SPR_jump;
            RB2D_robot.velocity = Vector2.up * jumpspeed;
            // B_canjump = false;
        }

    }
    public void down()
    {
        if (!B_canjump)
        {
            this.GetComponent<Animator>().Play("land");
           
            RB2D_robot.velocity = Vector2.down * jumpspeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "star")
        {
            starplayparticle();
            Destroy(collision.gameObject);
            RB_Runner_Main.Instance.THI_Collect_Out(true);
           // Main_runner.OBJ_main_Runner.AddScore();
        }
/*        if (collision.gameObject.tag == "crtans")
        {
            Destroy(collision.gameObject);
            B_portalopen = true;
            G_10.SetActive(true);
           // Main_runner.OBJ_main_Runner.crtAns();
        }
        if (collision.gameObject.tag == "wrgans")
        {
            Destroy(collision.gameObject);
            G_5.SetActive(true);
           // wrgansout();                                  //destroyeffect
           // Main_runner.OBJ_main_Runner.wrgAns();
        }*/
        if (collision.gameObject.name == "portal")
        {
            G_portal = collision.gameObject;
            G_portal.GetComponent<Animator>().SetInteger("cond", 1);
                this.gameObject.SetActive(false);
                Invoke("nextquest", AC_portaldisapears.length);
            AS_Portal.Play();
            // Main_runner.OBJ_main_Runner.portalcloseanim();
            // this.transform.position = startpostion;
        }
    }
    public void backtostart()
    {
        Destroy(Local_blastanim);
        this.gameObject.SetActive(true);
      
    }

    public void nextquest()
    {
        Destroy(G_portal);
        RB_Runner_Main.Instance.THI_ShowQuestion();
       // B_portalopen = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor" || collision.gameObject.name == "floating")
        {
            movementsped = 0.1f;
            playparticle();
            this.GetComponent<Animator>().Play("walk");
            if (!AS_Walking.isPlaying)
            { AS_Walking.Play(); }
            B_canjump = true;
        }
        if (collision.gameObject.name == "out")
        {
            if (B_reducelife)
            {
                AS_Walking.Stop();
                RB_Runner_Main.Instance.THI_Collect_Out(false);
                Robot_Out();
            }
        }
    }
   /* public void wrgansout()
    {
        movementsped = 0;
        this.GetComponent<Animator>().Play("land");
        this.gameObject.SetActive(false);
        Local_blastanim = Instantiate(blast);
        Local_blastanim.transform.position = this.transform.position;
       // Main_runner.OBJ_main_Runner.reducelife();
        B_reducelife = false;
        Invoke("spawnthere", AC_blast.length);
    }
    public void spawnthere()
    {
        Destroy(Local_blastanim);
        this.gameObject.SetActive(true);
        B_reducelife = true;
    }*/
   public void Robot_Out()
    {
        movementsped = 0;
        this.gameObject.SetActive(false);

        Local_blastanim = Instantiate(blast);
        Local_blastanim.transform.position = this.transform.position;

       // Main_runner.OBJ_main_Runner.reducelife();
        B_reducelife = false;
        Invoke("THI_Outrespawn", AC_blast.length);
    }
    public void THI_Outrespawn()
    {
        Destroy(Local_blastanim);

        Vector2 pos = this.transform.position;
        if (pos != startpostion)
        {
            pos = new Vector2(pos.x - 10, pos.y + 20);
            this.transform.position = pos;
        }

        this.gameObject.SetActive(true);
        this.GetComponent<Animator>().Play("land");

        RB2D_robot.gravityScale = 0;
        B_reducelife = true;
        play.SetActive(true);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor" || collision.gameObject.name == "floating")
        {
           // movementsped = 0;
            stopparticle();
            this.GetComponent<Animator>().Play("jump");
            B_canjump = false;
        }
    }
    public void THI_respawn()
    {
        Destroy(Local_blastanim);
        
        Vector2 pos = this.transform.position;
        if(pos!=startpostion)
        {
            pos = new Vector2(pos.x - 10, pos.y +10);
            this.transform.position = pos;
        }
        
        this.gameObject.SetActive(true);
        this.GetComponent<Animator>().Play("land");

        RB2D_robot.gravityScale = 0;
        B_reducelife = true;
        play.SetActive(true);

    }
    public void playparticle()
    {
        smokeeffect.Play();
    }
    public void stopparticle()
    {
        smokeeffect.Stop();
    }
    public void starplayparticle()
    {
        stareffect.Play();
    }
}
