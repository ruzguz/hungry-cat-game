using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Different types of food 
public enum FoodType {
    healthyFood,
    catFood,
    junkFood
}

public class FoodController : MonoBehaviour
{

    // Components vars
    SpriteRenderer _foodSR;

    // Food vars
    [SerializeField]
    FoodType _foodType;


    // Moving effets vars
    Vector3 startPosition;
    [SerializeField]
    float _speed = 2f;
    [SerializeField]
    float _lineDistance = 0.5f;


    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        startPosition = this.transform.position;       
        _foodSR = GetComponent<SpriteRenderer>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Moving effect
        this.transform.position = new Vector3(transform.position.x, 
                                              startPosition.y + Mathf.Sin(Time.time * _speed) * _lineDistance);
    }

    // Function to hide the game object
    public void Hide()
    {
        _foodSR.enabled = false;
    }


    //Sent when another object enters a trigger collider attached to this
    // object (2D physics only).
    void OnTriggerEnter2D(Collider2D collider) {
        
        // Detect if collide with player
        if (collider.tag == "Player") {
            
            this.Hide();


            // Transform the cat according to the food type eaten
            switch(_foodType) {
                case FoodType.healthyFood: 
                    Debug.Log("healthy");
                break;
                case FoodType.junkFood:
                    Debug.Log("fat");
                break;
                case FoodType.catFood:
                    Debug.Log("normal");
                break;
            }

        }

    }

}
