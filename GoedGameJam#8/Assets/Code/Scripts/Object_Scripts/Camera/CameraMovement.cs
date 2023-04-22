using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool canPan;
    private Camera cam;
    private Vector3 dragOrigin;

    [SerializeField] private SpriteRenderer mapRenderer;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    void Awake()
    {
        cam = this.gameObject.GetComponent<Camera>();
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPan)
            PanCameraDrag();
        else
            PanCameraHover();
    }

    private void PanCameraDrag() {
        // Save position of mouse in worls space when drag starts
        if (Input.GetMouseButtonDown(2))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        // Calculate distance between drag origin and new position if it is still held down
        if (Input.GetMouseButton(2)) {
            Vector3 diffrence = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            // Move the camera by that distance
            cam.transform.position = ClampCamera(cam.transform.position + diffrence);
        }
    }

    private void PanCameraHover() {

    }

    private Vector3 ClampCamera(Vector3 targetPosition) {
        float camWidth = cam.orthographicSize * cam.aspect;

        float newX = Mathf.Clamp(targetPosition.x, (mapMinX + camWidth), (mapMaxX - camWidth));
        float newY = Mathf.Clamp(targetPosition.y, (mapMinY + cam.orthographicSize), (mapMaxY - cam.orthographicSize));

        return new Vector3(newX, newY, targetPosition.z);
    }
}
