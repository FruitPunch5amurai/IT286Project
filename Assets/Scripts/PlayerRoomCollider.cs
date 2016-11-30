using UnityEngine;
using System.Collections;

public class PlayerRoomCollider : MonoBehaviour {

    public GameObject room;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //This handles detecting what room the player is in
    void OnTriggerEnter(Collider col)
    {
        if(col.tag== "Room")
        {
            Vector3 newPos = col.transform.parent.GetComponent<DunGen.Doorway>().ConnectedDoorway.transform.GetChild(1).position;
            Debug.Log("Trigger: RoomTransition");
            room = col.transform.parent.GetComponent<DunGen.Doorway>().ConnectedDoorway.transform.parent.gameObject;
            GetComponentInParent<PlayerMove>().CurrentRoom = room;
            transform.parent.position = new Vector3 (newPos.x,newPos.y,-.001f);
            Camera.main.GetComponent<CameraMove>().SetCameraBoundary();

        }

    }
}
