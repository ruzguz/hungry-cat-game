using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenController : MonoBehaviour
{
    // Components vars
    SpriteRenderer kitchenSR;
    Animator kitchenAnimator;
    Collider2D kitchenCollider;

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

        Debug.Log("Colllide with any");

        // Check if player collide with the kitchen on
        if (other.gameObject.CompareTag("Player") && this.kitchenAnimator.GetBool("isOn")) {
            Debug.Log("collide with the player");
            other.gameObject.GetComponent<KitchenPlayerController>().SetIsBurned(true);
        }

    }
}
