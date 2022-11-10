using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarzan_parallax : MonoBehaviour
{
    float length, startpos;
    public GameObject Camera;
    public float Parallax_Speed;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Tarzan_Main.Instance != null)
        {
            if (Tarzan_Main.Instance.G_Player.activeInHierarchy)
            {
                transform.Translate(Vector3.left * Parallax_Speed * Time.deltaTime);

                if (transform.position.x > startpos + length)
                {
                    startpos -= length;
                }
                else if (transform.position.x < startpos - length)
                {
                    this.transform.position = new Vector3(startpos, this.transform.position.y, this.transform.position.z);
                }
            }
        }


        /* float temp = (Camera.transform.position.x * (1 - Parallax_Speed));
         float dist = (Camera.transform.position.x * Parallax_Speed);
         transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (transform.position.x > startpos + length) { startpos += length; }
        else if (transform.position.x < startpos - length)
        {
            startpos -= length;
        }*/

    }
}
