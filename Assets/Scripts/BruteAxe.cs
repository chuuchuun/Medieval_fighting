using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteAxe : EnemyWeapon
{
    private float _damage = 10f;
    private string _name = "Brute Axe";

    public override float GetDamage()
    {
        return _damage;
    }

    public override string GetName()
    {
        return _name;
    }
}
