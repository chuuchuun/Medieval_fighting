using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject target;

    public float runningDistance = 10.0f;
    public float attackDistance = 2f;
    public float triggerDistance = 20.0f;

    public float walkingSpeed = 2f;
    public float _runningModificator = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;

    private int _speedID;
    private int _attackID;
    private int _hurtID;
    private int _deathID;

    private float _speed;
    private bool _isAttacking = false;

    // Stamina system variables
    public float stamina = 100f; // Initial stamina
    public float maxStamina = 100f; // Maximum stamina
    public float staminaRecoveryRate = 10f; // Stamina recovery per second
    public float attackStaminaCost = 20f; // Stamina cost per attack
    public float attackCooldown = 2f; // Cooldown time after an attack in seconds

    private bool canAttack = true; // Determines if the enemy can attack

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        _speedID = Animator.StringToHash("Speed");
        _attackID = Animator.StringToHash("Attack");
    }

    void Start()
    {
        // Initialize any needed setup here
    }

    private void HeadForDestination()
    {
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
            // Perform attack
            stamina -= attackStaminaCost; // Deduct stamina
            animator.SetFloat(_speedID, 0f);
            _isAttacking = true;
            canAttack = false; // Set cooldown for next attack
            StartCoroutine(AttackCooldown());
        }
        else
        {
            // Not enough stamina to attack
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
        HeadForDestination();
        animator.SetBool(_attackID, _isAttacking);

        // Recover stamina over time
        if (stamina < maxStamina)
        {
            stamina += staminaRecoveryRate * Time.deltaTime;
        }

        // Ensure stamina does not exceed maxStamina
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
    }
}
