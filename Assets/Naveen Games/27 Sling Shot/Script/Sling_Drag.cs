using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling_Drag : MonoBehaviour
{
    bool isPressed;
    Rigidbody2D RB;
    SpringJoint2D SJ;
    private float ReleaseDelay;
    float Max_distance = 3f;
    Rigidbody2D SlingRB;
    LineRenderer LR;
    Vector2 direction, mousepos;
    TrailRenderer TR;
    public AudioSource AS_Sling;
    public AudioSource AS_Throw;
    public LineRenderer LR_Projection;
    public LineRenderer Rubber_2;

    [SerializeField]
    Camera cam;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SJ = GetComponent<SpringJoint2D>();
        LR = GetComponent<LineRenderer>();
        TR = GetComponent<TrailRenderer>();
        SlingRB = SJ.connectedBody;
        ReleaseDelay = 1 / (SJ.frequency * 4);    
       // LR.enabled = false;
        TR.enabled = false;

        LR.SetPosition(0, RB.position);
        LR.SetPosition(1, SlingRB.gameObject.transform.position);

        Rubber_2.SetPosition(0, Rubber_2.transform.position);
        Rubber_2.SetPosition(1, RB.gameObject.transform.GetChild(0).transform.position);

        LR.enabled = true;
        Rubber_2.enabled = true;
        this.transform.GetChild(0).gameObject.SetActive(true);

        cam = GameObject.FindWithTag("NewCam").GetComponent<Camera>();
    }

    /*  public void THI_BacktoStart()
      {
          RB.velocity = Vector2.zero;

         // RB.isKinematic = true;
         // RB.bodyType = RigidbodyType2D.Static;


          RB.bodyType = RigidbodyType2D.Dynamic; 
          RB.MovePosition(V3_InitialPos);
          B_CanDrag = true;
          SJ.enabled = true;

         // this.transform.position = V3_InitialPos;

      }*/

    private void Update()
    {
        if (isPressed)
        {
            DragBall();
        }
        // Debug.Log(RB.velocity.magnitude);
    }
    void DragBall()
    {
        SetLR();
        
        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);

        float distance = Vector2.Distance(mousepos, SlingRB.position);
        if (distance > Max_distance)
        {
            direction = (mousepos - SlingRB.position).normalized;
            RB.position = SlingRB.position + direction * Max_distance;
            // Debug.Log("Drag");
        }
        else
        {
            RB.position = mousepos;
        }
    }
    void SetLR()
    {
        Vector3[] positions = new Vector3[2];

        LR.SetPosition(0, RB.position);
        LR.SetPosition(1, SlingRB.gameObject.transform.position);

        Rubber_2.SetPosition(0, Rubber_2.transform.position);
        Rubber_2.SetPosition(1, RB.gameObject.transform.GetChild(0).transform.position);

        float PullDistacne = Vector3.Distance(SlingRB.transform.position, RB.transform.position);
        ShowTrajactory(PullDistacne);

        //LR.SetPositions[]
    }

    void ShowTrajactory(float distance)
    {
        LR_Projection.enabled = true;

        Vector3 diff = SlingRB.transform.position - RB.transform.position;
        int segmentcount = 25;
        Vector2[] segments = new Vector2[segmentcount];
        segments[0] = RB.transform.position;

        Vector2 setVelocity = new Vector2(diff.x, diff.y) * distance * 1.75f;       //1.5f added for correction
        for (int i=0;i< segmentcount;i++)
        {
            float timeCurve = (i * Time.fixedDeltaTime * 2); //5
            segments[i] = segments[0] + setVelocity*timeCurve + 0.5f * Physics2D.gravity * Mathf.Pow(timeCurve, 2);  //0.5f
        }

        LR_Projection.positionCount = segmentcount;
        for(int j=0;j<segmentcount;j++)
        {
            LR_Projection.SetPosition(j, segments[j]);
        }
    }


    private void OnMouseDown()
    {
        AS_Sling.Play();
        isPressed = true;
        RB.isKinematic = true;
       
    }
    private void OnMouseUp()
    {
        LR_Projection.enabled = false;
        TR.enabled = true;
        isPressed = false;
        RB.isKinematic = false;
        RB.gravityScale = 1f;
        StartCoroutine(Release());
        LR.enabled = false;
        Rubber_2.enabled = false;
        this.transform.GetChild(1).gameObject.SetActive(false);
        AS_Throw.Play();
        //Invoke(nameof(THI_BacktoStart), 1f);
    }


    IEnumerator Release()
    {
        yield return new WaitForSeconds(ReleaseDelay);
        SJ.enabled = false;
        // TR.enabled = true;
       
        Invoke("cloneagain", 3f);
            
    }

    void cloneagain() {
        SlingShot_Main.Instance.THI_CloneSling();
     }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject!=null)
        {
            if(collision.gameObject.transform.parent.transform.parent.name=="Question")
            {
                SlingShot_Main.Instance.THI_Check(collision.gameObject);
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
