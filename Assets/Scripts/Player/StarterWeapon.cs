using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterWeapon : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        if (MenuControl.StarterWeapon != null)
        {
            GameObject starterWeapon = GameObject.Instantiate(MenuControl.StarterWeapon);
            starterWeapon.transform.position = transform.position;
            GetComponent<PlayerControl>().WeaponController.GetComponent<weaponHandler>().pickUpWeapon(starterWeapon);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
