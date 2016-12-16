using UnityEngine;
using System.Collections;
using System;

public interface IWeapon {
    string state
    {
        get;
        set;
    }
    float swingDelay {
        get;
    }
    bool requestSpecial();
    void specialAttack();
    void dropWeapon();
    void pickUp(GameObject weaponCont);
}
