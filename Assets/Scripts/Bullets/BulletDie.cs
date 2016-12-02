using UnityEngine;
using System.Collections;

public class BulletDie : MonoBehaviour {

    public float LifeTime;
    private float m_timeUntilDie;
	// Use this for initialization
	void Start () {
        m_timeUntilDie = Time.time + LifeTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_timeUntilDie < Time.time)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Destroy(gameObject);
    }
}
