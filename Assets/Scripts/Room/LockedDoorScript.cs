using UnityEngine;
using System.Collections;
using DunGen;

public class LockedDoorScript : MonoBehaviour, IKeyLock
{
    public Key Key { get { return keyManager.GetKeyByID(keyID); } }

    [HideInInspector]
    [SerializeField]
    private int keyID;

    [HideInInspector]
    [SerializeField]
    private KeyManager keyManager;

    private Vector3 m_Position;
    private Door door;


    // Use this for initialization
    void Start () {
        door = GetComponent<Door>();
	}
    public void OnKeyAssigned(Key key, KeyManager keyManager)
    {
        keyID = key.ID;
        this.keyManager = keyManager;
        Debug.Log("DoorScript!");
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (GameManager.singleton.HasBossKey)
            {
                Debug.Log("Opened Boss Door");
                GameManager.singleton.GiveBossKey(false);
                Open();
            }
            else
            {
                Debug.Log("Key required");
            }
        }
    }
    private void Open()
    {
        door.IsOpen = true;
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update () {
	
	}
    
}
