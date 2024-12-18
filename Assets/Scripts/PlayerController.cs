using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed, _jumpSpeed;

    [SerializeField]
    private LayerMask _ground;

    private PlayerActionControls _playerActionControls;

    private Rigidbody2D _rigidbody;

    private Collider2D _collider;

    private void Awake()
    {
        _playerActionControls = new PlayerActionControls();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _playerActionControls.Enable();
    }

    private void OnDisable()
    {
        _playerActionControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerActionControls.Ground.Jump.performed += _ => Jump();
    }

    // Update is called once per frame
    void Update()
    {
       float movementInput = _playerActionControls.Ground.Move.ReadValue<float>();
        //Character movement
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * _speed * Time.deltaTime;
        transform.position = currentPosition;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            _rigidbody.AddForce(new Vector2(0, _jumpSpeed), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= _collider.bounds.extents.x;
        topLeftPoint.y += _collider.bounds.extents.y;

        Vector2 bottomRightPoint = transform.position;
        bottomRightPoint.x += _collider.bounds.extents.x;
        bottomRightPoint.y -= _collider.bounds.extents.y; 


        return Physics2D.OverlapArea(topLeftPoint, bottomRightPoint, _ground);
    }
}
