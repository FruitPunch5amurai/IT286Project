using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spear : MonoBehaviour, IWeapon
{
    //Weapon Pickup Variables
    bool isHeld = false;
    public float throwSpeed = 25.0f;
    public GameObject weaponCont;
    float pickUpTime;

    //Idle Behavior
    private string localState = "idle";
    public string state {
        get {
            return localState;
        }
        set {
            localState = value;
        }
    }
    public Vector2 idleOffset;
    bool occupied = false;

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
    public float _SwingDelay = 1.0f;
    public float swingDelay {
        get {
            return _SwingDelay;
        }
    }
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
    private float lastSpecial;
    public float specialSpeed = 2.0f;
    public float specialCD = 4.0f;
    public float specialDmg;
    public float specialKnock;
    Color[] specialDeflections = new Color[1] { Color.blue };
    public Vector3 scale;
    Vector2 boxOffset;
    Vector2 boxSize;
    bool impact;
    Transform player;
    //Glow Effect
    Color targetColor = Color.yellow;
    Color curColor;
    public float glowSpeed = 0.005f;
    float colorThreshold = 0.1f;

    // Use this for initialization
    void Start()
    {
        lastSpecial = Time.time - specialCD;
        curColor = targetColor;

        scale = transform.localScale;
        boxOffset = GetComponent<BoxCollider2D>().offset;
        boxSize = GetComponent<BoxCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
        {
            
            if (localState == "pickup")
            {
                //Had some bugs in regards to scale when the weapon drops were scaled differently than the animations
                //This is a messy workaround
                if (pickUpTime != Time.time)
                {
                    occupied = true;
                    scale = transform.lossyScale;
                    state = "basic";
                    GetComponent<Animation>().Stop();
                    transform.parent = null;
                    weaponCont.GetComponent<weaponHandler>().basicAttack();
                }
            }
            else if (localState == "basic")
            {
                //do this
                GetComponent<Animation>().Stop();
                weaponCont.GetComponent<weaponHandler>().hitStuff(enemies, projectiles, basicDmg, basicKnock, basicDeflections, deflectionSpeed);
            }
            else if (localState == "special")
            {
                //do this
                if (GetComponent<Animation>().isPlaying)
                {
                    player.GetComponent<PlayerControl>().move(specialSpeed * player.GetComponent<PlayerControl>().moveSpeed, weaponCont.transform.up);

                    if (impact)
                    {
                        weaponCont.GetComponent<weaponHandler>().hitStuff(enemies, projectiles, specialDmg, specialKnock, specialDeflections, deflectionSpeed);
                    }
                }
                else {
                    localState = "idle";
                    transform.parent = null;
                    lastSpecial = Time.time;
                    GetComponent<BoxCollider2D>().offset = boxOffset;
                    GetComponent<BoxCollider2D>().size = boxSize;
                    weaponCont.GetComponent<weaponHandler>().weaponState = "idle";
                    player.GetComponent<PlayerControl>().invincible = false;
                    if (player.GetComponent<PlayerControl>().orientation == "e")
                    {
                        weaponCont.transform.localScale = new Vector3(weaponCont.transform.localScale.x * -1, weaponCont.transform.localScale.y, weaponCont.transform.localScale.z);
                    }
                    player.GetComponent<PlayerControl>().hasControl = true;
                }
            }
            else
            {
                //Reset behavior for idle
                if (occupied)
                {
                    transform.parent = null;
                    transform.position = player.transform.position + new Vector3(idleOffset.x, idleOffset.y, 0);
                    transform.up = Camera.main.transform.up;
                    occupied = false;
                    player.GetComponent<PlayerControl>().hasControl = true;
                    weaponCont.GetComponent<weaponHandler>().weaponState = "idle";
                    transform.localScale = scale;
                }
                //I like immediate feedback in the controls, but turning animations could go here
                else
                {

                }
            }

            if (Time.time - lastSpecial > specialCD) glow();
        }
    }

    void specialImpact() {
        impact = true;
    }
    void specialRelease() {
        impact = false;
    }

    void glow()
    {
        if (Mathf.Abs(player.GetComponent<SpriteRenderer>().color.b - curColor.b) < colorThreshold)
        {
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
        player.GetComponent<SpriteRenderer>().color = Color.white;
        occupied = true;
        localState = "special";
        //player.GetComponent<PlayerControl>().hasControl = false;
        transform.parent = weaponCont.transform;
        GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.13f);
        GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 0.2f);
        if ((player.GetComponent<PlayerControl>().orientation == "w") || (player.GetComponent<PlayerControl>().orientation == "e"))
        {
            if (player.GetComponent<PlayerControl>().orientation == "e")
            {
                weaponCont.transform.localScale = new Vector3(weaponCont.transform.localScale.x * -1, weaponCont.transform.localScale.y, weaponCont.transform.localScale.z);
            }
            GetComponent<Animation>().Play("SpearHorizontal");
        }
        else {
            GetComponent<Animation>().Play("SpearAttack");
        }
        player.GetComponent<PlayerControl>().invincible = true;
        player.GetComponent<PlayerControl>().hasControl = false;
    }
    bool IWeapon.requestSpecial() {
        if ((!occupied) && (Time.time - lastSpecial > specialCD))
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
        transform.parent = null;
        state = "idle";
    }

    void IWeapon.pickUp(GameObject owner)
    {
        isHeld = true;
        localState = "pickup";
        pickUpTime = Time.time;

        player = owner.transform.parent;
        weaponCont = owner;
        GetComponent<Collider2D>().isTrigger = true;
        weaponCont.GetComponent<weaponHandler>().basicSwingSpeed = basicSwingSpeed;
        weaponCont.GetComponent<weaponHandler>().specialOffset = specialOffset;
        weaponCont.GetComponent<weaponHandler>().specialStart = specialStart;

        transform.parent = weaponCont.transform;
        GetComponent<Animation>().Play("SpearAttack");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy") enemies.Add(coll.gameObject);
        if (coll.gameObject.tag == "Projectile")
        {
            if (coll.GetComponent<SpriteRenderer>().color != Color.yellow)
            {
                projectiles.Add(coll.gameObject);
            }
        }

    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            if (!enemies.Contains(coll.gameObject)) enemies.Add(coll.gameObject);
        }
        if (coll.gameObject.tag == "Projectile")
        {
            if ((!projectiles.Contains(coll.gameObject)) && (coll.GetComponent<SpriteRenderer>().color != Color.yellow)) projectiles.Add(coll.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Enemy") enemies.Remove(coll.gameObject);
        if (coll.tag == "Projectile") projectiles.Remove(coll.gameObject);

    }

}
