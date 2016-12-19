using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackGormogon : EnemyAttack{

    public GameObject SmallerBullet;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        m_timeUntilCanAttack = Time.time + AttackInterval;
	}

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (m_timeUntilCanAttack <= Time.time && m_BulletManager.CheckIfCanBullet())
        {
            Vector3 dis = (m_Player.transform.position
                    - transform.position);
            if (m_Player != null)
            {
                //Attack goes here
                GameObject iBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                iBullet.GetComponent<Rigidbody2D>().AddForce(dis.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                iBullet.transform.localScale *= 2;

                iBullet.AddComponent<BulletDisperse>();
                BulletDisperse bd= iBullet.GetComponent<BulletDisperse>();

                //Set Info
                bd.Amount = 6;
                bd.Bullet = SmallerBullet;
                bd.DisperseTime = .8f;
                bd.Force = 50;

                m_BulletManager.Bullets.Add(iBullet);
            }
            m_timeUntilCanAttack = Time.time + AttackInterval;
        }
    }
}
