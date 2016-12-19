using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovementTeleport : EnemyAI {

    public float FadeSpeed;
    public float IdleTime;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_CompletelyFaded;
    private Vector3 m_TeleportPosition;

    protected override void Start()
    {
        base.Start();
        state = State.Patrol;
        UpdateTarget();
        StartPos = transform.position;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_enemyAttack.Target = GameManager.singleton.Player;

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
            if (m_CompletelyFaded)
            {
                FadeIn();
            }
            if (!m_CompletelyFaded)
            {
                Debug.Log("UpdatedTarget");
                PathHasEnded = true;
                UpdateTarget();
            }
            return;
        }
        PathHasEnded = false;

        if (m_IdleTime > Time.time)
        {
            if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < 4f 
                && state != State.Attack 
                && PreviousState != State.KnockedBack
                )
            {
                state = State.Attack;
            }

            if (state == State.Idle || state == State.KnockedBack)
            {
                return;
            }
        }
        else
        {
            if (!m_CompletelyFaded)
            {
                FadeOut();
                if (state != State.Teleporting)
                    state = State.Teleporting;
            }
        }
        if (m_CompletelyFaded)
            Move();

            //rb.transform.Translate(direction);

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
        else
        {
           
            destination = new Vector3(Random.Range(m_Player.transform.position.x - 1.0f, m_Player.transform.position.x + 1.0f),
                        Random.Range(m_Player.transform.position.y - 1.0f, m_Player.transform.position.y + 1.0f),
                        -.001f);
      
        }
        
        GraphNode gn = GameManager.singleton.AStarGrid.GetComponent<AstarPath>().GetNearest(destination).node;
        var gnV = (Vector3)gn.position;
        Target = new Vector3(gnV.x, gnV.y, gnV.z);
        m_Seeker.StartPath(transform.position, Target, OnPathComplete);
    }
    void FadeOut()
    {
        m_SpriteRenderer.color = Color.Lerp(m_SpriteRenderer.color, Color.clear, FadeSpeed * Time.deltaTime);
        if(m_SpriteRenderer.color.a <= .05f)
        {
            m_SpriteRenderer.color = Color.clear;
            m_CompletelyFaded = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    void FadeIn()
    {
        m_SpriteRenderer.color = Color.Lerp(m_SpriteRenderer.color, Color.white, FadeSpeed * Time.deltaTime);
        if (m_SpriteRenderer.color.a >= .95f)
        {
            m_SpriteRenderer.color = Color.white;
            m_CompletelyFaded = false;
            m_IdleTime = Time.time + IdleTime;
            GetComponent<BoxCollider2D>().enabled = true;
            state = PreviousState ;
        }

    }
}