using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WS_PlayerControl : MonoBehaviour
{
    public static WS_PlayerControl Instance;
    bool[] B_Directions;
    
    public float movementspeed;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        B_Directions = new bool[4];
    }

    public void PUB_Directionselect(int count)
    {
        for(int i=0;i<B_Directions.Length;i++)
        {
            B_Directions[i] = false;
        }
        B_Directions[count] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(B_Directions[0])
        {
           // this.GetComponent<Animator>().Play("Walk");
            transform.Translate(Vector3.right * movementspeed * Time.deltaTime);
            transform.localScale = new Vector2(1 , 1);
        }
        else
        if (B_Directions[1])
        {
            transform.Translate(Vector3.left * movementspeed * Time.deltaTime);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        if (B_Directions[2])
        {
            transform.Translate(Vector3.down * movementspeed * Time.deltaTime);
        }
        else
        if (B_Directions[3])
        {
            transform.Translate(Vector3.up * movementspeed * Time.deltaTime);
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (WS_Main.Instance.B_Buttonclick)
        {
            string dummy = collision.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text;
            WS_Main.Instance.STR_currentSelectedAnswer = WS_Main.Instance.STR_currentSelectedAnswer + dummy;
        }
    }
}
