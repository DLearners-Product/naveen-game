using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    Vector2 dPosition;
    public BodyPart following = null;
    private bool isTail = false;
    SpriteRenderer SPR_Renderer =null;

    const int PartsRemembered = 10;
    public Vector3[] PreviousPosition = new Vector3[PartsRemembered];
    public int setIndex = 0;
    public int getIndex = -(PartsRemembered - 1);

    private void Awake()
    {
        SPR_Renderer = GetComponent<SpriteRenderer>();
    }

   
    // Update is called once per frame
    virtual public void Update()
    {
        if (!Snake_Main.Instance.B_Alive) return;

        if (Time.timeScale != 0)
        {
            Vector3 FollowPosition;
            if (following != null)
            {
                if (following.getIndex > -1)
                    FollowPosition = following.PreviousPosition[following.getIndex];
                else
                    FollowPosition = following.transform.position;
            }
            else
            {
                FollowPosition = gameObject.transform.position;
            }

            PreviousPosition[setIndex].x = gameObject.transform.position.x;
            PreviousPosition[setIndex].y = gameObject.transform.position.y;
            PreviousPosition[setIndex].z = gameObject.transform.position.z;

            setIndex++;
            if (setIndex >= PartsRemembered) setIndex = 0;

            getIndex++;
            if (getIndex >= PartsRemembered) getIndex = 0;

            if (following != null)
            {
                Vector3 newPosition;
                if (following.getIndex > -1)
                {
                    newPosition = FollowPosition;
                }
                else
                {
                    newPosition = following.transform.position;
                }

                newPosition.z = newPosition.z + 0.01f;

                SetMovement(newPosition - gameObject.transform.position);
                UpdateDirection();
                UpdatePosition();
            }
        }
    }

    public void SetMovement(Vector2 movement)
    {
        dPosition = movement;
    }
    public void UpdatePosition()
    {
        gameObject.transform.position += (Vector3)dPosition;
    }
    public void UpdateDirection()
    {
        if (dPosition.y > 0) //up
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        else if (dPosition.y < 0) //down
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
        else if (dPosition.x < 0) //left
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
        else if (dPosition.x > 0) //right
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);
    }
    public void TurnIntoTail()
    {
        isTail = true;
        SPR_Renderer.sprite = Snake_Main.Instance.SPR_Tail;
    }

    public void TurnIntoBodyPart()
    {
        isTail = false;
        SPR_Renderer.sprite = Snake_Main.Instance.SPR_Bodysprite;
    }
}
