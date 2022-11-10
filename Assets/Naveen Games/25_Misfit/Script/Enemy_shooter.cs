using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_shooter : MonoBehaviour
{
    public GameObject G_EnemyBulletPrefab;
    public GameObject Player;
    public Transform T_Childpos, T_bullet_clone;
    float temp, angle;
    public float F_Speed;
    public bool B_Fire;
    public Image I_Fill;
    

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        for (int i=0;i<this.transform.childCount;i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
        T_Childpos = this.transform.GetChild(0).transform;
        B_Fire = true;
        StartCoroutine(StartFire());
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 Taget_Cur_pos = Player.transform.position;

        Vector3 aimDirection = (Taget_Cur_pos - T_Childpos.position).normalized;
        angle = Mathf.Atan2(-aimDirection.y, -aimDirection.x) * Mathf.Rad2Deg;

        temp = angle + 50f;    

        T_Childpos.eulerAngles = new Vector3(0, 0, temp);
    }


    IEnumerator StartFire()
    {
       while(B_Fire)
       {
            for (int i = 0; i <3; i++)
            {
                if(B_Fire)
                {
                    yield return new WaitForSeconds(1.5f);
                    GameObject Bullets = Instantiate(G_EnemyBulletPrefab);
                    Bullets.transform.SetParent(this.transform, false);
                    Bullets.transform.position = T_bullet_clone.transform.position;

                    Bullets.GetComponent<Rigidbody2D>().velocity = (Player.transform.position - Bullets.transform.position).normalized * Time.deltaTime * F_Speed;
                }
                
            }
            yield return new WaitForSeconds(3f);
        }
       
    }

}
