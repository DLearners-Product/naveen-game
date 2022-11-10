using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;
    private int routeToGo;
    float tparam;
    Vector3 CarPosition;
    public float speedModifier, angle, temp;
    bool coroutineAllowed;
    Transform T_Thispos;
    public GameObject dummy;
    


    //public float Angle;
    private void Start()
    {
        this.transform.localScale = new Vector2(0.3025f, 0.3025f);

        T_Thispos = this.transform;
        routeToGo = 0;
        tparam = 0f;
       // speedModifier = 0.25f;
        coroutineAllowed = true;
    }
    private void Update()
    {
        if (coroutineAllowed)
        {
            /*Vector3 New_p3 = routes[0].GetChild(0).position;
            Vector3 Turnto = (New_p3 - this.transform.position).normalized;
            angle = Mathf.Atan2(-Turnto.y, -Turnto.x) * Mathf.Rad2Deg;
            temp = angle + 50f;
            T_Thispos.eulerAngles = new Vector3(0, 0, temp);*/

            StartCoroutine(GobytheRoute(routeToGo)); 
        }

        T_Thispos.rotation = Quaternion.Lerp(this.transform.rotation, dummy.transform.rotation, Time.deltaTime * 3f);
    }

    private IEnumerator GobytheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        
       // Angle = routes[routeNumber].GetChild(0).transform.rotation.z;
       // this.transform.rotation = Quaternion.EulerRotation(0f, 0f, Angle*10f);

        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        Vector3 New_p3 = p3;
        Vector3 Turnto = (New_p3 - this.transform.position).normalized;
        angle = Mathf.Atan2(-Turnto.y, -Turnto.x) * Mathf.Rad2Deg;
        temp = angle + 50f;
        //  T_Thispos.eulerAngles = new Vector3(0, 0, temp);
        dummy.transform.eulerAngles = new Vector3(0, 0, temp);

        while (tparam<1)
        {
            tparam += Time.deltaTime * speedModifier;
            CarPosition= Mathf.Pow(1 - tparam, 3) * p0 +
                3 * Mathf.Pow(1 - tparam, 2) * tparam * p1 +
                3 * (1 - tparam) * Mathf.Pow(tparam, 2) * p2 +
                Mathf.Pow(tparam, 3) * p3;

            transform.position = CarPosition;

            yield return new WaitForEndOfFrame();
        }
        tparam = 0f;

      //  T_Thispos.eulerAngles = new Vector3(0, 0, temp);
        routeToGo +=1;

        if(routeToGo>routes.Length-1)
        {
            routeToGo = 0;
        }
        coroutineAllowed = true;



    }
}
