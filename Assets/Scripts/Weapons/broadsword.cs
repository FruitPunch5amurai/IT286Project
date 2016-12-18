using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class broadsword : MonoBehaviour, IWeapon {
    //Weapon Pickup Variables
    bool isHeld = false;
    public float throwSpeed = 25.0f;
    public GameObject weaponCont;

    //Idle Behavior
    private string localState = "idle";
    public string state
    {
        get
        {
            return localState;
        }
        set
        {
            localState = value;
        }
    }
    public Vector2 idleOffset;
    public bool occupied = false;

    //Hit Stuff
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> projectiles = new List<GameObject>();
    public float deflectionSpeed = 5.0f;
    public float DeflectionSpeed
    {
        get
        {
            return deflectionSpeed;
        }
    }

    //Basic Attack
    public float _SwingDelay = 0.5f;
    public float swingDelay {
        get {
            return _SwingDelay;
        }
    }
    public float basicDmg;
    public float BasicDamage
    {
        get
        {
            return basicDmg;
        }
    }
    public float basicKnock;
    public float BasicKnock
    {
        get
        {
            return basicKnock;
        }
    }
    public float basicSwingSpeed = 2.0f;
    Color[] basicDeflections = new Color[1] { Color.blue };
    public Color[] BasicDeflections
    {
        get
        {
            return basicDeflections;
        }
    }

    //Special Attack
    public Vector2 specialOffset;
    public Vector2 specialStart = new Vector2(0, 1);
    public float dashSpeed = 10.0f;
    private float dashStart;
    private float lastDash;
    public float dashTime = 1.0f;
    public float specialCD = 1.0f;
    public float specialDmg;
    public float specialKnock;
    Color[] specialDeflections = new Color[1] { Color.blue };
    

    Transform player;
    //Glow Effect
    public Color targetColor = Color.yellow;
    private Color curColor;
    public float glowSpeed = 0.005f;
    public float colorThreshold = 0.1f;

    // Use this for initialization
    void Start () {
        lastDash = Time.time - specialCD;
        curColor = targetColor;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (isHeld)
        {
            if (localState == "basic")
            {
                //do this
                weaponCont.GetComponent<weaponHandler>().hitStuff(enemies, projectiles, basicDmg, basicKnock, basicDeflections, deflectionSpeed);

            }
            else if (localState == "special")
            {
                //do this
                weaponCont.GetComponent<weaponHandler>().hitStuff(enemies, projectiles, specialDmg, specialKnock, specialDeflections, deflectionSpeed);
                if (Time.time - dashStart < dashTime)
                {
                    player.GetComponent<PlayerControl>().move(dashSpeed, weaponCont.transform.up);
                }
                else {
                    localState = "idle";
                    transform.parent = null;
                    lastDash = Time.time;
                }
            }
            else
            {
                //Reset behavior for idle
                if (occupied)
                {
                    transform.position = player.transform.position + new Vector3(idleOffset.x, idleOffset.y, 0);
                    transform.up = Camera.main.transform.up;
                    occupied = false;
                    player.GetComponent<PlayerControl>().hasControl = true;
                    weaponCont.GetComponent<weaponHandler>().weaponState = "idle";
                }
                //I like immediate feedback in the controls, but turning animations could go here
                else
                {

                }
            }

            if (Time.time - lastDash > specialCD) glow();
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

    void IWeapon.specialAttack()
    {
        if ((!occupied) && (Time.time - lastDash > specialCD))
        {
            dashStart = Time.time;
            player.GetComponent<SpriteRenderer>().color = Color.white;
            occupied = true;
            transform.position = player.transform.position + new Vector3(specialOffset.x, specialOffset.y, 0);
            localState = "special";
            //player.GetComponent<PlayerControl>().hasControl = false;
            transform.parent = weaponCont.transform;
        }
    }

    bool IWeapon.requestSpecial()
    {
        if ((!occupied) && (Time.time - lastDash > specialCD))
        {
            return true;
        }
        else {
            return false;
        }
    }

    void IWeapon.dropWeapon()
    {
        isHeld = false;
        GetComponent<Rigidbody2D>().velocity = weaponCont.transform.up * throwSpeed;
        GetComponent<Collider2D>().isTrigger = false;
    }

    void IWeapon.pickUp(GameObject owner)
    {
        isHeld = true;
        localState = "idle";

        player = owner.transform.parent;
        weaponCont = owner;
        GetComponent<Collider2D>().isTrigger = true;
        weaponCont.GetComponent<weaponHandler>().basicSwingSpeed = basicSwingSpeed;
        weaponCont.GetComponent<weaponHandler>().specialOffset = specialOffset;
        weaponCont.GetComponent<weaponHandler>().specialStart = specialStart;
    }

    void OnTriggerEnter2D (Collider2D coll) {
        if (coll.gameObject.tag == "Enemy") enemies.Add(coll.gameObject);
        if (coll.gameObject.tag == "Projectile") projectiles.Add(coll.gameObject);
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (localState == "idle")
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
        if (coll.tag == "Projectile") projectiles.Remove(coll.gameObject);

    }

}
