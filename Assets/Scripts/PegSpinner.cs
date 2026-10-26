using UnityEngine;

public class PegSpinner : MonoBehaviour
{
    public float speed = 180f;
    public bool clockwise = true;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    } 

    void Update()
    {
        if (gameManager.GameActive) 
        {
            float direction = clockwise ? -1f : 1f;
            transform.Rotate(0f, 0f, direction * speed * Time.deltaTime);
        }
    }
}