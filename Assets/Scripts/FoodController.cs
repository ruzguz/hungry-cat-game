using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{


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
}
