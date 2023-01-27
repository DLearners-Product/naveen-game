using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarzan_Player : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer LR_Rope;
    ParticleSystem PS_spawnedParticleSystem;
    public Camera Cam;
    public GameObject[] GA_Circles;
    public GameObject G_Catchthis;
    public DistanceJoint2D DJ2D;
    public float Range;
    public Vector3 pos,V3_StartPos;
    public ParticleSystem PS_questionSpawnParticle;

    public AudioSource AS_Empty, AS_Jump;
    public AudioClip[] ACA_Clips;
    float F_Timertoturn;
    bool B_CanCount;

    public float[] F_Array;
    public float minDistance;
    int Index;
    public Vector2 playerVelocity;
    public float F_playerMagnitude;

    [SerializeField]
    bool B_landedOnMushroom;
    bool B_appliedDragChild;
    public bool B_blockInput;
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

        B_landedOnMushroom = true;
        B_appliedDragChild = false;
        B_blockInput = false;
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
        playerVelocity = rb.velocity;
        F_playerMagnitude = rb.velocity.magnitude;

        // if((rb.velocity.y < 0 || !B_landedOnMushroom) && B_appliedDragChild){
        //     B_appliedDragChild = false;
        //     rb.velocity = rb.velocity * F_dragForce;
        // }

        if(B_landedOnMushroom || (Input.GetButton("Horizontal") && !B_blockInput)){
        // if(B_landedOnMushroom){
            // if(B_landedOnMushroom && (previousAppliedDirection != Input.GetAxis("Horizontal")))
            if(B_landedOnMushroom)
                rb.velocity = Vector2.zero;
            
            if(Input.GetButton("Horizontal") && !B_blockInput){
                forceApplyDirection = new Vector2(
                    Input.GetAxis("Horizontal") * F_horizontalForceApplied,
                    ((B_landedOnMushroom) ? F_forceApplied : rb.velocity.y)
                );
            }else{
                forceApplyDirection = Vector2.up * F_forceApplied;
            }
            // forceApplyDirection = new Vector2();

            B_landedOnMushroom = false;

            // rb.AddForce(forceApplyDirection, ForceMode2D.Impulse);
            rb.velocity = forceApplyDirection;
        }
    }
    
    void PlayerAnimation(){
        if(!B_blockInput && !Input.GetButton("Horizontal")) { return; }
        // Debug.Log("Axis : "+Input.GetAxis("Horizontal"));
        transform.localScale = new Vector3(
            ((Input.GetAxis("Horizontal") > 0) ? 1 : -1) * Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
        // transform.forward = new Vector2(
        //     playerFacingDirection.x,
        //     0f
        // );
    }

    private void OnDrawGizmos()
    {
        if (G_Catchthis == null)
          return;

        Gizmos.DrawWireSphere(this.transform.GetChild(0).transform.position, Range);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            Tarzan_Main.Instance.THI_Levelcompleted();
            this.gameObject.SetActive(false);
        }

        if(collision.gameObject.GetComponent<Mushroom>()){
            B_landedOnMushroom = true;
            B_appliedDragChild = true;
            V3_StartPos = (collision.gameObject.GetComponent<Mushroom>().spawnPosition) ? collision.gameObject.GetComponent<Mushroom>().spawnPosition.position : V3_StartPos;
            if(collision.gameObject.GetComponent<Mushroom>().B_questionALlocated){
                PS_spawnedParticleSystem = Instantiate(PS_questionSpawnParticle, gameObject.transform.position, Quaternion.identity);

                Tarzan_Main.Instance.Invoke("THI_SpawnQuestion", 0.5f);

                // Debug.Log("Game Object Name : "+collision.gameObject.transform.parent.gameObject.name+" Game Object Instance ID : "+collision.gameObject.transform.parent.GetInstanceID(), collision.gameObject.transform.parent.gameObject);

                Tarzan_Main.Instance.THI_DeAllocateQuestion(collision.gameObject.transform.parent.gameObject);
                B_blockInput = true;
            }
        }
    }

    public void THI_StartPos()
    {
        rb.velocity = Vector2.zero;
        this.transform.position = V3_StartPos;
    }
}
