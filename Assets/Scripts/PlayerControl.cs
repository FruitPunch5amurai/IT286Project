using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    public float moveSpeed = 2.0f;
    public bool hasControl = true;
    public GameObject WeaponController;

    public GameObject CurrentRoom;
    public Sprite n;
    public Sprite e;
    public Sprite s;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.singleton.CurrentGameState != GameManager.GameState.Play) return;
        if (hasControl)
        {
            //Rotate Weapon Hitbox
            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
                transform.GetChild(0).rotation = Quaternion.LookRotation(transform.forward, (Camera.main.transform.right * Input.GetAxisRaw("Horizontal") + Camera.main.transform.up * Input.GetAxisRaw("Vertical")));

            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
            {
                move(moveSpeed);
            }


            //Change Sprites, waiting for Input

            if (Input.GetKey(KeyCode.UpArrow))
            {
                GetComponent<SpriteRenderer>().sprite = n;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                GetComponent<SpriteRenderer>().sprite = s;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                GetComponent<SpriteRenderer>().sprite = e;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<SpriteRenderer>().sprite = e;
                GetComponent<SpriteRenderer>().flipX = true;
            }

        }

    }

    public void move(float speed)
    {
        Vector3 input = WeaponController.transform.up * Time.deltaTime * speed;

        RaycastHit2D move;
        RaycastHit2D move2;
        if (input.x < 0)
        {
            move2 = Physics2D.Raycast(transform.position, new Vector2(input.x, 0), input.x, ~(1 << 8));
        }
        else
        {
            move2 = Physics2D.Raycast(transform.position, new Vector2(input.x, 0), input.x, ~(1 << 8));
        }
        if (input.x < 0)
        {
            move = Physics2D.BoxCast(transform.position + Camera.main.transform.right * GetComponent<BoxCollider2D>().size.x * -1, GetComponent<BoxCollider2D>().size, 360, new Vector2(input.x, 0), input.x, ~(1 << 8));
        }
        else
        {
            move = Physics2D.BoxCast(transform.position + Camera.main.transform.right * GetComponent<BoxCollider2D>().size.x * 1, GetComponent<BoxCollider2D>().size, 360, new Vector2(input.x, 0), input.x, ~(1 << 8));
        }
        if (!move2)
        {
            if (!move) transform.position = transform.position + new Vector3(input.x, 0, 0);
        }
        else transform.position = new Vector3(move.centroid.x, move.centroid.y, transform.position.z);



        if (input.y < 0)
        {
            move2 = Physics2D.Raycast(transform.position, new Vector2(0, input.y), input.y, ~(1 << 8));
        }
        else
        {
            move2 = Physics2D.Raycast(transform.position, new Vector2(0, input.y), input.y, ~(1 << 8));
        }
        if (input.y < 0)
        {
            move = Physics2D.BoxCast(transform.position + Camera.main.transform.up * GetComponent<BoxCollider2D>().size.y * -1, GetComponent<BoxCollider2D>().size, 360, new Vector2(0, input.y), input.y, ~(1 << 8));
        }
        else
        {
            move = Physics2D.BoxCast(transform.position + Camera.main.transform.up * GetComponent<BoxCollider2D>().size.y * 1, GetComponent<BoxCollider2D>().size, 360, new Vector2(0, input.y), input.y, ~(1 << 8));
        }
        if (!move2)
        {
            if (!move) transform.position = transform.position + new Vector3(0, input.y, 0);
        }
        else transform.position = new Vector3(move.centroid.x, move.centroid.y, transform.position.z);
    }

}
