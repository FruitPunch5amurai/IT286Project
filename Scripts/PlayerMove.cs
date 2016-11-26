using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour {

    public GameObject CurrentRoom;
    public GameObject adjacentTile;
    public float Horizontal;
    public float Vertical;
    public float MoveSpeed;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        MoveSpeed = 2.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.singleton.CurrentGameState != GameManager.GameState.Play) return;
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        rb.transform.Translate(new Vector3(Horizontal* MoveSpeed, Vertical * MoveSpeed,0) * Time.deltaTime);
        

    }



    void GetCurrentRoom()
    {


       // Vector2 roomPos;
       // Vector2 roomDim;
       // BoxCollider roomBox;
       //  Transform[] listOfRooms = GameManager.singleton.Dungeon.GetComponentsInChildren<Transform>();
       // Vector2 playerPos = new Vector2(GetComponent<BoxCollider2D>().transform.position.x, GetComponent<BoxCollider2D>().transform.position.y);
       // Vector2 playerDim = new Vector2(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y);
       // foreach (Transform t in listOfRooms)
       // {
       //
       //     roomBox = t.GetComponent<BoxCollider>();
       //     roomPos =new Vector2(roomBox.transform.position.x, roomBox.transform.position.y);
       //     roomDim = new Vector2(roomBox.size.x, roomBox.size.y);
       //     if (t.gameObject != CurrentRoom)
       //     {
       //         if(Mathf.Abs(playerPos.x) + playerDim.x >= roomPos.x &&
       //             Mathf.Abs(playerPos.x) <= Mathf.Abs(roomPos.x) + roomDim.x &&
       //             Mathf.Abs(playerPos.y) >= Mathf.Abs(roomPos.y) + roomDim.y &&
       //             Mathf.Abs(playerPos.y) + playerDim.y <= roomDim.y)
       //         {
       //             Debug.Log("player intersecting a room");
       //         }
       //
       //     }
       // }
    }
}
