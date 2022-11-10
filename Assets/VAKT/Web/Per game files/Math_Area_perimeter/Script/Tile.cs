using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color BaseColor, OffsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] GameObject G_Highlight;
  
    public void Init(bool isOffset)
    {
        renderer.color = isOffset ? OffsetColor : BaseColor;
    }

    private void OnMouseEnter()
    {
        G_Highlight.SetActive(true);

    }

    private void OnMouseExit()
    {
        G_Highlight.SetActive(false);
    }
}
