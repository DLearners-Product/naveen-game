using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarzan_Player : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer LR_Rope;
    public Camera Cam;
    public GameObject[] GA_Circles;
    public GameObject G_Catchthis;
    public DistanceJoint2D DJ2D;
    public float Range;
    public Vector3 pos,V3_StartPos;

    public AudioSource AS_Empty, AS_Jump;
    public AudioClip[] ACA_Clips;
    float F_Timertoturn;
    bool B_CanCount;

    public float[] F_Array;
    public float minDistance;
    int Index;
    public Vector2 F_playerVelocity;
    [SerializeField]
    bool B_landedOnMushroom;
    Vector2 forceApplyDirection;

    [Header("Force Applied")]
    public float F_forceApplied;
    public float F_horizontalForceApplied;
    public float F_dragForce;
    float previousAppliedDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DJ2D = GetComponent<DistanceJoint2D>();
        DJ2D.enabled = false;
        LR_Rope = this.transform.GetChild(0).GetComponent<LineRenderer>();
        previousAppliedDirection = 0;

        pos = transform.position;
        V3_StartPos = transform.position;
        B_landedOnMushroom = false;
    }
    public void THI_GetCircles()
    {
        
        GA_Circles = GameObject.FindGameObjectsWithTag("Wine");
        for (int i = 0; i < GA_Circles.Length; i++)
        {
            GA_Circles[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        Cam.GetComponent<Frog_Follow>().B_canfollow = true;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerAnimation();
    }

    void PlayerMovement(){
        F_playerVelocity = rb.velocity;

        if(rb.velocity.magnitude < 0)
            rb.velocity = rb.velocity * F_dragForce;

        if(B_landedOnMushroom || Input.GetButton("Horizontal")){
        // if(B_landedOnMushroom){
            // if(B_landedOnMushroom && (previousAppliedDirection != Input.GetAxis("Horizontal")))
            if(B_landedOnMushroom)
                rb.velocity = Vector2.zero;
            
            if(Input.GetButton("Horizontal")){
                forceApplyDirection = new Vector2(
                    Input.GetAxis("Horizontal") * F_horizontalForceApplied,
                    ((B_landedOnMushroom) ? (1  * F_forceApplied) : rb.velocity.y)
                );
            }else{
                forceApplyDirection = Vector2.up * F_forceApplied;
            }

            B_landedOnMushroom = false;

            // rb.AddForce(forceApplyDirection, ForceMode2D.Impulse);
            rb.velocity = forceApplyDirection;
        }
    }
    
    void PlayerAnimation(){
        Vector2 playerFacingDirection = rb.velocity.normalized;

        Debug.Log("Player Facing Direction : "+playerFacingDirection);

        transform.forward = new Vector2(
            playerFacingDirection.x,
            0f
        );
    }

    private void OnDrawGizmos()
    {
        if (G_Catchthis == null)
          return;

        Gizmos.DrawWireSphere(this.transform.GetChild(0).transform.position, Range);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision entered");
        if(collision.gameObject.name=="Ground")
        {
            rb.velocity = Vector2.zero;

            AS_Empty.clip = ACA_Clips[2];
            AS_Empty.Play();
            Invoke(nameof(THI_StartPos),1f);
        }

        if (collision.gameObject.name == "End")
        {
            Frog_Follow.OBJ_followingCamera.B_canfollow = false;
            Tarzan_Main.Instance.AS_LevelOver.Play();
            Tarzan_Main.Instance.G_Question.SetActive(true);
            this.gameObject.SetActive(false);
        }

        if(collision.gameObject.GetComponent<Mushroom>()){
            B_landedOnMushroom = true;
        }
    }

    public void THI_StartPos()
    {
        rb.velocity = Vector2.zero;
        this.transform.position = V3_StartPos;
    }
}
