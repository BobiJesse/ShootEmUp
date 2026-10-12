using UnityEngine;

public static class CameraHelper
{
    public static bool SpriteBoundsInCameraView(SpriteRenderer spriteRenderer, Camera camera)
    {
        Bounds bounds = spriteRenderer.bounds;

        //This could be done with WorldToViewportPoint, but it's pointless since we have ortographic camera and we can just use size
        float cameraHeight = camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        Vector3 cameraPosition = camera.transform.position;

        if (bounds.max.x >= cameraPosition.x - cameraWidth &&
            bounds.min.x <= cameraPosition.x + cameraWidth &&
            bounds.max.y >= cameraPosition.y - cameraHeight &&
            bounds.min.y <= cameraPosition.y + cameraHeight)
            return true;
        else
            return false;
    }

    public static Vector2 GetSpawnPositionAbove(SpriteRenderer spriteRenderer, Camera camera, int index, int count)
    {
        float cameraHeight = camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        Vector3 cameraPosition = camera.transform.position;

        float left = cameraPosition.x - cameraWidth;
        float right = cameraPosition.x + cameraWidth;

        //We can use linear interpolation from viewport's left edge to right edge in order to distribute objects evenly. 
        //Index + 0.5 offsets objects, so that they don't end up right to the edge of the left or right edge of the viewport.
        float x = Mathf.Lerp(left, right, (index + 0.5f) / count);

        //Sprite's bottom edge right above the viewport.
        float y = cameraPosition.y + cameraHeight + spriteRenderer.bounds.extents.y;

        return new Vector2(x, y);
    }
    public static Vector3 ClampSpriteToCameraView(SpriteRenderer spriteRenderer, Camera camera, Vector3 position)
    {
        Bounds bounds = spriteRenderer.bounds;

        float halfWidth = bounds.extents.x;
        float halfHeight = bounds.extents.y;

        float cameraHeight = camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        Vector3 cameraPosition = camera.transform.position;

        float minX = cameraPosition.x - cameraWidth + halfWidth;
        float maxX = cameraPosition.x + cameraWidth - halfWidth;

        float minY = cameraPosition.y - cameraHeight + halfHeight;
        float maxY = cameraPosition.y + cameraHeight - halfHeight;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
}