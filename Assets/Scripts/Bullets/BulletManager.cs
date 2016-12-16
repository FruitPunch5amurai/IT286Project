using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {


    public List<GameObject>Bullets;
    public int MaxBulletsInGame;

	//Clear Bullet Pool
    public bool CheckIfCanBullet()
    { 
        if(Bullets.Count >= MaxBulletsInGame)
        {
            return false;
        }
        return true;
    }
    public void ClearBullets()
    {
        foreach(GameObject b in Bullets)
        {
            if (b != null)
                Destroy(b);
        }
    }
}
