using Kryz.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public static class MovementPatterns
{
    public static Vector2 Straight(Vector2 position, SOMovementParameters parameters, float deltaTime)
    {
        Vector2 newPosition = position + parameters.Direction.normalized * parameters.speed * deltaTime;
        return newPosition;
    }

    public static Vector2 Sine(Vector2 position, SOMovementParameters parameters,float time, float direction)
    {
        Vector2 movementdirection = parameters.Direction.normalized;
        //Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        Vector2 perpendicular = new Vector2(-movementdirection.y, movementdirection.x);

        Vector2 forwardMovement = movementdirection * parameters.speed * time;
        float offset = Mathf.Sin(time * parameters.frequency) * parameters.amplitude * direction;

        return position + forwardMovement + perpendicular * offset;
    }

    public static Vector2 MoveToTarget(Vector2 startPosition, Vector2 targetPosition, float elapsedTime, SOMovementParameters parameters)
    {
        float t = elapsedTime / parameters.duration;

        t = Mathf.Clamp01(t);
        t = EasingFunctions.OutCirc(t);
        return Vector2.Lerp(startPosition, targetPosition, t);
    }
}
