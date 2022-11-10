using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train_engine : MonoBehaviour
{
    public static train_engine OBJ_train_Engine;
    public bool B_TrainIn;
    public float speed = 3f;
    public Vector3 V3_Startpos;
    public AudioSource AS_Effect;
    private void Awake()
    {
        OBJ_train_Engine = this;
        V3_Startpos = this.transform.parent.transform.position;
    }
    public void Update()
    {
        if(B_TrainIn)
        {
            transform.parent.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
    public void THI_MoveTrain()
    {
        AS_Effect.Play();
        B_TrainIn = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Start_Parallax")
        {
            B_TrainIn = false;
            Main_trainsorting.OBJ_Main_trainsorting.THI_PlaneStart();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.collider.name == "Train_Respawn")
        {
            THI_StartPos();
        }
    }

    public void THI_StartPos()
    {
        AS_Effect.Stop();
        transform.parent.transform.position = V3_Startpos;
        B_TrainIn = false;
        Main_trainsorting.OBJ_Main_trainsorting.THI_Transition();
    }


   
   
}
