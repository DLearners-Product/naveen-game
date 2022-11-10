using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_coins : MonoBehaviour
{
    public Transform T_endPos;
    public float F_lerpTime;
    public bool B_CanCollectcoin;
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("T_end_pos") != null)
        {
            if (B_CanCollectcoin)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), F_lerpTime);
                transform.position = Vector2.Lerp(transform.position, T_endPos.position, F_lerpTime);
                B_CanCollectcoin = false;
                Invoke("THI_Destroy", 2f);
            }

        }
    }
    void THI_Destroy()
    {
        Destroy(this.gameObject);
    }
}
