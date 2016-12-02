using UnityEngine;
using Pathfinding;
using System.Threading;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public abstract class EnemyAI : MonoBehaviour {

    public float Speed;

    //Enemies current room
    public GameObject CurrentRoom;
    protected GameObject m_Player;

    //The max distance from the AI to the waypoint for it to continue to next waypoint
    public float NextWayPointDistance = 3f;

    [HideInInspector]
    public bool PathHasEnded = false;

    public Vector3 Target;
    protected Vector3 StartPos;  
    public ForceMode2D FMode;
    public ABPath path = null;
    
    //Some Caching
    protected Seeker m_Seeker;
    protected Rigidbody2D rb;
    //Waypoint we are currently moving toward
    protected int CurrentWaypoint = 0;
    protected EnemyAttack m_enemyAttack;


    public enum State
    {
        Patrol,
        Attack,
    }

    public State CurrentState;



    // Use this for initialization
     protected virtual void Start () {
        m_Seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        m_Player = GameManager.singleton.Player;
        m_enemyAttack = GetComponent<EnemyAttack>();

	}

    public void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.Log("AI: Got Path, Detected error");
            return;
        }
        
        if (path != null)
        {
            path.Release(gameObject);
        }
        path = p as ABPath;
        path.Claim(gameObject);
        CurrentWaypoint = 0;
        PathHasEnded = false;

    }

     protected virtual void FixedUpdate()
    {
        
    }

    protected virtual void UpdateTarget()
    {

    }

}
