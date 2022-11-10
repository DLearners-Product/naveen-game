using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling_Ground : MonoBehaviour
{
    public AudioSource AS_fall;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AS_fall.Play();
    }
}
