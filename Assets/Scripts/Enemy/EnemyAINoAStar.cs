using UnityEngine;
using System.Collections;

public class EnemyAINoAStar : MonoBehaviour {
    public float Speed = 3f;

    //Enemies current room
    public GameObject CurrentRoom;

    protected GameObject m_Player;
    public ForceMode2D FMode;
    public Vector3 Target;
    protected Vector3 StartPos;
    protected Rigidbody2D rb;

    public enum State
    {
        Patrol,
        Attack,
        Standby
    }
    public State CurrentState;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        m_Player = GameManager.singleton.Player;
        StartPos = transform.position;
        CurrentState = State.Patrol;
        UpdateTarget();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
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
       //            rb.AddForce(dir * Speed * Time.fixedDeltaTime,FMode);
       //            return;
       //
       //        }
       //       else if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .8f)
       //       {
       //            return;
       //           
       //       }
       //
       //        break;
       //    //define Patrol behavior
       //    case State.Patrol:
       //
       //        if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .5f)
       //        {
       //            CurrentState = State.Attack;
       //        }
       //        
       //        break;
       //    default:
       //        break;
       //}

        Vector3 direction = (Target- transform.position).normalized;
        direction *= Speed * Time.fixedDeltaTime;

        if ((transform.position - Target).sqrMagnitude < .1f)
        {
            UpdateTarget();
            return;
        }
        //Time to move the AI
        rb.AddForce(direction * Speed * Time.fixedDeltaTime,FMode);
        
    }
    void UpdateTarget()
    {
        if (CurrentState == State.Patrol)
        {
           // Debug.Log("Patrolling");
            Target = new Vector3(Random.Range(StartPos.x - 1.0f, StartPos.x + 1.0f),
                        Random.Range(StartPos.y - 1.0f, StartPos.y + 1.0f),
                        -.001f);
        }
        else if (CurrentState == State.Attack)
        {
           // Debug.Log("AI: Attacking!");
            Target = new Vector3(
                Random.Range(GameManager.singleton.Player.transform.position.x, transform.position.x),
                        Random.Range(GameManager.singleton.Player.transform.position.y, transform.position.y),
                        -.001f);
        }
    }
}
