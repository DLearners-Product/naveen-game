using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ansslice : MonoBehaviour
{
    public GameObject G_slice;
    GameObject slice;
    float startforce = 12f;
    Rigidbody2D rb;
   // public AudioSource AS_Cutting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up* startforce, ForceMode2D.Impulse);
        this.GetComponent<Image>().enabled = true;
        this.GetComponent<Collider2D>().enabled = true;
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("collide");
        if (collision.name == "Blade")
        {
            FruitNinja_Main.Instance.STR_currentSelectedAnswer = this.transform.GetChild(0).gameObject.GetComponent<Text>().text;
            //  Main_fruitslice.OBJ_main_Fruitslice.IncreaseScore();
            FruitNinja_Main.Instance.THI_Check();
           // AS_Cutting.Play();
            slice = Instantiate(G_slice, this.transform);
            slice.transform.position = this.transform.position;
            this.GetComponent<AudioSource>().Play();
            Invoke("triggerON", 1f);

            this.GetComponent<Collider2D>().enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.GetComponent<Image>().enabled = false;
        }
        if (collision.name == "missed")
        {
          //  Main_fruitslice.OBJ_main_Fruitslice.STR_Selectedans = this.transform.GetChild(0).gameObject.GetComponent<Text>().text;
          //  Main_fruitslice.OBJ_main_Fruitslice.Missedobjects();
        }

    }
    public void triggerON()
    {
        for (int i = 0; i < 1; i++)
        {
            G_slice.transform.GetChild(i).GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
