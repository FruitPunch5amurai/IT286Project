using UnityEngine;
using System.Collections;

public class EnemyAttackShotGun : EnemyAttack {

    private float m_CurrentBulletAngle;
    public int BulletAmount;
    public float Spread;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (m_timeUntilCanAttack <= Time.time)
        {
            if (Target != null)
            {
                Vector3 dir = (Target.transform.position
                       - transform.position);
                dir = dir.normalized;
                Quaternion rotation = Quaternion.identity;
                for (int i = 0; i < BulletAmount; i++)
                {
                    GameObject bullet;
                    m_CurrentBulletAngle = (1 / Mathf.Tan(dir.y / dir.x)) - i * Spread;
                    Vector3 secondDir = Quaternion.AngleAxis(m_CurrentBulletAngle + 20, Vector3.forward) * dir;

                    bullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().AddForce(secondDir.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }
                m_timeUntilCanAttack = Time.time + AttackInterval;
            }

        }
    }
}
