using System.Collections;
using UnityEngine;

public class PegMovement : MonoBehaviour
{
    public float speed = 2f;
    public bool moveRight = true;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeColorRoutine());
    }

    void Update()
    {
        if (gameManager.GameActive) {
            float direction = moveRight ? 1f : -1f;
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        }        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveRight = !moveRight;
        }
    }

    IEnumerator ChangeColorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            if (spriteRenderer != null && gameManager.GameActive)
                spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}