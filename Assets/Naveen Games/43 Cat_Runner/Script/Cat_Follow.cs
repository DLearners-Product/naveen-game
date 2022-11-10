using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Follow : MonoBehaviour
{
    public static Cat_Follow OBJ_followingCamera;
    public bool B_canfollow = false;

    Transform T_TargetPlayer;

    public float X_Offset, Y_Offset;



    public void Awake()
    {
        OBJ_followingCamera = this;

    }
    private void LateUpdate()   //player movement in fixed update for smoothness
    {
        if (B_canfollow)
        {
            T_TargetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 xtemp = transform.position;
            xtemp.x = T_TargetPlayer.position.x;
            xtemp.x += X_Offset;
            transform.position = xtemp;

           /* Vector3 ytemp = transform.position;
            ytemp.y = T_TargetPlayer.position.y;
            ytemp.y += Y_Offset;
            transform.position = ytemp;*/
        }
    }
}