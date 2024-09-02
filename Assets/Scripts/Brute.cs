using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brute : EnemyController
{
    private float _runningDistance = 10.0f;
    private float _attackDistance = 2f;
    private float _triggerDistance = 20.0f;
    private float _walkingSpeed = 2f;

    private float _stamina = 100f;
    private float _maxStamina = 100f;
    private float _staminaRecoveryRate = 10f;
    private float _attackStaminaCost = 20f;
    private float _attackCooldown = 2f;

    private int _health = 10; 

    override public float GetRunningDistance()
    {
        return _runningDistance;
    }
    override public float GetAttackDistance()
    {
        return _attackDistance;
    }
    override public float GetTriggerDistance()
    {
        return _triggerDistance;
    }

    override public float GetWalkingSpeed()
    {
        return _walkingSpeed;
    }
    override public float GetStamina()
    {
        return _stamina;
    }
    override public float GetMaxStamina()
    {
        return _maxStamina;
    }
    override public float GetStaminaRecoveryRate()
    {
        return _staminaRecoveryRate;
    }
    override public float GetStaminaCost()
    {
        return _attackStaminaCost;
    }
    override public float GetAttackCooldown()
    {
        return _attackCooldown;
    }
    public override int GetHealth()
    {
        return _health;
    }
}
