using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Types of cat the character can be transformed according to the food it eats
public enum CatType {
    normalCat,
    fitCat,
    fatCat
};

public class PlayerController : MonoBehaviour
{

    // Components vars
    Rigidbody2D _playerRB;
    SpriteRenderer _playerSR;

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

    // Change the cat to catType
    public void ChangeCat(CatType catType)
    {
        switch(catType){
            case CatType.normalCat:
                _playerSR.sprite = normalCatSprite;
            break;
            case CatType.fitCat:
            _playerSR.sprite = fitCatSprite;
            break;
            case CatType.fatCat:
                _playerSR.sprite = fatCatSprite;
            break;
        }
    }
}
