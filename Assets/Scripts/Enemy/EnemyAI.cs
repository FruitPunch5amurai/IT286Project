using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {


    //How many times each second we will update our path
    public float UpdateRate = 2f;
    public float Speed = 3f;

    //Enemies current room
    public GameObject CurrentRoom;

    protected GameObject m_Player;

    //The max distance from the AI to the waypoint for it to continue to next waypoint
    public float NextWayPointDistance = 3f;

    [HideInInspector]
    public bool PathHasEnded = false;

    public Vector3 Target;
    protected Vector3 StartPos;
    public CircleCollider2D Sight;    
    public ForceMode2D FMode;
    public ABPath path = null;
    
    //Some Caching
    protected Seeker m_Seeker;
    protected Rigidbody2D rb;
    //Waypoint we are currently moving toward
    protected int CurrentWaypoint = 0;


    public enum State
    {
        Patrol,
        Attack,
        Standby
    }

    public State CurrentState;



    // Use this for initialization
     void Start () {
        m_Seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        m_Player = GameManager.singleton.Player;
        UpdateTarget();
        StartPos = transform.position;
        CurrentState = State.Patrol;
        
        //StartCoroutine(UpdatePath());
	}

    void OnPathComplete(Path p)
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

    }
   //  IEnumerator UpdatePath()
   // {
   //     if (Target == null) { }
   //     Debug.Log("AI: Path updated");
   //     m_Seeker.StartPath(transform.position, Target, OnPathComplete);
   //     yield return new WaitForSeconds(1f/UpdateRate);
   //     StartCoroutine(UpdatePath());
   // }

     void FixedUpdate()
    {
        if (Target == null)
        {
            Debug.Log("AI: Path was Null");
        }
        if (path == null)
        {
            Debug.Log("AI:Path is null");
            return;
        }

        //Check to see if we are at our last node in our path
        if (CurrentWaypoint >= path.vectorPath.Count)
        {
            if (PathHasEnded) return;

            Debug.Log("End of Path Reached");
            PathHasEnded = true;
            UpdateTarget();
            return;
        }
        PathHasEnded = false;
        //define Attack behavior

        //switch (CurrentState)
        //{
        //    case State.Attack:
        //        if (Vector3.Distance(Target, GameManager.singleton.Player.transform.position) > 1.2f)
        //        {
        //            UpdateTarget();
        //        }
        //        if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .4f)
        //        {
        //            Vector3 dir = -(m_Player.transform.position - transform.position).normalized;
        //            rb.AddForce(dir * Speed * Time.fixedDeltaTime, FMode);
        //            return;
        //
        //        }
        //        else if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .8f)
        //        {
        //            UpdateTarget();
        //            return;
        //        }
        //        
        //            break;
        //    //define Patrol behavior
        //    case State.Patrol:
        //        
        //        if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .5f)
        //        {
        //            CurrentState = State.Attack;
        //        }
        //        break;
        //    default:
        //        break;
        // }
        //Find direction to next node
        Vector3 direction = (path.vectorPath[CurrentWaypoint] - transform.position).normalized;
        direction *= Speed * Time.fixedDeltaTime;

        //Time to move the AI!
        
        rb.AddForce(direction, FMode);
      // if ((transform.position - Target).sqrMagnitude < .1f)
      // {
      //     UpdateTarget();
      // }
        
        if (Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]) < NextWayPointDistance)
        {
            CurrentWaypoint++;
            return;
        }
    }

     void UpdateTarget()
    {
        Vector3 destination = new Vector3(0, 0, 0);
        if (CurrentState == State.Patrol)
        {
            Debug.Log("Patrolling");
            destination = new Vector3(Random.Range(StartPos.x - 1.0f, StartPos.x + 1.0f),
                        Random.Range(StartPos.y - 1.0f, StartPos.y + 1.0f),
                        -.001f);
        }
        else if (CurrentState == State.Attack)
        {
            Debug.Log("AI: Attacking!");
            destination = GameManager.singleton.Player.transform.position;
            //destination = new Vector3(
            //    Random.Range(GameManager.singleton.Player.transform.position.x, transform.position.x),
            //            Random.Range(GameManager.singleton.Player.transform.position.y, transform.position.y),
            //            -.001f);
        }
        GraphNode gn = GameManager.singleton.AStarGrid.GetComponent<AstarPath>().GetNearest(destination).node;
        var gnV = (Vector3)gn.position;
        Target = new Vector3(gnV.x, gnV.y, gnV.z);
        m_Seeker.StartPath(transform.position, Target, OnPathComplete);
    }

}
