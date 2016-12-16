using UnityEngine;
using System.Collections;

public class BulletRotateAndDisperse : MonoBehaviour
{


    public float DisperseTime;
    public float Speed;
    private float TimeUntilDisperse;
    public Transform[] children;
    private bool m_Dipsersed;
    public float rotationSpeed; //degrees per second

    void Start()
    {
        m_Dipsersed = false;
        TimeUntilDisperse = Time.time + DisperseTime;
        children = transform.GetComponentsInChildren<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (TimeUntilDisperse < Time.time && m_Dipsersed == false)
        {
            if (GetComponent<Rigidbody2D>())
                Destroy(GetComponent<Rigidbody2D>());

            foreach (Transform child in children)
            {
                if (child != null && child != transform)
                {
                    Vector3 dir = -(transform.position - child.position).normalized;
                    child.GetComponent<Rigidbody2D>().AddForce((new Vector3(dir.x * Speed, dir.y * Speed, 0) * Time.deltaTime), ForceMode2D.Impulse);

                }
            }
            m_Dipsersed = true;
        }
        //Handle Rotation
        else if (m_Dipsersed == false)
        {
            foreach (Transform child in children)
            {
                if (child != null)
                {
                    if (child != transform)
                    {
                        child.position = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime) * (child.position - transform.position) + transform.position;
                        //child.position = (child.position - transform.position) + transform.position;

                    }
                }
            }
        }
    }
}
