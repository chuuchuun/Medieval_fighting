using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject swordPrefab; 

    [SerializeField] private Inputs _inputs;
    [SerializeField] private float _speed;

    [SerializeField] private float _walkingSpeed = 100f;

    [SerializeField] private float _runModificator = 1.5f;
    [SerializeField] private float _crouchModificator = 0.75f;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _mouseSens;

    [SerializeField] private Transform groundCheckTransform; // Reference to ground check position


    private Rigidbody _rb;
    private Camera _camera;
    private Animator _animator;

    private float _xRotation = 0;
    private float _yRotation = 0;

    private bool _isCrouching = false;
    private bool _isRunning = false;
    private bool _isJumping = false;

    private bool _isGrounded = true;

    private string _speedID = "Speed";
    private string _strongAttackID = "StrongAttack";
    private int _attackID;
    private string _crouchID = "Crouch";
    private string _runID = "Run";
    private string _jumpID = "Jump";

    public float groundCheckRadius = 0.2f; // Radius of the ground check collider
    public LayerMask groundLayer;
    public float jumpDelay = 1.0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();

        _attackID = Animator.StringToHash("Attack");


        _inputs.strongAttackEvent.AddListener(OnStrongAttack);
        _inputs.attackEvent.AddListener(OnAttack);

        _inputs.jumpEvent.AddListener(OnJump);
        _inputs.crouchEvent.AddListener(OnCrouch);


        _inputs.startRunEvent.AddListener(OnRunStart);
        _inputs.stopRunEvent.AddListener(OnRunStop);

        _speed = _walkingSpeed;


    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        OnLook();
        //OnRun();

        if (_animator.GetBool(_strongAttackID) && !_inputs.strongAttack)
        {
            _animator.SetBool(_strongAttackID, false);
        }

        if (_animator.GetBool(_attackID) && !_inputs.attack)
        {
            _animator.SetBool(_attackID, false);
        }

        CheckGround();
        
        _animator.SetBool(_jumpID, _isJumping);
        //print(_animator.GetBool(_jumpID));
    }

    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundLayer);

        // Debug to visualize the ground check
        Debug.DrawRay(groundCheckTransform.position, Vector3.down * groundCheckRadius, Color.red);


    }

private void OnMove()
    {

        _rb.AddRelativeForce(new Vector3(_inputs.move.x, 0, _inputs.move.y) * _speed * Time.deltaTime);
        _animator.SetFloat(_speedID, _inputs.move.magnitude);
    }
    private void OnLook()
    {
        _xRotation -= _inputs.look.y;
        _xRotation = Mathf.Clamp(_xRotation, -60f, 30f);

        _yRotation += _inputs.look.x;

        //_camera.transform.rotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }



    // Coroutine to handle the jump with a delay
    private void OnJump()
    {
        if (!_isJumping && _isGrounded)
        {
            StartCoroutine(JumpWithDelay());
        }
    }

    // Coroutine to handle the jump with a delay
    private IEnumerator JumpWithDelay()
    {
        
        _animator.SetBool(_jumpID, true); // Start the jump animation while on the ground
        _isJumping = true;
        print("jump started");

        yield return new WaitForSeconds(0.5f);


        print("applying force");
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse); // Apply the jump force after the animation starts

        // You can reset the animator boolean after jumping or let the animation handle it
        yield return new WaitForSeconds(0.1f); // Optional: Wait briefly before resetting
        print("jump ended");
        _animator.SetBool(_jumpID, false);
        _isJumping = false;
    }

    private void OnAttack()
    {
        swordPrefab.SetActive(true);

        // Determine the current movement state
        if (_isRunning)
        {
            // Set the animator for a running attack animation
            _animator.SetBool(_attackID, true);
            _animator.SetBool(_runID, true);
            _animator.SetFloat(_speedID, _inputs.move.magnitude); // Set speed to a higher value to trigger running animations
            Debug.Log("Running Attack");
        }
        else if (_inputs.move.magnitude > 0) // Check if player is walking
        {
            // Set the animator for a walking attack animation
            _animator.SetBool(_attackID, true);
            _animator.SetBool(_runID, false);
            _animator.SetFloat(_speedID, _inputs.move.magnitude); // Set speed to a lower value to trigger walking animations
            Debug.Log("Walking Attack");
        }
        else
        {
            // Set the animator for an idle attack animation
            _animator.SetBool(_attackID, true);
            _animator.SetBool(_runID, false);
            _animator.SetFloat(_speedID, 0); // Set speed to 0 to trigger idle animations
            Debug.Log("Idle Attack");
        }

        Debug.Log("Attack performed");
    }

    private void OnStrongAttack()
    {
        swordPrefab.SetActive(true);

        if (_isRunning)
        {
            // Set the animator for a running strong attack animation
            _animator.SetBool(_strongAttackID, true);
            _animator.SetBool(_runID, true);
            _animator.SetFloat(_speedID, _inputs.move.magnitude); // Set speed to a higher value to trigger running animations
            Debug.Log("Running Strong Attack");
        }
        else if (_inputs.move.magnitude > 0) // Check if player is walking
        {
            // Set the animator for a walking strong attack animation
            _animator.SetBool(_strongAttackID, true);
            _animator.SetBool(_runID, false);
            _animator.SetFloat(_speedID, _inputs.move.magnitude); // Set speed to a lower value to trigger walking animations
            Debug.Log("Walking Strong Attack");
        }
        else
        {
            // Set the animator for an idle strong attack animation
            _animator.SetBool(_strongAttackID, true);
            _animator.SetBool(_runID, false);
            _animator.SetFloat(_speedID, 0); // Set speed to 0 to trigger idle animations
            Debug.Log("Idle Strong Attack");
        }

        Debug.Log("Strong attack performed");
    }

    private void OnCrouch()
    {
        if (!_isCrouching)
        {
            _isCrouching = true;
            _speed = _walkingSpeed * _crouchModificator;
            print("crouching");
        }
        else
        {
            _isCrouching = false;
            _speed = _walkingSpeed;
            print("standing up");
        }
        _animator.SetBool(_crouchID, _isCrouching);
    }
    private void OnRun()
    {
        if (!_isCrouching)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                print("running");
                _speed = _walkingSpeed * _runModificator;
                _isRunning = true;
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift)) {
                print("walking again");
                _speed = _walkingSpeed;
                _isRunning = false;
            }
        }
        _animator.SetBool(_runID, _isRunning);
    }

    private void OnRunStart()
    {
        if (!_isCrouching)
        {
            print("running");
            _speed = _walkingSpeed * _runModificator;
            _isRunning = true;

        }
        _animator.SetBool(_runID, _isRunning);
    }

    private void OnRunStop()
    {
        print("walking again");
        _speed = _walkingSpeed;
        _isRunning = false;
        _animator.SetBool(_runID, _isRunning);
    }
}
