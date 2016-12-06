using UnityEngine;
using System.Collections;

public class BulletRotateAndDisperse : MonoBehaviour
{


    public float DisperseTime;
    public float Speed;
    private float TimeUntilDisperse;
    public Transform[] children;
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
                if (child != null)
                {
                    Vector3 dir = -(transform.position - child.position).normalized;
                    child.Translate(new Vector3(dir.x * Speed, dir.y * Speed, 0) * Time.deltaTime);
                }
            }
        }
        else
        {
            foreach (Transform child in children)
            {
                if (child != null)
                {
                    if (child.name == "SpinningProjectile(Clone)")
                    {
                        Debug.Log("YEP");
                    }
                    else if (child != transform)
                        child.position = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime) * (child.position - transform.position) + transform.position;
                }
            }
        }
    }
}
