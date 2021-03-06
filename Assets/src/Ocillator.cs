﻿using UnityEngine;

public class Ocillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    float movementFactor;
    Vector3 startingPosition;
    
    // Use this for initialization
    void Start () {
        startingPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if(period == 0) { return; }
        const float pi = Mathf.PI;
        float cycle = Time.time / period;
        movementFactor = Mathf.Sin(cycle * pi); // Ocillate between -1 and 1.
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
