using UnityEngine;
using System.Collections;

public class EnemyAttackNoRayCast : EnemyAttack {


    public int MaxBurst;
    public float BurstRate;
    private float m_ShootNext;
    private float m_Count;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        m_ShootNext = Time.time;
        m_Count = 0;
	
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        if (m_timeUntilCanAttack <= Time.time && m_BulletManager.CheckIfCanBullet())
        {
            
            if (m_EnemyAI.CurrentState == EnemyAI.State.Attack && m_BulletManager.CheckIfCanBullet())
            {
                if (m_ShootNext < Time.time)
                {
                    //Debug.Log("Attack has occured!");
                    Vector3 dis = (Target.transform.position
                        - transform.position);
                    //Attack goes here
                    GameObject iBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                    m_BulletManager.Bullets.Add(iBullet);
                    iBullet.GetComponent<Rigidbody2D>().AddForce(dis.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    m_ShootNext = Time.time + BurstRate;
                    m_Count++;
                    if (m_Count >= MaxBurst)
                    {
                        m_timeUntilCanAttack = Time.time + AttackInterval;
                        m_Count = 0;
                    }
                }
            }
        }
    }
}
