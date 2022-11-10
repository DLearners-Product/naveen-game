using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_SwipeControls : MonoBehaviour
{
    
    public bool B_Tap, B_swipeLeft, B_swipeRight, B_swipeUp, B_swipeDown;
    public Vector2 VEC2_StartTouch, VEC2_SwipeDelta;

    public bool B_isDragging = false;

    public Vector2 swipedelta { get { return VEC2_SwipeDelta; } }
    public bool swipeleft { get { return B_swipeLeft; } }
    public bool swiperight { get { return B_swipeRight; } }
    public bool swipeup { get { return B_swipeUp; } }
    public bool swipedown { get { return B_swipeDown; } }


    public static N_SwipeControls OBJ_swipe;

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
                if (x < 0)
                {
                   /* if(WS_PlayerControl.Instance!=null)
                    {
                        WS_PlayerControl.Instance.PUB_Directionselect(1);
                    }*/
                    if(Santa_Player.Instance!=null)
                    {
                        Santa_Player.Instance.B_Left = true;
                    }
                    if(Monkey_Player.Instance!=null)
                    {
                        Monkey_Player.Instance.PUB_Directionselect(1);
                    }
                   

                    // Debug.Log("left");
                }
                else
                {
                    /*if (WS_PlayerControl.Instance != null)
                    {
                        WS_PlayerControl.Instance.PUB_Directionselect(0);
                    }*/
                    if (Santa_Player.Instance != null)
                    {
                        Santa_Player.Instance.B_Right = true;
                    }
                    if (Monkey_Player.Instance != null)
                    {
                        Monkey_Player.Instance.PUB_Directionselect(0);
                    }
                    
                    //  Debug.Log("right");
                }
            }
            else
            {

                if (y < 0)
                {
                   /* if (WS_PlayerControl.Instance != null)
                    {
                        WS_PlayerControl.Instance.PUB_Directionselect(3);
                    }*/
                    if (ACI_Main.Instance != null)
                    {
                        ACI_Main.Instance.B_MoveUp = false;
                        ACI_Main.Instance.B_MoveDown = true;
                    }
                    if(Fish_sorting_main.Instance!=null)
                    {
                        Fish_sorting_main.Instance.B_Down = true;
                    }
                    if(Misfit_Main.Instance!=null)
                    {
                        Misfit_Main.Instance.B_MoveDown = true;
                    }
                    if (FW_PlayerController.Instance != null)
                    {
                        if (!FW_PlayerController.Instance.B_Jump)
                        {
                            FW_PlayerController.Instance.Jumping();
                          //  Debug.Log("Jump");
                        }
                    }
                    if (Monkey_Player.Instance != null)
                    {
                        Monkey_Player.Instance.PUB_Directionselect(3);
                    }
                    if(RiverRafting_Main.Instance!=null)
                    {
                        RiverRafting_Main.Instance.B_MoveDown = true;
                    }
                   
                    // Debug.Log("Down");
                }
                else
                {
                    /*if (WS_PlayerControl.Instance != null)
                    {
                        WS_PlayerControl.Instance.PUB_Directionselect(2);
                    }*/
                    if (ACI_Main.Instance != null)
                    {
                        ACI_Main.Instance.B_MoveUp = true;
                        ACI_Main.Instance.B_MoveDown=false;
                    }
                    if (Fish_sorting_main.Instance != null)
                    {
                        Fish_sorting_main.Instance.B_Up = true;
                    }
                    if (Misfit_Main.Instance != null)
                    {
                        Misfit_Main.Instance.B_Moveup = true;
                    }
                    if (Monkey_Player.Instance != null)
                    {
                        Monkey_Player.Instance.PUB_Directionselect(2);
                    }
                    if (RiverRafting_Main.Instance != null)
                    {
                        RiverRafting_Main.Instance.B_MoveUp = true;
                    }

                    // Debug.Log("Up");
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