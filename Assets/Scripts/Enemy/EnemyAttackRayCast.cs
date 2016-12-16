using UnityEngine;
using System.Collections;

public class EnemyAttackRayCast : EnemyAttack {
    public LayerMask RaycastIgnore;
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
            if (m_EnemyAI.CurrentState == EnemyAI.State.Attack)
            {
                //Debug.Log("Attack has occured!");
                Vector3 dis = (Target.transform.position
                    - transform.position);
                ray = Physics2D.Raycast(transform.position, dis.normalized, Vector3.Distance(Target.transform.position
                    , transform.position), ~RaycastIgnore);
                Debug.DrawRay(transform.position, dis, Color.red);
                if (ray)
                {
                    if (ray.collider.tag == "Player")
                    {
                        //Attack goes here
                        GameObject iBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                        iBullet.GetComponent<Rigidbody2D>().AddForce(dis.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Force);

                    }
                }
                m_timeUntilCanAttack = Time.time + AttackInterval;
            }
        }
    }
}
