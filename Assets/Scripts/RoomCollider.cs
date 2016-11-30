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
    }
}
