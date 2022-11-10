using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    public static Enemybullet OBJ_enemybullet;
    float speed = 5f;

    private void Start()
    {
        Invoke(nameof(Destroythis),8f);
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shooter")
        {
            Misfit_Main.Instance.THI_Enemybullethit();
            Destroythis();                 
        }
    }
    void Destroythis()
    {
        Destroy(this.gameObject);
    }


}
   
