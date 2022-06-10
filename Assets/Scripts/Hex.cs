using UnityEngine;

public class Hex
{
    // Q + R + S = 0
    // S = -(Q + R)

    public readonly int Q; // Column
    public readonly int R; // Row
    public readonly int S;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public Hex(int q, int r)
    {
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    public Vector3 Position()
    {
        float radius = 1f;
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vertical = height * 0.75f;
        float horizont = width;

        return new Vector3(
            horizont * (this.Q + this.R/2f),
            0,
            vertical * this.R 
        );
    }
}
