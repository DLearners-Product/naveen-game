using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Player : MonoBehaviour
{
    GameObject Object;
    public AudioSource AS_Collect, AS_Wrong;
    Vector3 tmpPos;
    void Update()
    {
        if (ACI_Main.Instance != null)
        {
            if (ACI_Main.Instance.B_MoveUp)
            {
                transform.Translate(Vector3.up * 10f * Time.deltaTime); ACI_Main.Instance.B_MoveUp = false;

                //Debug.Log("Temp ")
                // this.transform.position = tmpPos;
            }
            if (ACI_Main.Instance.B_MoveDown)
            {
                transform.Translate(Vector3.down * 10f * Time.deltaTime); ACI_Main.Instance.B_MoveDown = false;
                /*Vector3 tmpPos = this.transform.position;
                //tmpPos.x = Mathf.Clamp(tmpPos.x, -3f, -3f);
                tmpPos.y = Mathf.Clamp(tmpPos.y, -2f, 2f);
                this.transform.position = tmpPos;*/
            }
            if (ACI_Main.Instance.B_MoveFishFront) { transform.Translate(Vector3.right * 2f * Time.deltaTime); }

            tmpPos = this.transform.position;
            //tmpPos.x = Mathf.Clamp(tmpPos.x, -3f, -3f);
            tmpPos.y = Mathf.Clamp(tmpPos.y, -3f, 2f);
            this.transform.position = tmpPos;
        }
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cave_Inside")
        {
            ACI_Main.Instance.B_MoveFishFront = false;
            ACI_Main.Instance.THI_Transition();
            //Inside cave gameTHI_ShowQuestion
        }
        if (collision.gameObject.name == "Shark(Clone)")
        {
            Object = collision.gameObject;
            ACI_Main.Instance.THI_Injured();
            AS_Wrong.Play();
            Destroy(Object);
            StartCoroutine(FishRed());
        }
        if (collision.gameObject.transform.parent.name == "Jelly")
        {
            Object = collision.gameObject;
            ACI_Main.Instance.THI_Injured();
            AS_Wrong.Play();
            Object.SetActive(false);
            StartCoroutine(FishRed());
        }
        if (collision.gameObject.transform.parent.name == "Coins")
        {
            Object = collision.gameObject;

            // collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            // collision.gameObject.GetComponent<Collect_coins>().B_CanCollectcoin = true;
            ACI_Main.Instance.THI_CoinsCollected();
            AS_Collect.Play();
            Object.SetActive(false);
        }

    }

    IEnumerator FishRed()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f); 
        this.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
    }
}
