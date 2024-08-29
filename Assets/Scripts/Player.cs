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

    private Rigidbody _rb;
    private Camera _camera;
    private Animator _animator;

    private float _xRotation = 0;
    private float _yRotation = 0;
    private bool _isCrouching = false;
    private bool _isRunning = false;

    private string _speedID = "Speed";
    private string _strongAttackID = "StrongAttack";
    private string _crouchID = "Crouch";
    private string _runID = "Run";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();

        _inputs.attackEvent.AddListener(OnAttack);
        _inputs.jumpEvent.AddListener(OnJump);
        _inputs.crouchEvent.AddListener(OnCrouch);

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
        OnRun();

        if (_animator.GetBool(_strongAttackID) && !_inputs.attack)
        {
            _animator.SetBool(_strongAttackID, false);
        }
    }

    private void OnMove()
    {

        _rb.AddRelativeForce(new Vector3(_inputs.move.x, 0, _inputs.move.y) * _speed * Time.deltaTime);
        print(_inputs.move.x);
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

    private void OnJump()
    {
        _isCrouching = false;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void OnAttack()
    {
        swordPrefab.SetActive(true);
        _animator.SetBool(_strongAttackID, true);
        print("shoot");
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
}
