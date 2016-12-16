using UnityEngine;
using System.Collections;

public class SlowStatusEffect : MonoBehaviour,IEnemyStatusEffect {

    private EnemyAI p_Movement;
    private float SlowAmount;
    private float p_TimeUntilStatusEnd;
    public float StatusDuration;
    private float p_Speed;
    // Use this for initialization
	void Start () {
	    if(GetComponent<EnemyAI>())
        {
            p_Movement = GetComponent<EnemyAI>();
            p_Speed = p_Movement.Speed;
        }
        SlowAmount = p_Speed / 2;
        ApplyStatusEffect();
	}
    
    void Update()
    {
        if (p_TimeUntilStatusEnd < Time.time)
        {
            p_Movement.Speed = p_Speed;
            Destroy(this);
        }

    }
    public void ApplyStatusEffect()
    {
        p_Movement.Speed = SlowAmount;
        p_TimeUntilStatusEnd = Time.time + StatusDuration;
    }
}
