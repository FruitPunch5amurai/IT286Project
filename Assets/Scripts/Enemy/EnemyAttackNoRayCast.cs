using UnityEngine;
using System.Collections;

public class EnemyAttackNoRayCast : EnemyAttack {

	// Use this for initialization
	protected override void Start () {
        base.Start();
	
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
        base.FixedUpdate();
        if (m_timeUntilCanAttack <= Time.time)
        {
            if (m_EnemyAI.CurrentState == EnemyAI.State.Attack)
            {
                //Debug.Log("Attack has occured!");
                Vector3 dis = (Target.transform.position
                    - transform.position);
                 //Attack goes here
                 GameObject iBullet = (GameObject)Instantiate(Bullet, transform.position, Quaternion.identity);
                 iBullet.GetComponent<Rigidbody2D>().AddForce(dis.normalized * BulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
                 m_timeUntilCanAttack = Time.time + AttackInterval;
            }
        }
    }
}
