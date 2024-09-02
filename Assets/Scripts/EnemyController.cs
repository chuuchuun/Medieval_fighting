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
    
    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        _speedID = Animator.StringToHash("Speed");
        _attackID = Animator.StringToHash("Attack");

    }
    void Start()
    {

    }

    private void HeadForDestination()
    {
        Vector3 destination = target.transform.position;
        float distance = Vector3.Distance(agent.transform.position, destination);
        _speed = walkingSpeed;
        if(distance < triggerDistance)
        {
            if (distance > runningDistance)
            {
                Run();
            }
            else if (distance <= attackDistance)
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
        animator.SetFloat(_speedID, 0f);
        _isAttacking = true;
    }
    // Update is called once per frame
    void Update()
    {
        HeadForDestination();
        animator.SetBool(_attackID, _isAttacking);

    }
}
