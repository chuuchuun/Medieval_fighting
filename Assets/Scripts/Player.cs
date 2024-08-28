using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject swordPrefab; 

    [SerializeField] private Inputs _inputs;
    [SerializeField] private float _speed = 100;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _mouseSens;

    private Rigidbody _rb;
    private Camera _camera;
    private Animator _animator;

    private float _xRotation = 0;
    private float _yRotation = 0;
    private bool _isCrouching = false;

    private string _speedID = "Speed";
    private string _strongAttackID = "StrongAttack";
    private string _crouchID = "Crouch";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();

        _inputs.attackEvent.AddListener(OnAttack);
        _inputs.jumpEvent.AddListener(OnJump);
        _inputs.crouchEvent.AddListener(OnCrouch);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        OnLook();

        if (_animator.GetBool(_strongAttackID) && !_inputs.attack)
        {
            _animator.SetBool(_strongAttackID, false);
        }
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

    private void OnJump()
    {
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
            _speed *= 0.75f;
            print("crouching");
        }
        else
        {
            _isCrouching = false;
            _speed /= 0.75f;
            print("standing up");
        }
        _animator.SetBool(_crouchID, _isCrouching);
    }
}
