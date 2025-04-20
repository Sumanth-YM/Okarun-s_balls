using UnityEngine;

public class obstaclemove : MonoBehaviour
{
    public float baseSpeed = 3f;
    private float currentSpeed;

    void Update()
    {
        currentSpeed = baseSpeed ;
    transform.Translate(Vector2.left * currentSpeed * Time.deltaTime, Space.World);

        if (transform.position.x < -10f) // out of screen
            Destroy(gameObject);
    }
}

