using UnityEngine;

public class BallCollisionDebug : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Debug.Log(
            $"Ball hit: {collision.gameObject.name} | " +
            $"Position: {rb.position} | " +
            $"Velocity: {rb.linearVelocity} | " +
            $"Contact: {collision.GetContact(0).normal}",
            this
        );
    }
}