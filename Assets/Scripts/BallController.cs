using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool hasScored = false;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("TRIGGER WAOOOOOOOO");
            gameObject.SetActive(false);
        }
    }
}