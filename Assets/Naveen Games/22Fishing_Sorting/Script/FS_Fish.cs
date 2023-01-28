using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FS_Fish : MonoBehaviour
{
    bool B_CanMove;
    public float speed = 1f;
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
       
       
    }

    public void FishClicked()
    {
        Debug.Log("FishClicked");
        Fish_sorting_main.Instance.B_FishClicked = true;
        Fish_sorting_main.Instance.G_ClickedFish = this.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name== "Rope_end" && Fish_sorting_main.Instance.B_FishClicked == true)
        {
            
            
                if (Fish_sorting_main.Instance.B_CanCatched && Fish_sorting_main.Instance.B_FishClicked == true)
                {
                    B_CanMove = false;
                    // Debug.Log("this == " + this.transform.GetChild(0).GetComponent<Text>().text);
                    Fish_sorting_main.Instance.STR_currentSelectedAnswer = this.transform.GetChild(0).GetComponent<Text>().text;
                    Fish_sorting_main.Instance.THI_Catched();
                    this.transform.GetChild(0).GetComponent<AudioSource>().Play();
                    this.transform.SetParent(Fish_sorting_main.Instance.G_hook.transform.GetChild(0).transform, false);
                    this.transform.position = Fish_sorting_main.Instance.G_hook.transform.GetChild(0).transform.position;
                }
            
            
        }
        if(collision.gameObject.name== "DestroyFish")
        {
            Destroy(this.gameObject);
            Fish_sorting_main.Instance.B_FishClicked = false;
            Fish_sorting_main.Instance.G_ClickedFish = null;

        }
    }
}
