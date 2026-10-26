using UnityEngine;

public class PegSpinner : Peg
{
    public float speed = 180f;
    public bool clockwise = true;

    public override void Act()
    {
       float direction = clockwise ? -1f : 1f;
       transform.Rotate(0f, 0f, direction * speed * Time.deltaTime);
    }
}