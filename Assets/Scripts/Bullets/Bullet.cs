using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


    public enum BulletTypes
    {
        Light,
        Heavy
    }
    public BulletTypes BulletType;

    public bool IsDeflectable;
    public bool Deflected;
}
