using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class weaponHandler : MonoBehaviour {
    bool hasWeapon;
    public List<Transform> weapons = new List<Transform>();

	// Use this for initialization
	void Start () {
        if (transform.childCount == 0) hasWeapon = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (hasWeapon)
        {
            if (transform.parent.GetComponent<playerController>().hasControl)
            {
                if (Input.GetButtonDown("BasicAttack"))
                {
                    pickUpWeapon();
                    basicAttack();
                }
                if (Input.GetButtonDown("SpecialAttack")) specialAttack();
            }
        }
        else if (Input.GetButtonDown("BasicAttack")) {
            pickUpWeapon();
            if (transform.childCount > 0)
            {
                basicAttack();
            }
        }
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
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), transform.GetChild(0).GetComponent<Collider2D>(), false);
            transform.GetChild(0).GetComponent<IWeapon>().dropWeapon();
            Debug.Log("Drop called");
        }
        else {
            hasWeapon = true;
        }
        weapons[closestI].SetParent(transform);
        weapons[closestI].GetComponent<IWeapon>().pickUp();
        weapons[closestI].gameObject.layer = 8;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), weapons[closestI].GetComponent<Collider2D>(), true);
        weapons.Remove(weapons[closestI]);
    }

    void basicAttack() {
        transform.GetChild(0).GetComponent<IWeapon>().basicAttack();
    }

    void specialAttack() {
        transform.GetChild(0).GetComponent<IWeapon>().specialAttack();
    }

}
