using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class synSwipeControls : MonoBehaviour
{
    public bool B_Tap, B_swipeLeft, B_swipeRight, B_swipeUp, B_swipeDown;
    public Vector2 VEC2_StartTouch, VEC2_SwipeDelta;


    public bool B_isDragging = false;



    public Vector2 swipedelta { get { return VEC2_SwipeDelta; } }
    public bool swipeleft { get { return B_swipeLeft; } }
    public bool swiperight { get { return B_swipeRight; } }
    public bool swipeup { get { return B_swipeUp; } }
    public bool swipedown { get { return B_swipeDown; } }


    public static synSwipeControls OBJ_synswipe;


    public void Update()
    {
        B_Tap = B_swipeLeft = B_swipeRight = B_swipeUp = B_swipeDown = false;

        #region standaloneinput
        if (Input.GetMouseButtonDown(0))
        {
            B_Tap = true;
            B_isDragging = true;
            VEC2_StartTouch = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            B_isDragging = false;
            THI_ResetPosition();
        }
        #endregion
        #region mobileinput
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                B_isDragging = true;
                B_Tap = true;
                VEC2_StartTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                B_isDragging = false;
                THI_ResetPosition();
            }
        }
        #endregion



        //calculations
        VEC2_SwipeDelta = Vector2.zero;
        if (B_isDragging)
        {
            if (Input.touches.Length > 0)
            {
                VEC2_SwipeDelta = Input.touches[0].position - VEC2_StartTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                VEC2_SwipeDelta = (Vector2)Input.mousePosition - VEC2_StartTouch;
            }
        }



        //crossdeadline
        if (VEC2_SwipeDelta.magnitude > 125)
        {

            float x = VEC2_SwipeDelta.x;
            float y = VEC2_SwipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {

            }
            else
            {
                if (y < 0)
                {
                    Robotmovement.OBJ_robotmovement.down();
                }
                else
                {
                    Robotmovement.OBJ_robotmovement.jump();
                }
            }
        }
    }

    public void THI_ResetPosition()
    {
        VEC2_StartTouch = VEC2_SwipeDelta = Vector2.zero;
        B_isDragging = false;
    }
}