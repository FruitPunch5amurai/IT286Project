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
}
