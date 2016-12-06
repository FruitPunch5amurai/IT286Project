using UnityEngine;
using System.Collections;

public class RoomCollider : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    //This handles detecting what room the player is in
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "RoomDoor" && transform.parent.tag == "Player")
        {
            GameManager.singleton.TransitionRoom(col.transform.parent.GetComponent<DunGen.Doorway>().ConnectedDoorway.transform.parent.gameObject,
                col.transform.parent.GetComponent<DunGen.Doorway>().ConnectedDoorway.transform.GetChild(1).position);
        }
        else if(col.tag == "Room")
        {
            if (transform.parent.tag == "Player")
            {
                transform.parent.GetComponent<PlayerControl>().CurrentRoom = col.gameObject;
            }
            else if(transform.parent.tag == "Enemy")
            {
                transform.parent.GetComponent<EnemyAI>().CurrentRoom = col.gameObject;
            }
        }
    }
}
