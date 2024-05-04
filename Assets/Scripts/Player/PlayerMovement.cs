using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Vector2 _movement;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //I set the _movement Vector to be the value of the input manager and because it's already normalized on the InputSystem
        //the movement speed will not break going diagonally
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        //Because I'm setting the velocity directly I don't need to multiply the speed by Time.deltatime or putting on FixedUpdate
        _rb.velocity = _movement * _moveSpeed;
    }
}
