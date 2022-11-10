using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public static FollowingCamera OBJ_followingCamera;
    public bool B_canfollow;

    Transform T_TargetPlayer;
   
    public float X_Offset, Y_Offset;
    public void Awake()
    {
        OBJ_followingCamera = this;
        T_TargetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()   //player movement in fixed update for smoothness
    {
        if (B_canfollow)
        {
            Vector3 xtemp = transform.position;
            xtemp.x = T_TargetPlayer.position.x;
            xtemp.x += X_Offset;
            transform.position = xtemp;

            Vector3 ytemp = transform.position;
            ytemp.y = T_TargetPlayer.position.y;
            ytemp.y += Y_Offset;
            transform.position = ytemp;
        }
    }

}
