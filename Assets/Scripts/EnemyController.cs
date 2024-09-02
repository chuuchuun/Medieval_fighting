using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private float runningDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float triggerDistance;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float _runningModificator = 1.5f;

    [SerializeField] private int health;

    private NavMeshAgent agent;
    private Animator animator;

    private int _speedID;
    private int _attackID;
    private int _hurtID;
    private int _deathID;

    private float _speed;
    private bool _isAttacking = false;
    private bool _isAlive = true;

    private Rigidbody _rb;
    // Stamina system variables
    [SerializeField] private float stamina; // Initial stamina
    [SerializeField] private float maxStamina; // Maximum stamina
    [SerializeField] private float staminaRecoveryRate; // Stamina recovery per second
    [SerializeField] private float attackStaminaCost; // Stamina cost per attack
    [SerializeField] private float attackCooldown; // Cooldown time after an attack in seconds

    private bool canAttack = true; // Determines if the enemy can attack

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();

        _speedID = Animator.StringToHash("Speed");
        _attackID = Animator.StringToHash("Attack");
        _hurtID = Animator.StringToHash("Hurt");
        _deathID = Animator.StringToHash("Death");

        runningDistance = GetRunningDistance();
        attackDistance = GetAttackDistance();
        triggerDistance = GetTriggerDistance();
        walkingSpeed = GetWalkingSpeed();
        stamina = GetStamina();
        maxStamina = GetMaxStamina();
        staminaRecoveryRate = GetStaminaRecoveryRate();
        attackStaminaCost = GetStaminaCost();
        attackCooldown = GetAttackCooldown();

        health = GetHealth();
    }

    private void HeadForDestination()
    {
        if (!_isAlive) return;  // Prevents any further actions if dead

        Vector3 destination = target.transform.position;
        float distance = Vector3.Distance(agent.transform.position, destination);
        _speed = walkingSpeed;

        if (distance < triggerDistance)
        {
            if (distance > runningDistance)
            {
                Run();
            }
            else if (distance <= attackDistance && canAttack)
            {
                Attack();
            }
            else
            {
                agent.speed = _speed;
                animator.SetFloat(_speedID, 0.5f);
            }
            agent.SetDestination(destination);
        }
        else
        {
            animator.SetFloat(_speedID, 0f);
            _isAttacking = false;
        }
    }

    private void Run()
    {
        animator.SetFloat(_speedID, 1);
        _speed = walkingSpeed * _runningModificator;
        _isAttacking = false;
    }

    private void Attack()
    {
        if (stamina >= attackStaminaCost)
        {
            stamina -= attackStaminaCost;
            animator.SetFloat(_speedID, 0f);
            _isAttacking = true;
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
        else
        {
            _isAttacking = false;
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void Update()
    {
        if (_isAlive)
        {
            HeadForDestination();
            animator.SetBool(_attackID, _isAttacking);
            if (stamina < maxStamina)
            {
                stamina += staminaRecoveryRate * Time.deltaTime;
            }
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (!_isAlive) return; // Prevent further damage after death

        animator.SetTrigger(_hurtID);
        health -= damage;
        print("Enemy " + name + " received " + damage + " damage. Current health is " + health);
        if (health < 1)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!_isAlive) return; // Prevents Die() from being called multiple times

        _speed = 0;
        _isAlive = false;

        _rb.freezeRotation = true;
        // Disable NavMeshAgent to stop all movement and rotation
        agent.enabled = false;
        agent.updatePosition = false;
        agent.updateRotation = false;

        animator.SetBool(_deathID, true);
        print("Enemy died");
    }

    public abstract float GetRunningDistance();
    public abstract float GetAttackDistance();
    public abstract float GetTriggerDistance();
    public abstract float GetWalkingSpeed();
    public abstract float GetStamina();
    public abstract float GetMaxStamina();
    public abstract float GetStaminaRecoveryRate();
    public abstract float GetStaminaCost();
    public abstract float GetAttackCooldown();
    public abstract int GetHealth();
}
