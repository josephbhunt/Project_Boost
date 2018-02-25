using UnityEngine;

public class RotationOscillator : MonoBehaviour {

    [SerializeField] Quaternion rotation;
    [SerializeField] float period;
    float movementFactor;
    Quaternion startingRotation;

    // Use this for initialization
    void Start () {
        startingRotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (period == 0) { return; }
        const float pi = Mathf.PI;
        float cycle = Time.time / period;
        movementFactor = Mathf.Sin(cycle * pi); // Ocillate between -1 and 1.
        Vector3 offset = rotation.eulerAngles * movementFactor;
        Vector3 newEulerAngles = startingRotation.eulerAngles + offset;
        Quaternion newRotation = transform.rotation;
        newRotation.eulerAngles = newEulerAngles;
        transform.rotation = newRotation;
    }
}
