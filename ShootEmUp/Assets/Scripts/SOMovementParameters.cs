using UnityEngine;

[CreateAssetMenu(fileName = "SOMovementParameters", menuName = "Scriptable Objects/SOMovementParameters")]
public class SOMovementParameters : ScriptableObject
{
    public enum MovePattern { Straight, Sine, Target }
    public MovePattern Pattern;

    public Vector2 Direction = Vector2.down;
    public float speed = 1f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public float duration = 1f;
    public Vector2 TargetOffset = Vector2.zero;
}
