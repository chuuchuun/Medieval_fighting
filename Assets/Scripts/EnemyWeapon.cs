using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    // Use protected fields to allow access in subclasses but not outside
    protected float damage;
    protected string weaponName;

    // Initialize the fields in the constructor or use Unity's Awake/Start methods
    protected virtual void Awake()
    {
        damage = GetDamage();
        weaponName = GetName();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Hit by {damage} with damage {weaponName}");
            other.gameObject.GetComponent<Player>().ReceiveDamage(damage, weaponName);
        }
    }
    // Abstract methods to enforce implementation in subclasses
    public abstract float GetDamage();
    public abstract string GetName();
}
