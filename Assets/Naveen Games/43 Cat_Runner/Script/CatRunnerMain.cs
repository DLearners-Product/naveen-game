using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRunnerMain : MonoBehaviour
{
    GameObject G_Cat;
    public int I_Count=1;
    public float F_MovementSpeed;
    public bool B_CanChange,B_CanMove = true;
    public bool B_LevelCompleted;
    //public ParticleSystem[] P_sys;
    GameObject G_Obstacle;
   // public GameObject G_Particle;
    //public GameObject G_Question;
    public AudioSource AS_MouseChase;
    public AudioSource AS_Hit;

    // Start is called before the first frame update
    void Start()
    {
       B_LevelCompleted = false;
       G_Cat = this.gameObject;
       B_CanMove = true;
      //  G_Particle.SetActive(false);
     //  G_Question = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(B_CanChange)
        {
            if (Input.GetMouseButtonUp(0))
            {
                // if(G_Question == null)
                // {
              //  G_Particle.SetActive(false);
                I_Count++;
                    B_CanMove = true;
                    B_CanChange = false;
                    if (I_Count % 2 == 0)
                    {
                        G_Cat.GetComponent<Animator>().Play("CatUp");
                        G_Cat.GetComponent<Rigidbody2D>().gravityScale = -5;
                        // G_Cat.transform.eulerAngles = new Vector3(0, 180, 180);
                    }
                    else
                    {
                        G_Cat.transform.eulerAngles = new Vector3(0, 0, 0);
                        G_Cat.GetComponent<Animator>().Play("CatDown");
                        G_Cat.GetComponent<Rigidbody2D>().gravityScale = 5;
                    }
               // }
            }
        }
    }

    private void LateUpdate()
    {
        if(B_CanMove)
        {
           G_Cat.transform.Translate(Vector3.right * F_MovementSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.parent.name=="Coins")
        {
           // Debug.Log("Coins Collected");
            Destroy(collision.gameObject);
            Main_CatRunner.Instance.THI_pointCoinFxOn(true);
        }
        if(collision.gameObject.name=="Mouse_Anim")
        {
            AS_Hit.Stop();
            AS_MouseChase.Play();
            collision.gameObject.GetComponent<Animator>().enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name=="Top")
        {
           // G_Particle.SetActive(true);
            G_Cat.GetComponent<Animator>().Play("Cat_Running");
            G_Cat.transform.eulerAngles = new Vector3(0, 180, 180);
            if(!B_LevelCompleted)
            {
                B_CanChange = true;
                B_CanMove = true;
            }
            
        } 
        else 
        if(collision.gameObject.name == "Bottom")
        {
           // G_Particle.SetActive(true);
            G_Cat.GetComponent<Animator>().Play("Cat_Running");
            G_Cat.transform.eulerAngles = new Vector3(0, 0, 0);
            if (!B_LevelCompleted)
            {
                B_CanChange = true;
                B_CanMove = true;
            }
        }
        if(collision.gameObject.name=="Obstacle")
        {
            Main_CatRunner.Instance.THI_pointCoinFxOn(false);
            if(G_Obstacle==null)
            {
                AS_MouseChase.Stop();
                AS_Hit.Play();
                B_CanChange = false;
                G_Obstacle = collision.gameObject;
            }
            G_Cat.GetComponent<Animator>().Play("CatDizzy");

            StartCoroutine(Reducealpha());
            B_CanMove = false;
        }

        if(collision.gameObject.name=="Question")
        {
           // G_Question = collision.gameObject;
            Main_CatRunner.Instance.THI_ShowQuestion();
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            B_CanChange = false;
            B_CanMove = false;
            G_Cat.GetComponent<Animator>().enabled = false;
            
            Destroy(collision.gameObject);
        }
    }

    IEnumerator Reducealpha()
    {
        for(int i=0;i<2;i++)
        {
            yield return new WaitForSeconds(0.15f);
            G_Obstacle.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);

            yield return new WaitForSeconds(0.15f);
            G_Obstacle.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        yield return new WaitForSeconds(0.15f);
        if (G_Obstacle != null)
        {
            Destroy(G_Obstacle.gameObject);
        }
        B_CanChange = true;
        B_CanMove = true;
    }
}
