using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovementNormal : EnemyAI {
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
            Debug.Log("End of Path Reached");
            PathHasEnded = true;
            UpdateTarget();
            return;
        }
        PathHasEnded = false;


        switch (state)
        {
            case State.KnockedBack:
                if (m_IdleTime < Time.time)
                    state = PreviousState;
                return;

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
                    state = State.Attack;
                    m_enemyAttack.Target = GameManager.singleton.Player;
                    PathHasEnded = true;
                    UpdateTarget();
                    return;
                }
                break;
            default:
                break;
        }

        //Move AI
        Move();

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
        if (state == State.Patrol)
        {
            destination = new Vector3(Random.Range(StartPos.x - 1.0f, StartPos.x + 1.0f),
                        Random.Range(StartPos.y - 1.0f, StartPos.y + 1.0f),
                        -.001f);
        }
        else if (state == State.Attack)
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

