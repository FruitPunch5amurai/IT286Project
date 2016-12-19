using UnityEngine;
using Pathfinding;
using System.Threading;
using System.Collections;

public abstract class EnemyAI : MonoBehaviour {

    public float Speed;
    protected float m_IdleTime;
    protected float m_LastTimeKnockedBack;
    public bool CanKnockBack;
    public bool IsABoss;

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
        Teleporting,
        Idle,
        KnockedBack

    }

    protected State state
    {
        get { return CurrentState; }
        set
        {
            PreviousState = CurrentState;
            CurrentState = value;
        }
    }
    public State CurrentState;
    public State PreviousState;


    // Use this for initialization
     protected virtual void Start () {
        m_Seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        m_Player = GameManager.singleton.Player;
        m_enemyAttack = GetComponent<EnemyAttack>();
        IsABoss = false;
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
    protected virtual void Move()
    {
        //Find direction to next node
        Vector3 direction = (path.vectorPath[CurrentWaypoint] - transform.position).normalized;
        direction *= Speed * Time.fixedDeltaTime;
        direction.z = 0;
        //Time to move the AI!
        rb.transform.Translate(direction);

    }
    public virtual void KnockBack(Vector2 dir,float force)
    {
        if (state == State.KnockedBack || Time.time - m_LastTimeKnockedBack < 1f || !CanKnockBack)
            return;
        state = State.KnockedBack;
        m_IdleTime = Time.time + 1f;
        m_LastTimeKnockedBack = Time.time;
        rb.AddForce(dir, ForceMode2D.Impulse);
    }
    
}
