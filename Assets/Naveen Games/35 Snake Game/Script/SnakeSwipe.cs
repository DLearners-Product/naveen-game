using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSwipe : MonoBehaviour
{
    Vector2 swipeStart;
    Vector2 swipeEnd;
    float minimunDistance = 10;
    bool B_right, B_left, B_up, B_down;

    public static event System.Action<SwipeDirection> OnSwipe = delegate { };

    private void Start()
    {
        B_up = true;
    }
    public enum SwipeDirection
    {
        Up,Down,Left,Right
    };

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                ProcessSwipe();
            }
        }


        if(Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            ProcessSwipe();
        }


        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(!B_down)
            {
                OnSwipe(SwipeDirection.Up);
                B_up = true;
                B_left = B_right = B_down = false;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(!B_up)
            {
                OnSwipe(SwipeDirection.Down);
                B_down = true;
                B_left = B_up = B_right = false;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(!B_left)
            {
                OnSwipe(SwipeDirection.Right);
                B_right = true;
                B_down = B_up = B_left = false;
            }
           
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(!B_right)
            {
                OnSwipe(SwipeDirection.Left);
                B_left = true;
                B_down = B_up = B_right = false;
            }
            
        }
    }
    void ProcessSwipe()
    {
        float distance = Vector2.Distance(swipeStart, swipeEnd);
        if (distance > minimunDistance)
        {
            if(IsVerticalSwipe())
            {
                if(swipeEnd.y>swipeStart.y)
                {
                    if (!B_down)
                    {
                        OnSwipe(SwipeDirection.Up);
                        B_up = true;
                        B_left = B_right = B_down = false;
                    }
                }
                else
                {
                    if (!B_up)
                    {
                        OnSwipe(SwipeDirection.Down);
                        B_down = true;
                        B_left = B_up = B_right = false;
                    }
                }
            }
            else
            {
                if(swipeEnd.x>swipeStart.x)
                {
                    if (!B_left)
                    {
                        OnSwipe(SwipeDirection.Right);
                        B_right = true;
                        B_down = B_up = B_left = false;
                    }
                }
                else
                {
                    if (!B_right)
                    {
                        OnSwipe(SwipeDirection.Left);
                        B_left = true;
                        B_down = B_up = B_right = false;
                    }
                }
            }
        }
    }

    bool IsVerticalSwipe()
    {
        float Vertical = Mathf.Abs(swipeEnd.y - swipeStart.y);
        float horizontal = Mathf.Abs(swipeEnd.x - swipeStart.x);
        if (Vertical > horizontal)
            return true;
        return false;
    }
}
