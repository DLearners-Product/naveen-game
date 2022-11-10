using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bomb_FN : MonoBehaviour
{
    public GameObject G_slice;
    GameObject slice;
    float startforce = 12f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * startforce, ForceMode2D.Impulse);
        this.GetComponent<Image>().enabled = true;
        this.GetComponent<Collider2D>().enabled = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("collide");
        if (collision.name == "Blade")
        {
            FruitNinja_Main.Instance.THI_Wrong();
            Vector3 pos= this.transform.position;
            this.GetComponent<Image>().enabled = false;
            this.GetComponent<Collider2D>().enabled = false;
            startforce = 0;
            slice = Instantiate(G_slice, this.transform.parent.transform);
            slice.transform.position = pos;
            
            Invoke(nameof(THI_Destroy), 2f);
        }
      
    }
    public void THI_Destroy()
    {
        Destroy(this.gameObject);
    }
}
