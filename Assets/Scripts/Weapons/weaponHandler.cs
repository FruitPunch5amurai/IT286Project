using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weaponHandler : MonoBehaviour {
    //State and inventory management
    bool hasWeapon;
    bool attacking;
    public string weaponState = "idle";
    List<Transform> weapons = new List<Transform>();
    GameObject curWeapon;

    //Weapon Pathing
    List<Vector2> playerPath = new List<Vector2>();
    float remainingDistance;
    float lastStop;
    public float followDelay = 0.5f;
    public float followDistance = 2.0f;

    //Basic Attack implementation
    public Vector2 basicOffset;
    Vector3 basicStart = new Vector3(1, 1, 0);
    Vector3 startPos;
    Quaternion startRot;
    Vector3 basicEnd = new Vector3(-1, 1, 0);
    float swingDelay = 1.0f;
    float lastAttack;

    //Special Attack implementation
    public Vector2 specialOffset;
    public Vector3 specialStart = new Vector3(1, 1, 0);


    //State transition management
    bool transitioning = false;
    public float transitionSpeed = 1.0f;

    //Local copies of weapon stats
    public float basicSwingSpeed;

    // Use this for initialization
    void Start () {
        if (curWeapon == null) hasWeapon = false;
        lastStop = Time.time;
        lastAttack = Time.time - swingDelay;
	}
	
	// Update is called once per frame
	void Update () {
        if (hasWeapon)
        {
            //Handle input
            if (transform.parent.GetComponent<PlayerControl>().hasControl)
            {
                if (Input.GetButtonDown("BasicAttack"))
                {
                    pickUpWeapon();
                    basicAttack();
                }
                if (Input.GetButtonDown("SpecialAttack")) specialAttack();
            }


            //Following behaviour
            if (!attacking)
            {
                if (curWeapon.transform.position != transform.parent.position)
                {
                    trackPlayer();
                }
                if ((Time.time - lastStop > followDelay) || ((curWeapon.transform.position - transform.position).sqrMagnitude > followDistance))
                {
                    followPlayer(transform.parent.GetComponent<PlayerControl>().moveSpeed * Time.deltaTime);
                }
            }
        }
        //Try to pick up a weapon
        else if (Input.GetButtonDown("BasicAttack")) {
            pickUpWeapon();
            if (curWeapon != null)
            {
                basicAttack();
            }
        }

        //Manage weapon states
        if (attacking)
        {
            if (weaponState == "basic")
            {
                if (transitioning)
                {
                    curWeapon.transform.position = Vector2.Lerp(curWeapon.transform.position, startPos, transitionSpeed);
                    curWeapon.transform.rotation = Quaternion.Lerp(curWeapon.transform.rotation, startRot, transitionSpeed);
                    if (((curWeapon.transform.position - startPos).sqrMagnitude < 0.1) && (Quaternion.Angle(curWeapon.transform.rotation, startRot) < 5)) {
                        curWeapon.transform.position = startPos;
                        curWeapon.transform.rotation = startRot;
                        transitioning = false;
                        curWeapon.GetComponent<IWeapon>().state = "basic";
                    }
                }
                else {
                    curWeapon.transform.rotation = Quaternion.Slerp(curWeapon.transform.rotation, Quaternion.LookRotation(curWeapon.transform.forward, (curWeapon.transform.position + transform.right * basicEnd.x + transform.up * basicEnd.y) - curWeapon.transform.position), basicSwingSpeed * Time.deltaTime);
                    if (Quaternion.Angle(curWeapon.transform.rotation, Quaternion.LookRotation(curWeapon.transform.forward, (curWeapon.transform.position + transform.right * basicEnd.x + transform.up * basicEnd.y) - curWeapon.transform.position)) < 1) weaponState = "idle";
                }

            }
            else if (weaponState == "special")
            {
                //Do nothing, let the weapon script handle it
                if (transitioning)
                {
                    curWeapon.transform.position = Vector2.Lerp(curWeapon.transform.position, startPos, transitionSpeed);
                    curWeapon.transform.rotation = Quaternion.Lerp(curWeapon.transform.rotation, startRot, transitionSpeed);
                    if (((curWeapon.transform.position - startPos).sqrMagnitude < 0.1) && (Quaternion.Angle(curWeapon.transform.rotation, startRot) < 5))
                    {
                        curWeapon.transform.position = startPos;
                        curWeapon.transform.rotation = startRot;
                        transitioning = false;
                        curWeapon.GetComponent<IWeapon>().specialAttack();
                    }
                }
            }
            else {
                //return to idle state
                attacking = false;
                lastAttack = Time.time;
                playerPath.Clear();
                weaponState = "idle";
                lastStop = Time.time;
                transform.parent.GetComponent<PlayerControl>().hasControl = true;
                curWeapon.GetComponent<IWeapon>().state = "idle";
            }
        }
    }

    public void hitStuff(List<GameObject> enemies, List<GameObject> projectiles, float damage, float knockbackStrength, Color[] deflections, float deflectionSpeed)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            //Whack Enemy
            //Probably gonna outsource this code into the Enemy scripts, when they exist

            enemies[0].GetComponent<EnemyHealth>().getHit(damage, (enemies[0].transform.position - transform.position).normalized * knockbackStrength);
            enemies.RemoveAt(0);
            
        }
        while (projectiles.Count > 0)
        {
            //Check if it can be Deflected
            for (int j = 0; j < deflections.Length; j++)
            {
                try
                {
                    if (projectiles[0].GetComponent<SpriteRenderer>().color == deflections[j])
                    {
                        projectiles[0].GetComponent<SpriteRenderer>().color = Color.yellow;

                        //Looking into a more nuanced deflection system
                        //Using a simple turn-around for testing
                        
                        if (projectiles[0].transform.parent != null)
                        {
                            //Deal with parent-child relationships
                            //projectiles[0].AddComponent<Rigidbody2D>().velocity = (projectiles[0].transform.position - transform.parent.position).normalized * deflectionSpeed;
                            //projectiles[0].GetComponent<Rigidbody2D>().gravityScale = 0;
                            //Transform rotator = projectiles[0].transform.parent;
                            //projectiles[0].transform.parent = null;
                            //rotator.GetComponent<BulletRotateAndDisperse>().children = rotator.GetComponentsInChildren<Transform>();
                        }
                        else {
                            projectiles[0].GetComponent<Rigidbody2D>().velocity = (projectiles[0].transform.position - transform.parent.position).normalized * deflectionSpeed;
                        }
                    }
                    projectiles.RemoveAt(0);
                }
                catch (MissingReferenceException e)
                {
                    projectiles.RemoveAt(0);
                }

            }
        }
    }

    void followPlayer(float distance) {
        //Move Weapon along the player's recorded path
        if (playerPath.Count > 0)
        {
            //Calculate movement Vector
            Vector3 target = new Vector3(playerPath[0].x, playerPath[0].y, curWeapon.transform.position.z) - curWeapon.transform.position;
            remainingDistance = distance - target.magnitude;

            //If the weapon moves past its destination, Do this again with the remaining distance
            if (remainingDistance >= 0)
            {
                curWeapon.transform.position = playerPath[0];
                playerPath.RemoveAt(0);
                followPlayer(remainingDistance);
            }
            else {
                curWeapon.transform.position += new Vector3(target.x, target.y, 0).normalized * distance;
            }
        }
        //If the path is empty, we've caught up to the player
        else {
            curWeapon.transform.position = transform.parent.position;
            lastStop = Time.time;
        }
    }

    void trackPlayer() {
        //Keep track of player's path
        playerPath.Add(transform.parent.position);
        
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Weapon") weapons.Add(coll.transform);

    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag == "Weapon")
        {
            if (!weapons.Contains(coll.transform)) weapons.Add(coll.transform);
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Weapon") weapons.Remove(coll.transform);
    }

    void pickUpWeapon()
    {
        if (weapons.Count == 0) return;

        float closestD = 0;
        int closestI = 0;
        for (int i = 0; i < weapons.Count; i++) {
            if (i == 0) {
                closestD = (transform.position - weapons[i].position).sqrMagnitude;
                closestI = i;
                continue;
            }
            if ((transform.position - weapons[i].position).sqrMagnitude < closestD) {
                closestD = (transform.position - weapons[i].position).sqrMagnitude;
                closestI = i;
            }
        }
        if (hasWeapon)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), curWeapon.GetComponent<Collider2D>(), false);
            curWeapon.GetComponent<IWeapon>().dropWeapon();
        }
        else {
            hasWeapon = true;
        }
        curWeapon = weapons[closestI].gameObject;
        curWeapon.GetComponent<IWeapon>().pickUp(gameObject);
        swingDelay = curWeapon.GetComponent<IWeapon>().swingDelay;
        //curWeapon.gameObject.layer = 8;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), curWeapon.GetComponent<Collider2D>(), true);
        weapons.RemoveAt(closestI);
        lastAttack = Time.time - swingDelay;
    }

    public void basicAttack() {
        //curWeapon.GetComponent<IWeapon>().basicAttack();

        //New code, testing
        if ((!attacking) && (Time.time - lastAttack > swingDelay)) {
            attacking = true;
            //New transition setup
            transitioning = true;
            startPos = transform.position + transform.right * basicOffset.x + transform.up * basicOffset.y;

            //Old teleport-based implementation
            //curWeapon.transform.position = transform.position + transform.right * basicOffset.x + transform.up * basicOffset.y;


            weaponState = "basic";
            startRot = Quaternion.LookRotation(transform.forward, (transform.position + transform.right * basicStart.x + transform.up * basicStart.y) - transform.position);
            transform.parent.GetComponent<PlayerControl>().hasControl = false;
        }
    }

    void specialAttack() {
        if (curWeapon.GetComponent<IWeapon>().requestSpecial())
        {
            attacking = true;
            //New transition setup
            transitioning = true;
            startPos = transform.position + transform.right * specialOffset.x + transform.up * specialOffset.y;


            weaponState = "special";
            startRot = Quaternion.LookRotation(transform.forward, (transform.position + transform.right * specialStart.x + transform.up * specialStart.y) - transform.position);
            transform.parent.GetComponent<PlayerControl>().hasControl = false;
        }
    }

}
