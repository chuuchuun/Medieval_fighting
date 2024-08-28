using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon 
{
    public WeaponType type;

    public class WeaponType
    {
        public string name;
        public int damage;
    }
}
