﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CatColliderValues {
    public float bodyOffsetX;
    public float bodyOffsetY;
    public float bodyWidth;
    public float bodyHeight;
    public float headX;
    public float headY;
    public float headRadius;

    public CatColliderValues(float bX, float bY, float bW, float bH, float hX, float hY, float hR){
        bodyOffsetX = bX;
        bodyOffsetY = bY;
        bodyWidth = bW;
        bodyHeight = bH;
        headX = hX;
        headY = hY;
        headRadius = hR;
    }

    public CatColliderValues InvertedPosition(){
        return new CatColliderValues(-bodyOffsetX, bodyOffsetY, bodyWidth,
                                    bodyHeight, -headX, headY, headRadius);
    }
}

public class PlayerController : MonoBehaviour
{
    // Components vars
    Rigidbody2D _playerRB;
    SpriteRenderer _playerSR;
    CircleCollider2D _headCollider;
    BoxCollider2D _bodyCollider;

    // Player vars
    [SerializeField]
    float _runningSpeed = 8f;
    [SerializeField]
    float _jumpForce = 20f;

    // Other vars
    public LayerMask groundLayerMask;
    [SerializeField]
    float _floorDetectionLine = 0.7f;
    public Sprite normalCatSprite, fitCatSprite, fatCatSprite;

    // Vars for collider sizes for each cat type 
    CatColliderValues _catCV = new CatColliderValues(0.25f, -0.35f, 1.08f, 0.52f, 0.41f, 0.22f, 0.34f);
    CatColliderValues _fitCatCV = new CatColliderValues(0, -0.4f, 1f, 0.8f, 0, 0.3f, 0.34f);
    CatColliderValues _fatCatCV = new CatColliderValues(0.06f, -0.32f, 1.14f, 0.85f, 0.25f, 0.31f, 0.34f); 


    // Awake is called when the script instance is being loaded.
    void Awake() 
    {
        _playerRB = this.GetComponent<Rigidbody2D>();
        _playerSR = this.GetComponent<SpriteRenderer>();
        _headCollider = this.GetComponent<CircleCollider2D>();
        _bodyCollider = this.GetComponent<BoxCollider2D>();
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

        if (Input.GetButtonDown("Jump")) {
            Jump();
        }

        // If player press running buttons 
        if (Input.GetAxis("Horizontal") != 0) {
            Run();
        } else {
            _playerRB.velocity = new Vector3(0, _playerRB.velocity.y);
        }

        Debug.DrawRay(this.transform.position, Vector3.down * _floorDetectionLine, Color.red);
    }

    // Control the player movement
    void Run()
    {
        // getting axis and dir
        float axis = Input.GetAxis("Horizontal");
        bool dir = (axis < 0)?true:false;

        // Changing Collider position according to player direction
        if (_playerSR.flipX != dir) {
            foreach (Collider2D c in GetComponents<Collider2D>()) {
                c.offset = new Vector2(c.offset.x * -1, c.offset.y);
            }
        }

        // Fliping sprite according to player direction
        _playerSR.flipX = dir;


        // Moving player
        _playerRB.velocity = new Vector3(axis * _runningSpeed, _playerRB.velocity.y);
    }
    
    // Control the player jump
    void Jump()
    {
        if(IsTouchingTheGround()) {
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
                _playerSR.sprite = fitCatSprite;  
                ResizeCatColliders((_playerSR.flipX)?_fitCatCV.InvertedPosition():_fitCatCV);  
            break;
            case FoodType.junkFood:
                _playerSR.sprite = fatCatSprite;
                ResizeCatColliders((_playerSR.flipX)?_fatCatCV.InvertedPosition():_fatCatCV);  
            break;
            case FoodType.catFood:
                _playerSR.sprite = normalCatSprite;
                ResizeCatColliders((_playerSR.flipX)?_catCV.InvertedPosition():_catCV);  
            break;
        }
    }

    // Function to set player collider 
    void ResizeCatColliders(CatColliderValues c)
    {
        _bodyCollider.offset = new Vector2(c.bodyOffsetX, c.bodyOffsetY);
        _bodyCollider.size = new Vector2(c.bodyWidth, c.bodyHeight);
        _headCollider.offset = new Vector2(c.headX, c.headY);
        _headCollider.radius = c.headRadius;
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
