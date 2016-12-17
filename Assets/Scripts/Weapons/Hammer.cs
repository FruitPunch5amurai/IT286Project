using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hammer : MonoBehaviour, IWeapon
{
    //Weapon Pickup Variables
    bool isHeld = false;
    public float throwSpeed = 25.0f;
    


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
    float deflectionSpeed = 2.0f;
    public float DeflectionSpeed {
        get {
            return deflectionSpeed;
        }
    }


    //Basic Attack
    public float _SwingDelay = 2.0f;
    public float swingDelay {
        get {
            return _SwingDelay;
        }
    }
    public float basicDmg;
    public float BasicDamage {
        get {
            return basicDmg;
        }
    }
    public float basicKnock;
    public float BasicKnock {
        get {
            return basicKnock;
        }
    }
    public float basicSwingSpeed = 2.0f;
    Color[] basicDeflections = new Color[1] { Color.blue };
    public Color[] BasicDeflections {
        get {
            return basicDeflections;
        }
    }

    //Special Attack
    public Vector2 specialOffset;
    public Quaternion specialStart;
    Quaternion spinTarget;
    public float spinSpeed = 10.0f;
    public float numSpins = 2;
    float spinCounter = 0.0f;
    bool startedSpin;
    Quaternion prevRot;
    private float lastSpin;
    public float spinTime = 1.0f;
    public float spinCD = 3.0f;
    public float specialDmg;
    public float specialKnock;
    Color[] specialDeflections = new Color[1] { Color.blue };

    Transform player;
    GameObject weaponCont;
    //Glow Effect
    public Color targetColor = Color.yellow;
    private Color curColor;
    public float glowSpeed = 0.005f;
    public float colorThreshold = 0.1f;
    private float newSpinSpeed;
    private float finalSpinSpeed;

    // Use this for initialization
    void Start()
    {
        lastSpin = Time.time - spinCD;
        curColor = targetColor;
    }

    // Update is called once per frame
    void Update()
    {
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
                if ((Mathf.Abs(Quaternion.Angle(weaponCont.transform.rotation, specialStart)) > 45) || (startedSpin))
                {
                    if (!startedSpin)
                    {
                        finalSpinSpeed = newSpinSpeed;
                        newSpinSpeed = Quaternion.Angle(prevRot, weaponCont.transform.rotation) * 60;
                        weaponCont.transform.rotation = Quaternion.RotateTowards(specialStart, spinTarget, 45);
                        spinCounter = Mathf.Abs(Quaternion.Angle(weaponCont.transform.rotation, specialStart));
                        startedSpin = true;
                    }
                    if (spinCounter > 360 * numSpins - 1)
                    {
                        localState = "idle";
                        weaponCont.transform.rotation = specialStart;
                        transform.parent = null;
                        weaponCont.GetComponent<weaponHandler>().weaponState = "idle";
                    }
                    else if (spinCounter > 360 * numSpins - 45)
                    {
                        weaponCont.transform.rotation = Quaternion.Lerp(weaponCont.transform.rotation, specialStart, finalSpinSpeed);
                        spinCounter = 360 * numSpins - Mathf.Abs(Quaternion.Angle(weaponCont.transform.rotation, specialStart));
                    }
                    else
                    {
                        spinCounter += newSpinSpeed * Time.deltaTime;
                        weaponCont.transform.RotateAround(weaponCont.transform.position, transform.forward, -newSpinSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    newSpinSpeed = spinSpeed + 0.1f * (Time.time - lastSpin);
                    prevRot = weaponCont.transform.rotation;
                    weaponCont.transform.rotation = Quaternion.Lerp(weaponCont.transform.rotation, spinTarget, newSpinSpeed);
                }

            }
            else
            {
                //Reset behavior for idle
                if (occupied)
                {
                    transform.up = Camera.main.transform.up;
                    occupied = false;
                    player.GetComponent<PlayerControl>().hasControl = true;
                }
                //I like immediate feedback in the controls, but turning animations could go here
                else
                {

                }
            }

            if (Time.time - lastSpin > spinCD) glow();
        }
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
        if ((!occupied) && (Time.time - lastSpin > spinCD))
        {
            transform.parent = weaponCont.transform;
            lastSpin = Time.time;
            spinCounter = 0.0f;
            player.GetComponent<SpriteRenderer>().color = Color.white;
            occupied = true;
            transform.localPosition = specialOffset;
            specialStart = weaponCont.transform.rotation;
            localState = "special";
            player.GetComponent<PlayerControl>().hasControl = false;
            spinTarget = Quaternion.LookRotation(weaponCont.transform.forward, weaponCont.transform.right);
            startedSpin = false;
        }
    }

    bool IWeapon.requestSpecial() {
        if ((!occupied) && (Time.time - lastSpin > spinCD))
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

        weaponCont = owner;
        player = weaponCont.transform.parent;
        GetComponent<Collider2D>().isTrigger = true;
        weaponCont.GetComponent<weaponHandler>().basicSwingSpeed = basicSwingSpeed;
        weaponCont.GetComponent<weaponHandler>().specialOffset = specialOffset;
        weaponCont.GetComponent<weaponHandler>().specialStart = weaponCont.transform.up;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy") enemies.Add(coll.gameObject);
        if (coll.gameObject.tag == "Projectile") projectiles.Add(coll.gameObject);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
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

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Enemy") enemies.Remove(coll.gameObject);
        if (coll.tag == "Projectile") projectiles.Remove(coll.gameObject);

    }

}
