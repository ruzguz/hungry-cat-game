﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public LayerMask groundLayerMask, WallLayerMask;
    [SerializeField]
    float _floorDetectionLine = 0.5f;

    //-Force es la fuerza para mover obstaculos
    public int force;
    //M_Wall indica que se esta moviendo por la pared
    //M_Ground indica que se esta moviendo por el piso
    //Si Wall es = 1 entonces Ground = 0 y viceversa
    int M_Wall=0,M_Ground=1;
  
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        _playerRB = this.GetComponent<Rigidbody2D>();
        _playerSR = this.GetComponent<SpriteRenderer>();
        _headCollider = this.GetComponent<CircleCollider2D>();
        _bodyCollider = this.GetComponent<BoxCollider2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
        // If player press running buttons 
        if (Input.GetAxis("Horizontal") != 0)
        {
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
        bool dir = (axis < 0) ? true : false;

        // Fliping sprite according to player direction
        _playerSR.flipX = dir;

        // Moving player
        //-Si el jugador esta en el piso entonces M_Wall es 0 y M_Ground es 1, entonces la funcion quedara tal que asi  
        //      -_playerRB.velocity = new Vector3((axis * _runningSpeed),(_playerRB.velocity.y)); moviendose en eje x (caminando)

        //-Si el jugado esta en la pared entonces M_Wall es 1 y M_Ground es 0, entonces la funcion quedara tal que asi 
        //      -_playerRB.velocity = new Vector3(0, (axis * _runningSpeed)); //moviendose en eje y (escalando)

        _playerRB.velocity = new Vector3((axis * _runningSpeed*M_Ground), (axis * _runningSpeed * M_Wall) + (_playerRB.velocity.y * M_Ground));

    }

    // Control the player jump
    void Jump()
    {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, _floorDetectionLine, groundLayerMask)) {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        if (IsTouchingTheWall())
        {
            _playerRB.AddForce(Vector2.left * _jumpForce*100, ForceMode2D.Impulse);
        }
    }
    // Check if player is touching the ground
    bool IsTouchingTheGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,
                                             Vector2.down, _floorDetectionLine, groundLayerMask);
        return hit;
    }
    // Check if player is touching the wall
    bool IsTouchingTheWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,
                                             Vector2.left, _floorDetectionLine, WallLayerMask);
        if (hit==false)
        hit = Physics2D.Raycast(this.transform.position,
                                             Vector2.right, _floorDetectionLine, WallLayerMask);
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
        if (other.gameObject.CompareTag("Food"))
        {
            FoodController food = other.GetComponent<FoodController>();
            food.Hide();
            this.TransformCat(food.foodType);
        }
    }

    //-Detectar si el gato colisiona con una pared,es importante determinar porque lado esta colisionando el gato,
    //-si colisiona del lado izquierda entonces debe girar -90 grados, si es del lado derecho entonces gira 90 grados
    //-si las paredes tendran distintos angulos a 90 grados entonces debe hacerse una formula mas complicada
    //-Una vez en el muro en el movimiento del gato no debe ser en el eje X sino en en eje Y.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
           climb(true);
        }
        else
        if (collision.gameObject.CompareTag("Ground"))
        {
            climb(false);
        }

    }

    
    void climb(bool climbing)
    {
        if (climbing == true)
        {
            //-M_Wall se multiplica por el Axis para que varie entre 1 y -1 para que los movimientos del gato coincidan con los sprite
            M_Ground = 0; M_Wall = 1* Convert.ToInt32(Input.GetAxis("Horizontal")); 
            transform.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
            //-si la colision es con un muro, gira 90 grados por 1(si es a la derecha) o -1(si es a la izquierda) en el eje Z
            transform.rotation = Quaternion.AngleAxis((90 * Convert.ToInt32(Input.GetAxis("Horizontal"))), new Vector3(0, 0, 1));
            transform.position = new Vector2((0.5f*Convert.ToInt32(Input.GetAxis("Horizontal"))) +gameObject.transform.position.x, 0 + gameObject.transform.position.y);
        }
        else
        {
            M_Ground = 1; M_Wall = 0;
            transform.GetComponent<Rigidbody2D>().gravityScale = 1;
            transform.rotation = Quaternion.AngleAxis((0 * Convert.ToInt32(Input.GetAxis("Horizontal"))), new Vector3(0, 0, 1));
        }
    }
}
