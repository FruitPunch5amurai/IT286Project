using UnityEngine;
using System.Collections;
using DunGen;
public class KeyScript : MonoBehaviour, IKeyLock {

    public Key Key { get { return keyManager.GetKeyByID(keyID); } }

    [HideInInspector]
    [SerializeField]
    private int keyID;

    [HideInInspector]
    [SerializeField]
    private KeyManager keyManager;

    public void OnKeyAssigned(Key key, KeyManager keyManager)
    {
        keyID = key.ID;
        this.keyManager = keyManager;
        Debug.Log("KeyScript");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Key Trigger");
        if (col.tag == "Player")
        {
            Debug.Log("Picked Up BossKey");
            GameManager.singleton.GiveBossKey(true);
            Destroy(gameObject);
        }
    }

}
