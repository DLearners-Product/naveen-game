using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FS_Fish : MonoBehaviour
{
    bool B_CanMove;
    public float speed=1f;
    private void Start()
    {
        B_CanMove = true;
    }
    private void Update()
    {
        if(B_CanMove)
        {
            this.transform.Translate(Vector3.left *speed* Time.deltaTime);
        }
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("FishClicked");
            Fish_sorting_main.Instance.B_FishClicked = true;
            Fish_sorting_main.Instance.ClickedFish = this.gameObject;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name== "Rope_end")
        {
            if(Fish_sorting_main.Instance.B_CanCatched)
            {
               // Debug.Log("this == " + this.transform.GetChild(0).GetComponent<Text>().text);
                Fish_sorting_main.Instance.STR_currentSelectedAnswer = this.transform.GetChild(0).GetComponent<Text>().text;
                Fish_sorting_main.Instance.THI_Catched();
                this.transform.GetChild(0).GetComponent<AudioSource>().Play();
                
                B_CanMove = false;
                this.transform.SetParent(Fish_sorting_main.Instance.G_hook.transform.GetChild(0).transform, false);
                this.transform.position = Fish_sorting_main.Instance.G_hook.transform.GetChild(0).transform.position;
            }
        }
        if(collision.gameObject.name== "DestroyFish")
        {
            Destroy(this.gameObject);
        }
    }
}
