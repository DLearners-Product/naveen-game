using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarzan_Player : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        DJ2D = GetComponent<DistanceJoint2D>();
        DJ2D.enabled = false;
        LR_Rope = this.transform.GetChild(0).GetComponent<LineRenderer>();
       
        pos = transform.position;
        V3_StartPos = transform.position;
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
       /* if (transition)
        {
            elapsed += Time.deltaTime / duration;
           // Debug.Log("Elapsed ==" + elapsed);
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, F_CamZoomout, elapsed);
        }
        if (B_transition)
        {
            elapsed += Time.deltaTime / duration;
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, F_CamZoomin, elapsed);
            if (elapsed > 1.0f)
            {
                B_transition = false;
                elapsed = 0;
            }
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            AS_Empty.clip = ACA_Clips[0];
            AS_Empty.Play();
            F_Timertoturn = 0;

            pos = transform.position;
           
            OnDrawGizmos();
            F_Array = new float[GA_Circles.Length];
            for (int i=0;i<GA_Circles.Length;i++)
            {
                float range = Vector3.Distance(this.transform.position, GA_Circles[i].transform.position);
                F_Array[i] = range;
            }

            minDistance = Mathf.Min(F_Array);

            for (int i = 0; i < F_Array.Length; i++)
            {
                if (minDistance == F_Array[i])
                {
                    Index = i;
                }
            }

             GA_Circles[Index].transform.GetChild(0).gameObject.SetActive(true);
             G_Catchthis = GA_Circles[Index];
             DJ2D.connectedAnchor = G_Catchthis.transform.position;


             LR_Rope.SetPosition(0, G_Catchthis.transform.position);
             LR_Rope.SetPosition(1, this.transform.GetChild(0).transform.position);

             LR_Rope.enabled = true;
             DJ2D.enabled = true;


        }
        if(Input.GetMouseButtonUp(0))
        {
            AS_Empty.clip = ACA_Clips[1];
            AS_Empty.Play();
            /* B_transition = true;
             transition = false;
             elapsed = 0;*/
            F_Timertoturn = 0;
            B_CanCount = true;

            for (int i = 0; i < GA_Circles.Length; i++)
            {
                GA_Circles[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            pos = this.transform.position;
            DJ2D.enabled = false;
            LR_Rope.enabled = false;
            G_Catchthis = null;
        }

        if (DJ2D.enabled)
        {
            LR_Rope.SetPosition(1, this.transform.GetChild(0).transform.position);
        }

        if(G_Catchthis!=null)
        {
            if(B_CanCount)
            F_Timertoturn = F_Timertoturn + 1f*Time.deltaTime;

            if (G_Catchthis.transform.position.x < transform.position.x)
            {
                this.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                if (F_Timertoturn > 5)
                {
                    B_CanCount = false;
                    this.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                    
            }
        }
        else
        {
            //if (B_CanCount)
            //    F_Timertoturn = F_Timertoturn + 1f * Time.deltaTime;

            if (pos.x < transform.position.x)
            {
               // if (F_Timertoturn > 5)
               // {
                  //  B_CanCount = false;
                    this.transform.localScale = new Vector3(1f, 1f, 1f);
               // }
                    
            }
            else
            {
               // if (F_Timertoturn > 5)
               // {
                      B_CanCount = false;
                    this.transform.localScale = new Vector3(-1f, 1f, 1f);
               // }
            }
        }
       
      

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
    }

    public void THI_StartPos()
    {
        this.transform.position = V3_StartPos;
    }
}
