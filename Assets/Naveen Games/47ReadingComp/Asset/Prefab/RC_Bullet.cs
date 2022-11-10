using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_Bullet : MonoBehaviour
{
    [SerializeField]
     float F_Speed=5f;
  

    // Update is called once per frame
    void Update()
    {
        if(this.name=="Hero_bullet(Clone)")
        {
            this.transform.Translate(Vector2.right* F_Speed*Time.deltaTime);
        }else
        {
            this.transform.Translate(Vector2.left * F_Speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider!=null)
        {
            if (this.name == "Hero_bullet(Clone)")
            {
                Read_Comp.Instance.THI_HitEffect(1);
            }
            else
            {
                Read_Comp.Instance.THI_HitEffect(2);
            }
               
            Destroy(this.gameObject);
        }
    }
}
