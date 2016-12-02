using UnityEngine;
using System.Collections;

public interface IWeapon {
    void basicAttack();
    void specialAttack();
    void dropWeapon();
    void pickUp();
}
