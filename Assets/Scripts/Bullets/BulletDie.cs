using UnityEngine;
using System.Collections;

public class BulletDie : MonoBehaviour {

    public float LifeTime;
    public bool m_dead;
    public float m_timeUntilDie;
    public bool IgnoreTerrain;
    // Use this for initialization
	void Start () {
        m_timeUntilDie = Time.time + LifeTime;
        m_dead = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (m_timeUntilDie < Time.time && !m_dead == true)
        {
            Destroy();
        }
    }

    IEnumerator DestroyBullet()
    {

        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        yield return new WaitForSeconds(.25f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Bullet b = GetComponent<Bullet>();
        if (col.tag == "Enemy")
        {
            if (b)
            {
                if (b.Deflected)
                {
                    //Take Damage!

                }
            }
            return;
        }
        else if (col.tag == "Terrain" && IgnoreTerrain)
            return;
        if(!m_dead == true)
            Destroy();
    }
    void Destroy()
    {
        m_dead = true;
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetBool("Dead", m_dead);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        StartCoroutine(DestroyBullet());
    }
}
