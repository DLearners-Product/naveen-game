using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Blade_slicing : MonoBehaviour
{
    public static Blade_slicing OBJ_blade_Slicing;
    public bool iscutting = false;
    Rigidbody2D rb;
    public Camera cam;
    CircleCollider2D circleCollider;
    public float mincutvelocity = .001f;

    public GameObject G_bladetrail;
    GameObject currenttrail;
    Vector2 previouspos;
    public bool formtrail = true;
    public AudioSource AS_Slicing;

    void Start()
    {
        OBJ_blade_Slicing = this;
       // cam = Camera.main;
        rb = this.GetComponent<Rigidbody2D>();
        circleCollider = this.GetComponent<CircleCollider2D>();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            StartCutting();
        }
        else
        if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }
        if (iscutting)
        {
            Updatecut();
        }
    }
    void Updatecut()
    {
        Vector2 newpos = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newpos;

        float velocity = (newpos - previouspos).magnitude * Time.deltaTime;
        if (velocity > mincutvelocity)
        {
            circleCollider.enabled = true;
            //formtrail = true;
        }
        else
        {
            circleCollider.enabled = false;
            //formtrail = false;
        }
        previouspos = newpos;
    }
    public void StartCutting()
    {
        iscutting = true;
        circleCollider.enabled = true;
        if (formtrail)
        {
            AS_Slicing.Play();
            currenttrail = Instantiate(G_bladetrail, transform);
        }
        previouspos = cam.ScreenToWorldPoint(Input.mousePosition);

    }
    public void StopCutting()
    {
        iscutting = false;
        circleCollider.enabled = false;
        //formtrail = false;
        if (currenttrail != null)
        {
            currenttrail.transform.SetParent(null);
            Destroy(currenttrail);
        }

    }
}
