using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenteclesFloat : MonoBehaviour {

    private bool m_FloatUp;
    private float m_FloatTime;
    private Rigidbody2D rb;
    public float FloatSpeed;

	// Use this for initialization
	void Start () {
        m_FloatUp = false;
        rb = GetComponent<Rigidbody2D>();
        m_FloatTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(m_FloatUp)
        {
            transform.Translate(new Vector2(0, 1f) * FloatSpeed * Time.deltaTime);
            if (m_FloatTime + 1f < Time.time)
            {
                m_FloatUp = false;
                m_FloatTime = Time.time;
            }
        }
        else if (!m_FloatUp)
        {
            transform.Translate(new Vector2(0, -1f) * FloatSpeed * Time.deltaTime);
            if (m_FloatTime + 1f < Time.time)
            {
                m_FloatUp = true;
                m_FloatTime = Time.time;
            }
        }
	}

}
