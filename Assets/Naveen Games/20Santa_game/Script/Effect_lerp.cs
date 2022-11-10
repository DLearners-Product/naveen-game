using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_lerp : MonoBehaviour
{
    public GameObject G_Start, G_End;

    // Update is called once per frame
    void Update()
    {
        // this.transform.position = Vector3.Lerp(this.transform.position, G_End.transform.position, 0.003f);
        if (this.name == "Santa_coins(Clone)")
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, G_End.transform.position, 0.02f);
            this.transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(0.255f, 0.255f), 0.01f);
        }
           else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, G_End.transform.position, 0.01f);
            this.transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(0.30f, 0.30f), 0.01f);
        }
    }
}
