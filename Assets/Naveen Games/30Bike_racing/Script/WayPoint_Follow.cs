using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint_Follow : MonoBehaviour
{
    public GameObject[] WayPoints;
    public int I_WayPoint = 0;
    Transform Target;
    public float F_speed;
    public float speedModifier, angle, temp;
    public GameObject dummy;
    public bool B_GameStarted;
    // Start is called before the first frame update
    void Start()
    {
        Target = WayPoints[0].transform.GetChild(1);
        B_GameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(B_GameStarted)
        {
           // Debug.Log("MOve");
            Vector3 dir = Target.position - transform.position;


            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.Translate(dir.normalized * F_speed * Time.deltaTime);


            if (Vector3.Distance(transform.position, Target.position) <= 0.1f)
            {
                GetNextWaypoint();
            }
        }
       
       
    }

    void GetNextWaypoint()
    {
        if (I_WayPoint >= WayPoints[0].transform.childCount - 1)
        {
            I_WayPoint = 1;
            // Start Again
            return;
        }

        I_WayPoint++;
        Target = WayPoints[0].transform.GetChild(I_WayPoint);

        Vector3 New_p3 = WayPoints[0].transform.GetChild(I_WayPoint).transform.position;
        Vector3 Turnto = (New_p3 - this.transform.position).normalized;
        angle = Mathf.Atan2(-Turnto.y, -Turnto.x) * Mathf.Rad2Deg;
        temp = angle + 50f;
        dummy.transform.eulerAngles = new Vector3(0, 0, angle);

    }
}
