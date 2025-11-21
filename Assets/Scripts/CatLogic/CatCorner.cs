using UnityEngine;

public class CatCorner : MonoBehaviour
{
    public Vector2 screenOffset = new Vector2(50f, 50f);
    public Camera cam;

    private void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    private void LateUpdate()
    {
        Vector3 screenPos = new Vector3(screenOffset.x, screenOffset.y, cam.nearClipPlane + 5f);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);

        transform.position = worldPos;

        transform.localScale = Vector3.one;
    }
}
