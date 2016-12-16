using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyA :EnemyAI{


    
    protected override void Start()
    {
        base.Start();
        UpdateTarget();
        StartPos = transform.position;

    }
    protected override void FixedUpdate()
    {
        
        base.FixedUpdate();
        if (path == null)
        {
            //Debug.Log("AI:Path is null");
            return;
        }
        //define Attack behavior
        if (PathHasEnded) return;
        //Check to see if we are at our last node in our path
        if (CurrentWaypoint >= path.vectorPath.Count)
        {
            //Debug.Log("End of Path Reached");
            PathHasEnded = true;
            UpdateTarget();
            return;
        }
        PathHasEnded = false;
       //TODO: define Attack behavior

       switch (CurrentState)
       {
           case State.Attack:
               if (Vector3.Distance(Target, GameManager.singleton.Player.transform.position) > 1f)
               {
                   PathHasEnded = true;
                   UpdateTarget();
               }
               if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .4f)
               {
                   Vector3 dir = -(m_Player.transform.position - transform.position).normalized;
                   rb.AddForce(dir * Speed * Time.fixedDeltaTime, FMode);
                   return;
       
               }
                else if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .8f)
                {
                    PathHasEnded = true;
                    UpdateTarget();
                    return;
                }
       
               break;
           //define Patrol behavior
           case State.Patrol:
       
               if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .8f)
               {
                   CurrentState = State.Attack;
                    m_enemyAttack.Target = GameManager.singleton.Player;
                    PathHasEnded = true;
                    UpdateTarget();
                    return;
                }
               break;
           default:
               break;
       }
        //Find direction to next node
        Vector3 direction = (path.vectorPath[CurrentWaypoint] - transform.position).normalized;
        direction *= Speed * Time.fixedDeltaTime;
        direction.z = 0;
        //Time to move the AI!

        rb.transform.Translate(direction);
        //if ((transform.position - Target).sqrMagnitude < .1f)
        //{
        //    UpdateTarget();
        //}

        if (Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]) < NextWayPointDistance)
        {
            CurrentWaypoint++;
            return;
        }
    }
    protected override void UpdateTarget()
    {
        base.UpdateTarget();
        Vector3 destination = new Vector3(0, 0, 0);
        if (CurrentState == State.Patrol)
        {
            destination = new Vector3(Random.Range(StartPos.x - 1.0f, StartPos.x + 1.0f),
                        Random.Range(StartPos.y - 1.0f, StartPos.y + 1.0f),
                        -.001f);
        }
        else if (CurrentState == State.Attack)
        {
            destination = GameManager.singleton.Player.transform.position;
        }
        GraphNode gn = GameManager.singleton.AStarGrid.GetComponent<AstarPath>().GetNearest(destination).node;
        var gnV = (Vector3)gn.position;
        Target = new Vector3(gnV.x, gnV.y, gnV.z);
        //GameManager.singleton.PathRequests.Add(gameObject);

        m_Seeker.StartPath(transform.position, Target, OnPathComplete);
    }

}
