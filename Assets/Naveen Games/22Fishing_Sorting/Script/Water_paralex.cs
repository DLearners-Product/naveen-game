using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Water_paralex : MonoBehaviour
{

    float length, startpos;
    public GameObject Camera;
    public float Parallax_Speed;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<Image>().sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
       // if (Fish_sorting_main.Instance != null)
           // if (Pogostick_Main.Instance.B_MoveBG)
         //   {
                transform.Translate(Vector3.left * Parallax_Speed * Time.deltaTime);

                if (transform.position.x > startpos + length)
                {
                    startpos -= length;
                }
                else if (transform.position.x < startpos - length)
                {
                    this.transform.position = new Vector3(startpos, this.transform.position.y, this.transform.position.z);
                }
          //  }

    }
}
