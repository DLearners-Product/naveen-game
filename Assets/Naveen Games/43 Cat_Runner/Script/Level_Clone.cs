using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Clone : MonoBehaviour
{
    Transform Levelstart;
   // public GameObject Camera;
    public GameObject[] G_levels;
    public int I_Count;
    public float Parallax_Speed;
    public GameObject G_Cat;
    Vector3 LastEndPosition;
    int I_Index;
    // Start is called before the first frame update
    void Start()
    {
        //CloneSets();
        LastEndPosition = this.transform.position;
    }

    void CloneSets()
    {
        I_Count++;
        if (I_Count % 5 == 0)
        {
            I_Index = 3;
        }
        else
        {
            I_Index = Random.Range(0, G_levels.Length - 1);
        }
         
        GameObject Levels = Instantiate(G_levels[I_Index]);
        Levels.transform.SetParent(this.transform, false);
        Levels.transform.position = LastEndPosition;
        LastEndPosition = Levels.transform.Find("EndPosition").position;
        
       // Debug.Log("Clone count :"+ I_Count+ Levels.name);
       if(this.transform.childCount>4)
       {
            Destroy(this.transform.GetChild(0).gameObject);
       }
    }

    // Update is called once per frame
    private void Update()
    {
       // this.transform.Translate(Vector3.left * Parallax_Speed * Time.deltaTime);

        if(Vector3.Distance(G_Cat.transform.position, LastEndPosition) < 80f)
        {
            CloneSets();
        }
    }
}
