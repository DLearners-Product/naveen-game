using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid_Manager : MonoBehaviour
{
    public static Grid_Manager Instance;
    [SerializeField] private int width, height;
    [SerializeField] private Tile TilePrefab;
    [SerializeField] private Camera CAM;

    public int I_Count, I_Name;
    public GameObject G_LastObject;
    public TextMeshProUGUI TMP_Area;
    private void Start()
    {
        Instance = this;
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                var spawnedTile = Instantiate(TilePrefab, new Vector3(x, y), Quaternion.identity);
                I_Name++;
                spawnedTile.name = I_Name.ToString();
                spawnedTile.transform.SetParent(this.transform, false);
                var isOffset = (x % 2 == 0 && y%2!=0)||(x%2!=0 && y%2==0);
                spawnedTile.Init(isOffset);
            }
        }
      //  CAM.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -100f);
    }

    public  void IncreaseArea()
    {
        I_Count++;
        TMP_Area.text = "Area : " + I_Count;
    }
}
