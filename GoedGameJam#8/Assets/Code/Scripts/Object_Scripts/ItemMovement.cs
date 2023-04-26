using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private RuleTile ruleTile;
    private Tilemap conveyorMap;
    private Vector3 target;

    void OnTriggerStay2D(Collider2D col) {
        if (!col.CompareTag("Conveyors")) return;
        conveyorMap = col.GetComponent<Tilemap>();
        if (conveyorMap.GetSprite(Vector3Int.FloorToInt(transform.position)) == null) return;
        Debug.Log(MapManager.GetConveyorDictionary()[conveyorMap.GetSprite(Vector3Int.FloorToInt(transform.position))].directionOfMovement);
        target = this.transform.position;

        switch (MapManager.GetConveyorDictionary()[conveyorMap.GetSprite(Vector3Int.FloorToInt(transform.position))].directionOfMovement) {
            case Enums.Directions.up:
                target.y -= 1;
                break;
            case Enums.Directions.upRight:
                target.y -= 1;
                target.x += 1;
                break;
            case Enums.Directions.right:
                target.x += 1;
                break;
            case Enums.Directions.downRight:
                target.y += 1;
                target.x += 1;
                break;
            case Enums.Directions.down:
                target.y += 1;
                break;
            case Enums.Directions.downLeft:
                target.y += 1;
                target.x -= 1;
                break;
            case Enums.Directions.left:
                target.x -= 1;
                break;
            case Enums.Directions.upLeft:
                target.y -= 1;
                target.x -= 1;
                break;
        }
        StartCoroutine(MoveOverSeconds(this.gameObject, target, speed));
    }

    public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed) {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end) {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame ();
        }
    }
    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds) {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds) {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
}
