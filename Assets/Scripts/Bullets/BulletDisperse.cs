using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDisperse : MonoBehaviour {

    public GameObject Bullet;
    public int Amount;
    public float DisperseTime;
    public float Force;
    private float m_TimeUntilDisperse;
    private GameObject m_SpawnedBullet;
	// Use this for initialization
	void Start () {
        m_TimeUntilDisperse = Time.time + DisperseTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_TimeUntilDisperse < Time.time)
        //Disperse
        {
            for(int i = 0; i < Amount; i++)
            {
               // if(GameManager.singleton.BulletManager.GetComponent<BulletManager>().CheckIfCanBullet())
               // {
                    m_SpawnedBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                    int DegreeAmount = 360 / Amount;

                    Vector2 dir = new Vector2(Mathf.Cos((DegreeAmount*i) * Mathf.Deg2Rad), 
                        Mathf.Sin((DegreeAmount * i) * Mathf.Deg2Rad));
                    m_SpawnedBullet.GetComponent<Rigidbody2D>().AddForce(dir * Force * Time.deltaTime, ForceMode2D.Impulse);
                //}
            }
            Destroy(gameObject);
        }
	}
}
