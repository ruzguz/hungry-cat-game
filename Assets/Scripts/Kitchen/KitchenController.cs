using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenController : MonoBehaviour
{
    // Components vars
    SpriteRenderer kitchenSR;
    Animator kitchenAnimator;
    Collider2D kitchenCollider;

    // Other vars
    [SerializeField]
    float bounceForce = 10;

    // Is called before the frame 0
    private void Awake() {
        kitchenAnimator = this.GetComponent<Animator>();
        kitchenSR = this.GetComponent<SpriteRenderer>();
        kitchenCollider = this.GetComponent<Collider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D other) {

        // Check if player collide with the kitchen on
        if (other.gameObject.CompareTag("Player") && this.kitchenAnimator.GetBool("isOn")) {
            // get player
            KitchenPlayerController p = other.gameObject.GetComponent<KitchenPlayerController>();
            p.SetIsBurned(true);
            p.GetPlayerRB().AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            // Ignore collision between player and platforms
            Physics2D.IgnoreLayerCollision(0, 10, true);
            Debug.Log(Physics2D.GetIgnoreLayerCollision(0,10));
        }

    }

    // Setters and Getters
    public Collider2D GetKitchenCollider() {
        return this.kitchenCollider;
    }
}
