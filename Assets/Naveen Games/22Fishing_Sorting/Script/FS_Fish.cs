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
        Fish_sorting_main.Instance.G_ClickedFish = gameObject;
        // Fish_sorting_main.Instance.B_FishClicked = true;
        // StartCoroutine(Fish_sorting_main.Instance.THI_CatchFish());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Rope_end"){
            Debug.Log($"Other : "+collision.gameObject.name+" Game Object : "+gameObject.name+"\n B_cancatched : "+Fish_sorting_main.Instance.B_CanCatched+" B_FishClicked : "+Fish_sorting_main.Instance.B_FishClicked, gameObject);
        }

        if(collision.gameObject.name == "Rope_end" && Fish_sorting_main.Instance.G_ClickedFish != null)
        {
            Debug.Log("Came to 1st if statement", gameObject);

            if (Fish_sorting_main.Instance.B_CanCatched)
            {
                Debug.Log("Came to 2nd if statement", gameObject);
                B_CanMove = false;
                // Debug.Log("this == " + this.transform.GetChild(0).GetComponent<Text>().text);
                Fish_sorting_main.Instance.STR_currentSelectedAnswer = this.transform.GetChild(0).GetComponent<Text>().text;
                Fish_sorting_main.Instance.THI_Catched();
                this.transform.GetChild(0).GetComponent<AudioSource>().Play();
                this.transform.SetParent(Fish_sorting_main.Instance.G_hook.transform.GetChild(0).transform, false);
                this.transform.position = Fish_sorting_main.Instance.G_hook.transform.GetChild(0).transform.position;
            }else{
                Debug.Log("Came to 2nd else statement : "+Fish_sorting_main.Instance.B_CanCatched, gameObject);
            }
            
        }else{
            Debug.Log("Came to 1st else statement => \n GO Name : "+collision.gameObject.name+"\n Clicked Fish : "+((Fish_sorting_main.Instance.G_ClickedFish != null) ? Fish_sorting_main.Instance.G_ClickedFish.name : "NULL"), gameObject);
        }
        if(collision.gameObject.name== "DestroyFish")
        {
            Destroy(this.gameObject);
            // Fish_sorting_main.Instance.B_FishClicked = false;
            // Fish_sorting_main.Instance.G_ClickedFish = null;

        }
    }
}
