using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class BossMovementGormogon : EnemyAI
{
    private bool m_SecondForm;
    //public GameObject Tentecle;
    //public List<GameObject> Tentecles;
    //public int MaxTentecles;
    //private Vector3[] TenteclesPositions;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        UpdateTarget();
        IsABoss = true;
        GameManager.singleton.Canvas.GetComponent<CanvasController>().SetBossHealth(gameObject);

        //TenteclesPositions = new Vector3[MaxTentecles];
        //for(int i = 0; i < MaxTentecles;i++)
        //{
        //    TenteclesPositions[i] = Tentecles[i].transform.position;
        //}
        //m_enemyAttack.Target = GameManager.singleton.Player;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (path == null)
        {
            return;
        }
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

        if (Vector3.Distance(Target, GameManager.singleton.Player.transform.position) > 1.2f)
        {
            PathHasEnded = true;
            UpdateTarget();
        }
        if (Vector3.Distance(transform.position, GameManager.singleton.Player.transform.position) < .6f)
        {
            Vector3 dir = -(m_Player.transform.position - transform.position).normalized;
            rb.AddForce(dir * Speed * Time.fixedDeltaTime, FMode);
            return;

        }

        //Move AI
        Move();
        if (Vector3.Distance(transform.position, path.vectorPath[CurrentWaypoint]) < NextWayPointDistance)
        {
            CurrentWaypoint++;
            return;
        }
        CheckIfCanSecondForm();

    }
    void CheckIfCanSecondForm()
    {
       //if(transform.childCount != MaxTentecles)
       //{
       //    StartCoroutine(SpawnTentecle());
       //}
        if(transform.childCount == 0 && !m_SecondForm)
        {
            m_SecondForm = true;
            m_enemyAttack.enabled = true;
            GetComponent<EnemyHealth>().Invincible = false;
            Debug.Log("Final Form!");
        }
       // if(transform.childCount != 0)
       // {
       //     GetComponent<EnemyHealth>().Invincible = true;
       // }
    }
    //IEnumerator SpawnTentecle()
    //{
    //    yield return new WaitForSeconds(1f);
    //    bool taken = false;
    //    GameObject t = Instantiate(Tentecle);
    //    foreach(Vector3 pos in TenteclesPositions)
    //    {
    //        
    //        foreach(Transform trans in transform.GetComponentsInChildren<Transform>())
    //        {
    //            if (trans.position == pos)
    //                taken = true;
    //        }
    //        if (taken == false)
    //            t.transform.position = pos;
    //    }
    //    t.transform.parent = transform;
    //    Tentecles.Add(t);
    //
    //}
    protected override void UpdateTarget()
    {

        //base.UpdateTarget();
        Vector3 destination = new Vector3(0, 0, 0);
        destination = GameManager.singleton.Player.transform.position;
        GraphNode gn = GameManager.singleton.AStarGrid.GetComponent<AstarPath>().GetNearest(destination).node;
        var gnV = (Vector3)gn.position;
        Target = new Vector3(gnV.x, gnV.y, gnV.z);
        //GameManager.singleton.PathRequests.Add(gameObject);
        m_Seeker.StartPath(transform.position, Target, OnPathComplete);
    }





    public override void KnockBack(Vector2 dir, float force)
    {
        return;
    }
}
