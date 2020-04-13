using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("Saltando");
        }

        Debug.Log(IsTouchingTheGround());

        // If player press running buttons 
        if (Input.GetAxis("Horizontal") != 0) {
            Run();
        } else {
            _playerRB.velocity = new Vector3(0, _playerRB.velocity.y);
        }

        Debug.DrawRay(this.transform.position, Vector3.down * _floorDetectionLine, Color.red);
    }

    void Run()
    {
        // Fliping sprite according to player direction
        float dir = Input.GetAxis("Horizontal");
        _playerSR.flipX = (dir < 0)?true:false;

        // Moving player
        _playerRB.velocity = new Vector3(dir * _runningSpeed, _playerRB.velocity.y);
    }

    void Jump()
    {
        if(IsTouchingTheGround()) {
            _playerRB.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsTouchingTheGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, 
                                             Vector2.down, _floorDetectionLine, groundLayerMask);

        return hit;
    }
}
