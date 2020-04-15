using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Fliping sprite according to player direction
        float dir = Input.GetAxis("Horizontal");
        _playerSR.flipX = (dir < 0)?true:false;

        // Moving player
        _playerRB.velocity = new Vector3(dir * _runningSpeed, _playerRB.velocity.y);
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
                //ResizeCatColliders(0.06f, -0.32f, 1.14f, 0.85f, 0.25f, 0.31f, 0.34f);  
            break;
            case FoodType.junkFood:
                _playerSR.sprite = fatCatSprite;
                //ResizeCatColliders(-0.01f, -0.4f, 1f, 0.8f, 0f, 0.3f, 0.34f);
            break;
            case FoodType.catFood:
                _playerSR.sprite = normalCatSprite;
                //ResizeCatColliders(0.25f, -0.35f, 1.08f, 0.52f, 0.41f, 0.22f, 0.34f);
            break;
        }
    }

    // Function to set player collider 
    // void ResizeCatColliders(float bodyOffsetX, float bodyOffsetY, float bodyX, float bodyY, float headX, float headY, float headRadius)
    // {
    //     _bodyCollider.offset = new Vector2(bodyOffsetX, bodyOffsetY);
    //     _bodyCollider.size = new Vector2(bodyX, bodyY);
    //     _headCollider.offset = new Vector2(headX, headY);
    //     _headCollider.radius = headRadius;
    // }

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
