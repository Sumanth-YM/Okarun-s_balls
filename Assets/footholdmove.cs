using UnityEngine;

public class footholdmove : MonoBehaviour
{
    public float baseSpeed = 1f;
    private float currentSpeed;

    void Update()
    {
        currentSpeed = baseSpeed ;
    transform.Translate(Vector2.right * currentSpeed * Time.deltaTime, Space.World);


        if (transform.position.x > 10f) // out of screen
            Destroy(gameObject);
    }
}