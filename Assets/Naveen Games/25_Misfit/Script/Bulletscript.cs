using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulletscript : MonoBehaviour
{
    public float movementsped;
    public GameObject blastanim;
    public AnimationClip AC_blast;

    public GameObject spark;
    string Hit_name;
    Transform T_Pos;
    GameObject G_This;

    // Start is called before the first frame update
   
    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(Vector2.right * movementsped);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.transform.parent.name == "Options")    //play burst anim
        {
            Hit_name = collision.gameObject.transform.GetChild(0).name;
            T_Pos = collision.collider.transform;
            G_This = collision.gameObject;


            Misfit_Main.Instance.BUT_Clicking(Hit_name);

            if (Hit_name == Misfit_Main.Instance.STR_currentQuestionAnswer)
            {
                GameObject blast = Instantiate(blastanim);
                blast.transform.position = T_Pos.position;

                this.gameObject.SetActive(false);
                G_This.SetActive(false);
                Destroy(blast, 1.2f);
                GDestroy();
            }
            else
            {
                GDestroy();
            }
        }
        else if (collision.collider.transform.parent.name == "Enemy(Clone)")                                
        {
           Misfit_Main.Instance.THI_Count();
           GDestroy();
        }
    }

    private void GDestroy()
    {
        Destroy(this.gameObject);
    }

    void THI_HitEffect()
    {
       
    }
}
