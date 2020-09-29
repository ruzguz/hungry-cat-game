using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenPlayerController : MonoBehaviour
{
 // Components vars
    Rigidbody2D _playerRB;
    SpriteRenderer _playerSR;
    Animator _playerAnimator;
    Collider2D _playerCollider;

    // Player vars
    [SerializeField]
    float _runningSpeed = 8f;
    [SerializeField]
    float _jumpForce = 20f;
    [SerializeField]
    bool _isBurned = false;

    // Other vars
    public LayerMask groundLayerMask, WallLayerMask, platformLayerMask;
    [SerializeField]
    float _floorDetectionLine = 0.5f;
  
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        _playerRB = this.GetComponent<Rigidbody2D>();
        _playerSR = this.GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _playerCollider = GetComponent<Collider2D>();
    } 

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
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
        // Getting axis and dir
        float axis = Input.GetAxis("Horizontal");
        bool dir = (axis < 0) ? true : false;

        // Fliping sprite according to player direction
        _playerSR.flipX = dir;

        // Moving player
        _playerRB.velocity = new Vector3((axis * _runningSpeed), _playerRB.velocity.y);
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
        // Detectiong the floor
        if (Physics2D.Raycast(this.transform.position, Vector2.down, _floorDetectionLine, groundLayerMask)
            || Physics2D.Raycast(this.transform.position, Vector2.down, _floorDetectionLine, platformLayerMask)) {
            return true;
        }

        return false;
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
        Debug.Log("eating food!!!");
        // Detect if player collide with food to eat it 
        if (other.gameObject.CompareTag("Food"))
        {
            KitchenFoodController food = other.GetComponent<KitchenFoodController>();
            food.Hide();
            this.TransformCat(food.foodType);
        }
    }

    // Collision handler
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect when player touch the floor
        if (collision.gameObject.CompareTag("Floor")) {

            // Check if cat is burned
            if (_isBurned) {
                this.SetIsBurned(false);
                // Do not Ignore collision between player and platforms
                Physics2D.IgnoreLayerCollision(0, 10, false);
                Debug.Log(Physics2D.GetIgnoreLayerCollision(0,10));
            }
        }
    }

    // Settets and Getters


    // isBurned
    public void SetIsBurned(bool value){
        // Change isBurned value
        this._isBurned = value;
        // Change sprite color
        _playerSR.color = this._isBurned?Color.red:Color.white;
    }    
    
    public bool GetIsBurned(){
        return this._isBurned;
    }

    
    // playerRB
    public Rigidbody2D GetPlayerRB(){
        return this._playerRB;
    }

    void climb(bool climbing)
    {

    }
}
