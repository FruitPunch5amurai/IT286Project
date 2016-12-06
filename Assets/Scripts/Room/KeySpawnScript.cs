using UnityEngine;
using System.Collections;
using DunGen;

public class KeySpawnScript : MonoBehaviour, IKeySpawnable
{

    public void SpawnKey(Key key, KeyManager manager)
    {
        var obj = (GameObject)Instantiate(key.Prefab,transform.position,Quaternion.identity);
        Debug.Log("Spawned key");
    }
}
