﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Prueba
public class PlayerController : MonoBehaviour
{
    // Components vars
    Rigidbody2D _playerRB;
    SpriteRenderer _playerSR;
    CircleCollider2D _headCollider;
    BoxCollider2D _bodyCollider;
    Animator _playerAnimator;

    // Player vars
    [SerializeField]
    float _runningSpeed = 8f;
    [SerializeField]
    float _jumpForce = 20f;

    // Other vars
    public LayerMask groundLayerMask;
    [SerializeField]
    float _floorDetectionLine = 0.5f;

    // Awake is called when the script instance is being loaded.
    void Awake() 
    {
        _playerRB = this.GetComponent<Rigidbody2D>();
        _playerSR = this.GetComponent<SpriteRenderer>();
        _headCollider = this.GetComponent<CircleCollider2D>();
        _bodyCollider = this.GetComponent<BoxCollider2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate() 
    {
        // Jump action
        if (Input.GetButton("Jump")) {
            Jump();
        }

        // If player press running buttons 
        if (Input.GetAxis("Horizontal") != 0) {
            Run();
            _playerAnimator.SetBool("isRunning", true);
        } else {
            _playerRB.velocity = new Vector3(0, _playerRB.velocity.y);
            _playerAnimator.SetBool("isRunning", false);
        }

        // Check if player is touching the ground
            _playerAnimator.SetBool("isTouchingTheGround", IsTouchingTheGround());


        Debug.DrawRay(this.transform.position, Vector3.down * _floorDetectionLine, Color.red);
    }

    // Control the player movement
    void Run()
    {
        // getting axis and dir
        float axis = Input.GetAxis("Horizontal");
        bool dir = (axis < 0)?true:false;

        // Fliping sprite according to player direction
        _playerSR.flipX = dir;


        // Moving player
        _playerRB.velocity = new Vector3(axis * _runningSpeed, _playerRB.velocity.y);
    }
    
    // Control the player jump
    void Jump()
    {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, _floorDetectionLine, groundLayerMask)) {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    // Check if player is touching the ground
    bool IsTouchingTheGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, 
                                             Vector2.down, _floorDetectionLine, groundLayerMask);

        return hit;
    }

    // Transform the cat according to the food type eaten
    public void TransformCat(FoodType food)
    {
        switch(food){
            case FoodType.healthyFood:  
                _jumpForce = 22f;
                _runningSpeed = 12f;
                _playerAnimator.SetTrigger("fitCat");
            break;
            case FoodType.junkFood:
                _jumpForce = 15f;
                _runningSpeed = 4f;
                _playerAnimator.SetTrigger("fatCat");
            break;
            case FoodType.catFood:
                _jumpForce = 20f;
                _runningSpeed = 8f;
                _playerAnimator.SetTrigger("normalCat");
            break;
        }
    }

    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    void OnTriggerEnter2D(Collider2D other)
    {
        // Detect if player collide with food to eat it 
        if (other.gameObject.CompareTag("Food")){
            FoodController food = other.GetComponent<FoodController>();
            food.Hide();
            this.TransformCat(food.foodType);
        }
    }
}
