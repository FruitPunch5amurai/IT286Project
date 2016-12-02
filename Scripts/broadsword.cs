using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class broadsword : MonoBehaviour, IWeapon {
    //Weapon Pickup Variables
    bool isHeld = false;
    public float throwSpeed = 25.0f;

    //Idle Behavior
    public string state = "idle";
    public Vector2 idleOffset;
    public bool occupied = false;

    //Hit Stuff
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> projectiles = new List<GameObject>();
    //Probably better off in the Projectile Controller
    public Color yellow;

    //Basic Attack
    public Vector2 basicOffset;
    public Vector3 basicStart = new Vector3 (1, 1, 0);
    public Vector3 basicEnd = new Vector3(-1, 1, 0);
    public float basicDmg;
    public float basicKnock;
    public float basicSwingSpeed = 2.0f;
    string[] basicDeflections = new string[1] { "Blue" };

    //Special Attack
    public Vector2 specialOffset;
    public Vector2 specialStart = new Vector2(0, 1);
    public float dashSpeed = 10.0f;
    public float dashDistance = 2.0f;
    private float lastDash;
    public float dashTime = 1.0f;
    public float dashCD = 3.0f;
    public float specialDmg;
    public float specialKnock;
    string[] specialDeflections = new string[1] { "Blue" };

    Transform player;
    //Glow Effect
    public Color targetColor = Color.yellow;
    private Color curColor;
    public float glowSpeed = 0.005f;
    public float colorThreshold = 0.1f;

    // Use this for initialization
    void Start () {
        lastDash = Time.time - dashCD;
        curColor = targetColor;
}
	
	// Update is called once per frame
	void Update () {
        if (isHeld)
        {
            if (state == "basic")
            {
                //do this
                hitStuff(basicDmg, basicKnock, basicDeflections);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(transform.forward, (transform.position + basicEnd) - transform.position), basicSwingSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.localRotation, Quaternion.LookRotation(transform.forward, (transform.position + basicEnd) - transform.position)) < 1) state = "idle";
                
            }
            else if (state == "special")
            {
                //do this
                hitStuff(specialDmg, specialKnock, specialDeflections);
                if (Time.time - lastDash < dashTime)
                {
                    player.GetComponent<playerController>().move(dashSpeed);
                }
                else {
                    state = "idle";
                }
            }
            else
            {
                //Reset behavior for idle
                if (occupied)
                {
                    transform.localPosition = idleOffset;
                    transform.up = transform.parent.up;
                    occupied = false;
                    player.GetComponent<playerController>().hasControl = true;
                }
                //I like immediate feedback in the controls, but turning animations could go here
                else
                {

                }
            }

            if (Time.time - lastDash > dashCD) glow();
        }
	}

    void hitStuff(float damage, float knockbackStrength, string[] deflections) {
        for (int i = 0; i < enemies.Count; i++) {
            //Whack Enemy
            //Probably gonna outsource this code into the Enemy scripts, when they exist
            /*
            EnemyController victim = enemies[i].GetComponent<EnemyController>();
            Vector2 knockback = (enemies[i].transform.position - player.transform.position).normalized * (knockbackStrength - victim.knockbackResistance);
            victim.health = victim.health - damage;
            victim.knocked = true;
            enemies[i].GetComponent<Rigidbody2D>().velocity = knockback;
            enemies.Remove(enemies[i]);
            */
        }
        for (int i = 0; i < projectiles.Count; i++) {
            //Check if it can be Deflected
            for (int j = 0; j < deflections.Length; j++) {
                /*
                if (projectiles[i].GetComponent<ProjectileController>().projectileType == deflect[j]) {
                    projectiles[i].GetComponent<SpriteRenderer>().color = yellow;
                    projectiles[i].GetComponent<ProjectileController>().projectileType = "Yellow";

                    //Looking into a more nuanced deflection system
                    //Using a simple turn-around for testing
                    projectiles[i].GetComponent<Rigidbody2D>().velocity = -projectiles[i].GetComponent<Rigidbody2D>().velocity;
                    
                }
                */
            }
        }
    }

    void glow() {
        if (Mathf.Abs(player.GetComponent<SpriteRenderer>().color.b - curColor.b) < colorThreshold) {
            if (curColor == targetColor)
            {
                curColor = Color.white;
            }
            else {
                curColor = targetColor;
            }
        }

        player.GetComponent<SpriteRenderer>().color = Color.Lerp(player.GetComponent<SpriteRenderer>().color, curColor, glowSpeed);
        
    }
    
    
    void IWeapon.basicAttack()
    {
        if (!occupied)
        {
            occupied = true;

            transform.localPosition = basicOffset;
            transform.localRotation = Quaternion.LookRotation(transform.forward, (transform.position + basicStart) - transform.position);
            state = "basic";
            transform.parent.parent.GetComponent<playerController>().hasControl = false;
        }
    }

    void IWeapon.specialAttack()
    {
        if ((!occupied) && (Time.time - lastDash > dashCD))
        {
            lastDash = Time.time;
            player.GetComponent<SpriteRenderer>().color = Color.white;
            occupied = true;
            transform.localPosition = specialOffset;
            state = "special";
            transform.parent.parent.GetComponent<playerController>().hasControl = false;
        }
    }

    void IWeapon.dropWeapon()
    {
        transform.SetParent(null);
        isHeld = false;
        GetComponent<Rigidbody2D>().velocity = transform.up * throwSpeed;
        GetComponent<Collider2D>().isTrigger = false;
    }

    void IWeapon.pickUp()
    {
        transform.localPosition = idleOffset;
        transform.up = transform.parent.up;
        isHeld = true;
        state = "idle";

        player = transform.parent.parent;
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D (Collider2D coll) {
        if (coll.gameObject.tag == "Enemy") enemies.Add(coll.gameObject);
        if (coll.gameObject.tag == "Projectile") projectiles.Add(coll.gameObject);
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (state == "idle")
        {
            if (coll.gameObject.tag == "Enemy")
            {
                if (!enemies.Contains(coll.gameObject)) enemies.Add(coll.gameObject);
            }
            if (coll.gameObject.tag == "Projectile")
            {
                if (!projectiles.Contains(coll.gameObject)) projectiles.Add(coll.gameObject);
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.tag == "Enemy") enemies.Remove(coll.gameObject);
        if (coll.tag == "Projectile") enemies.Remove(coll.gameObject);

    }

}
