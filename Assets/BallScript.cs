using UnityEngine;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{   
    public GameUIManager logic;
    public Rigidbody2D ball;
    public float jumpStrength = 5f;
    public Vector2 initialVelocity = new Vector2(0f, -5f);

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {   
        if (ball == null)
            ball = GetComponent<Rigidbody2D>();
        
        ball.linearVelocity = initialVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    ContactPoint2D contact = collision.GetContact(0);
    Vector2 normal = contact.normal;

    if (collision.gameObject.CompareTag("Platform"))
    {
        if (normal.y > 0.5f)
        {
            // Landed on top
            ball.linearVelocity = new Vector2(ball.linearVelocity.x, jumpStrength);
        }
        else if (normal.y < -0.5f)
        {
            // Hit from below
            ball.linearVelocity = new Vector2(ball.linearVelocity.x, -jumpStrength);
        }
    }

    else if (collision.gameObject.CompareTag("normal_platform"))
    {
        if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x))
        {
            // Vertical collision
            if (normal.y > 0.5f)
            {
                // Landed on top
                ball.linearVelocity = new Vector2(ball.linearVelocity.x, jumpStrength);
            }
            else if (normal.y < -0.5f)
            {
                // Hit from below
                ball.linearVelocity = new Vector2(ball.linearVelocity.x, -jumpStrength);
            }
        }
        else{
                // Horizontal collision
            if (normal.x > 0.5f)
            {
                ball.linearVelocity = new Vector2(jumpStrength, ball.linearVelocity.y);
            }
            else if (normal.x < -0.5f)
            {
                ball.linearVelocity = new Vector2(-jumpStrength, ball.linearVelocity.y);
            }
        }
    }
    
    else if (collision.gameObject.CompareTag("reflect_horizontal"))
    {
        ball.linearVelocity = new Vector2(-jumpStrength, ball.linearVelocity.y);
    }
    else if (collision.gameObject.CompareTag("top_wall"))
    {
        ball.linearVelocity = new Vector2(ball.linearVelocity.x, -jumpStrength);
    }
    else if (collision.gameObject.CompareTag("lava"))
    {   
        Time.timeScale = 0f; // Resume the gameplay
        logic.gameOver();
    }
    else if (collision.gameObject.CompareTag("left_wall"))
    {
        ball.linearVelocity = new Vector2(jumpStrength, ball.linearVelocity.y);
    }
}

}
