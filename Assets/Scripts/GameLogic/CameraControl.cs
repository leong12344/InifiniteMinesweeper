using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 20f;

    [Header("Pan Settings")]
    public float panSpeed = 10f;
    public bool enableEdgePan = false;
    public float edgeThickness = 20f;

    private Camera cam;
    private Vector3 dragOrigin;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleZoom();
        HandlePan();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    private void HandlePan()
    {
        // Mouse drag panning
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }

        // Optional: Edge screen panning (like RTS games)
        if (enableEdgePan)
        {
            Vector3 pos = cam.transform.position;

            if (Input.mousePosition.x >= Screen.width - edgeThickness)
                pos.x += panSpeed * Time.deltaTime;
            if (Input.mousePosition.x <= edgeThickness)
                pos.x -= panSpeed * Time.deltaTime;
            if (Input.mousePosition.y >= Screen.height - edgeThickness)
                pos.y += panSpeed * Time.deltaTime;
            if (Input.mousePosition.y <= edgeThickness)
                pos.y -= panSpeed * Time.deltaTime;

            cam.transform.position = pos;
        }
    }

    /// <summary>
    /// Centers the camera on the board (called from Game.cs after generating new board)
    /// </summary>
    public void CenterOnBoard(int width, int height)
    {
        cam.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        cam.orthographicSize = Mathf.Max(width, height) / 2f;
    }
}
