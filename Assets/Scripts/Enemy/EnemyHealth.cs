using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public float MaxHealth;
    public float CurrentHealth;
    private bool m_Blink;
    float lastDmg;
    float recoveryTime = 0.5f;
    private float m_BlinkDuration;
    private float m_BlinkTime;
    public bool Invincible;
    private SpriteRenderer m_SpriteRenderer;
    // Use this for initialization
	void Start () {
        lastDmg = Time.time - recoveryTime;
        CurrentHealth = MaxHealth;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {            
        if(m_Blink == true)
        {
            DamageBlink();
        }
    }

    public void getHit(float dmg, Vector2 knockback) {
        if (Invincible)
            return;
        
        if (Time.time - lastDmg > recoveryTime)
        {
            CurrentHealth -= dmg;
            lastDmg = Time.time;
            m_Blink = true;
            m_BlinkDuration = Time.time + .5f;
            //Should probably stun the enemy until the knockback is resolved
            //GetComponent<EnemyAI>().stunned = true; .... or something like this

            if (GetComponent<EnemyAI>().IsABoss)
            {
                GameManager.singleton.Canvas.GetComponent<CanvasController>().UpdateBossHealth();
            }

            if (CurrentHealth <= 0)
            {
                //Die
                if (GetComponent<EnemyAI>().IsABoss)
                {
                    GameManager.singleton.Canvas.GetComponent<CanvasController>().RemoveBossHealth();
                }
                Destroy(gameObject);
            }
            
        }
    }
    void DamageBlink()
    {
        if(m_BlinkDuration > Time.time)
        {
            if (m_BlinkTime < Time.time)
            {
                if (m_SpriteRenderer.color == Color.white)
                    m_SpriteRenderer.color = Color.red;
                else
                    m_SpriteRenderer.color = Color.white;
                m_BlinkTime = Time.time + .1f;

            }
        }
        else
        {
            m_Blink = false;
            m_SpriteRenderer.color = Color.white;
        }
    }
}
