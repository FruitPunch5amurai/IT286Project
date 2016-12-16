using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public float health;
    float lastDmg;
    float recoveryTime = 0.5f;

	// Use this for initialization
	void Start () {
        lastDmg = Time.time - recoveryTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.01f) {
            //GetComponent<EnemyAI>().stunned = false; .... or something like this
        }
	}

    public void getHit(float dmg, Vector2 knockback) {
        if (Time.time - lastDmg > recoveryTime)
        {
            health -= dmg;
            lastDmg = Time.time;
            GetComponent<Rigidbody2D>().velocity = knockback;
            //Should probably stun the enemy until the knockback is resolved
            //GetComponent<EnemyAI>().stunned = true; .... or something like this

            if (health <= 0)
            {
                //Die
                Destroy(gameObject);
            }
        }
    }
}
