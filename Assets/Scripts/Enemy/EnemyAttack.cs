using UnityEngine;
using System.Collections;

public abstract class EnemyAttack : MonoBehaviour {

    public float AttackInterval;

    protected EnemyAI m_EnemyAI;
    protected GameObject m_Player;
    protected float m_timeUntilCanAttack;
    protected Vector3 m_dir;
    protected RaycastHit2D ray;
    protected bool m_hit;
    protected int m_ingnoreLayer = 1 << 8;
    public GameObject Bullet;
    public float BulletSpeed;
    public GameObject Target;
    // Use this for initialization
    protected virtual void Start () {
        m_EnemyAI = GetComponent<EnemyAI>();
        m_Player = GameManager.singleton.Player;
        m_ingnoreLayer = ~m_ingnoreLayer;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate() {
        
	}
    
}
