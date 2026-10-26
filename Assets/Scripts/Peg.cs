using UnityEngine;

public abstract class Peg : MonoBehaviour
{
    protected GameManager gameManager;

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (CanAct())
        {
            Act();
        }
    }

    protected bool CanAct()
    {
        return gameManager != null && gameManager.GameActive;
    }

    public abstract void Act();
}