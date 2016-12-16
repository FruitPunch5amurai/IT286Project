using UnityEngine;
using System.Collections;

public class EnemyAttackStationary : EnemyAttack {

    public bool AssignedTarget;
    private Vector3 dir;
    public float MaxDistance;
    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();

        if (m_timeUntilCanAttack <= Time.time && m_BulletManager.CheckIfCanBullet())
        {
            if (!AssignedTarget)
            {
                float dis = Vector3.Distance(m_Player.transform.position, transform.position);
                if (dis < MaxDistance)
                {
                    Target = m_Player;
                    dir = (m_Player.transform.position
                        - transform.position);

                    GameObject iBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                    m_BulletManager.Bullets.Add(iBullet);
                    iBullet.GetComponent<Rigidbody2D>().AddForce(dir.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                    m_timeUntilCanAttack = Time.time + AttackInterval;
                }
            }
            else
            {
                if (Target != null)
                {
                    dir = (Target.transform.position
                            - transform.position);
                    GameObject iBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);

                    iBullet.GetComponent<Rigidbody2D>().AddForce(dir.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Impulse);

                    m_timeUntilCanAttack = Time.time + AttackInterval;
                }
            }
        }
        }
}
