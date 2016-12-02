using UnityEngine;
using System.Collections;

public class BulletRotateAndDisperse : MonoBehaviour
{


    public float DisperseTime;
    public float Speed;
    private float TimeUntilDisperse;
    private Transform[] children;
    public Vector3 rotationMask = new Vector3(0,0, 1); //which axes to rotate around
    public float rotationSpeed; //degrees per second

    void Start()
    {
        TimeUntilDisperse = Time.time + DisperseTime;
        children = transform.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeUntilDisperse < Time.time)
        {
            foreach (Transform child in children)
            {
                Vector3 dir = -(transform.position - child.position).normalized;
                child.GetComponent<Rigidbody2D>().AddForce(dir*Speed*Time.deltaTime, ForceMode2D.Impulse);
            }
        }
            
        else
        {
            foreach (Transform child in children)
            {
                if (child != transform)
                    child.position = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime) * (child.position - transform.position) + transform.position;
            }
        }

    }
}
