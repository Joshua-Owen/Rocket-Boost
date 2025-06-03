using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;
    Vector3 endPosition;
    Vector3 startPosition;
    float movementFactor;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1.0f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
