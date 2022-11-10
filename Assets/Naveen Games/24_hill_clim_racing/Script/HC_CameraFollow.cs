using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HC_CameraFollow : MonoBehaviour
{
    public Transform T_TargetPlayer;
    Vector3 VEC3_offset;
    public float F_smoothspeed;


    void Start()
    {
        if(T_TargetPlayer==null)
        { T_TargetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); }
        
        VEC3_offset = transform.position - T_TargetPlayer.position;
    }


    void FixedUpdate()
    {
        if(T_TargetPlayer.gameObject.name=="Character") // for water finding game
        {
            Vector3 DesiredPosition = T_TargetPlayer.position + VEC3_offset;
            Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, F_smoothspeed);
            transform.position = new Vector3(0f, SmoothPosition.y, -100);
        }
        else
        {
            Vector3 DesiredPosition = T_TargetPlayer.position + VEC3_offset;
            Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, F_smoothspeed);
            transform.position = new Vector3(SmoothPosition.x, SmoothPosition.y, -100);
        }
       
       // Debug.Log("FOLLOWING PLAYER!");

    }
}
