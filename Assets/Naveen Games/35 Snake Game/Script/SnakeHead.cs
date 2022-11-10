using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : BodyPart
{
    Vector2 movement;

    BodyPart tail = null;

    const float TimeToAddBodyPart = 0.1f;
    float addTimer = TimeToAddBodyPart;
    public int partstoAdd = 0;
    Vector3 V3_InitialPos;
    [SerializeField]
    GameObject G_BodyParent;
    public bool B_CallOnce;
    public AudioSource AS_Eating;
    public AudioSource AS_Out;
    int I_BodyCount;
    // Start is called before the first frame update
    void Start()
    {
        SnakeSwipe.OnSwipe += SwipeDetection;
        V3_InitialPos = this.transform.position;
        I_BodyCount = 5;
    }

    // Update is called once per frame
    override public void Update()
    {
        if (!Snake_Main.Instance.B_Alive) return;

        if(Time.timeScale!=0)
        {
            base.Update();
            SetMovement(movement);
            UpdateDirection();
            UpdatePosition();
        }
        

        if(partstoAdd>0)
        {
            addTimer -= Time.deltaTime;
            if(addTimer<=0)
            {
                addTimer = TimeToAddBodyPart;
                AddBodyPart();
                partstoAdd--;
            }
        }
    }
    void AddBodyPart()
    {
        if (tail == null)
        {
            Vector3 newposition = transform.position;
            newposition.z = newposition.z + 0.01f;

            BodyPart newPart = Instantiate(Snake_Main.Instance.G_BodyPrefab, newposition, Quaternion.identity);
            newPart.transform.SetParent(G_BodyParent.transform, false);
            newPart.following = this;
            tail = newPart;
            newPart.TurnIntoTail();
        }
        else
        {
            Vector3 newposition = tail.transform.position;
            newposition.z = newposition.z + 0.01f;

            BodyPart newPart = Instantiate(Snake_Main.Instance.G_BodyPrefab, newposition, Quaternion.identity);
            newPart.transform.SetParent(G_BodyParent.transform, false);
            newPart.following = tail;
            newPart.TurnIntoTail();
            tail.TurnIntoBodyPart();
            tail = newPart;
        }
    }
    void SwipeDetection(SnakeSwipe.SwipeDirection direction)
    {
        switch(direction)
        {
            case SnakeSwipe.SwipeDirection.Up:
                MoveUp();
                break;
            case SnakeSwipe.SwipeDirection.Down:
                MoveDown();
                break;
            case SnakeSwipe.SwipeDirection.Left:
                MoveLeft();
                break;
            case SnakeSwipe.SwipeDirection.Right:
                MoveRight();
                break;

        }
    }
    void MoveUp()
    {
        movement = Snake_Main.Instance.SnakeSpeed  * 0.035f * Vector2.up ;
    }
    void MoveDown()
    {
        movement = Snake_Main.Instance.SnakeSpeed  * 0.035f * Vector2.down ; // * Time.deltaTime
    }
    void MoveLeft()
    {
        movement = Snake_Main.Instance.SnakeSpeed  * 0.035f * Vector2.left ;
    }
    void MoveRight()
    {
        movement = Snake_Main.Instance.SnakeSpeed  * 0.035f * Vector2.right ;
    }

    public void DestroyBody()
    {
        for (int i = 0; i < G_BodyParent.transform.childCount; i++)
        {
            Destroy(G_BodyParent.transform.GetChild(i).gameObject);
        }
        Invoke(nameof(ResetSnake), 3f);
    }

    public void ResetSnake()
    {
        this.transform.position = V3_InitialPos;
        tail = null;
        MoveUp();
        partstoAdd = I_BodyCount;
      //  I_BodyCount = partstoAdd;
        addTimer = TimeToAddBodyPart;
        Snake_Main.Instance.B_Alive = true;
       
    }

    public void THI_SnakeGrow()
    {
        partstoAdd = 3;
        I_BodyCount = I_BodyCount + partstoAdd;
        addTimer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision =" + collision.gameObject.name);
        if(collision.gameObject.name=="Rock(Clone)")
        {
            Snake_Main.Instance.THI_Collect_Out(false);
            Snake_Main.Instance.GameOver();
          //  Debug.Log("Rock = Out");
        }
        else
        if (collision.gameObject.name == "Body(Clone)")
        {
            Snake_Main.Instance.THI_Collect_Out(false);
            Snake_Main.Instance.GameOver();
           // Debug.Log("Body = Out");
        }
        else
        if(collision.gameObject.transform.parent.transform.parent.name == "Content")
        {
            if(B_CallOnce)
            {
                B_CallOnce = false;
                this.GetComponent<Animator>().Play("SnakeEat");
                AS_Eating.Play();
                this.GetComponent<AudioSource>().clip=collision.gameObject.GetComponent<AudioSource>().clip;
                Snake_Main.Instance.THI_Check(collision.gameObject);
                this.GetComponent<AudioSource>().Play();
            }
            
           
           // Debug.Log("Grow");
        }
    }
}
