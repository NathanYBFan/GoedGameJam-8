using UnityEngine;

public class LockonMouse : MonoBehaviour
{
    [SerializeField] private Camera cam;
    void Update()
    {
        Vector2 newPos = cam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = newPos;
    }
}
