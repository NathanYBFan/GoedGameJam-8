using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemMovement : MonoBehaviour
{
    public MapManager mapManager;
    private RuleTile ruleTile;
    private Tilemap conveyorMap;
    private Vector3 target;
    private Coroutine movement;
    private float distance = 0.1f;

    void Start(){
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    }

    void FixedUpdate()
    {
        target = this.transform.position;
        if (mapManager.GetMachineMap().GetTile(Vector3Int.FloorToInt(new Vector3(transform.position.x, transform.position.y, 0))) is MovementTile movementTile) {
            target += new Vector3(movementTile.movementDir.x, movementTile.movementDir.y, 0);            
        }
        else if (mapManager.GetMachineMap().GetTile(Vector3Int.FloorToInt(new Vector3(transform.position.x, transform.position.y, 0))) is OutputTile outputTile) {
            target += new Vector3(outputTile.movementDir.x, outputTile.movementDir.y, 0);            
        }        
        else if (mapManager.GetMachineMap().GetTile(Vector3Int.FloorToInt(new Vector3(transform.position.x, transform.position.y, 0))) is ContainerTile containerTile) {
            containerTile.heldItem = this.gameObject;
        }
        this.transform.position = target;
    }
    
    void OnTriggerStay2D(Collider2D col) {
        if (!col.CompareTag("Conveyors")) return;

        conveyorMap = col.GetComponent<Tilemap>();
        Vector3 positionToCheck = transform.position;
        positionToCheck.z = 0f;

        if (conveyorMap.GetSprite(Vector3Int.FloorToInt(positionToCheck)) == null) return;
        Vector3 temp = Vector3Int.FloorToInt(positionToCheck);
        target = this.transform.position;

        switch (MapManager.GetConveyorDictionary()[conveyorMap.GetSprite(Vector3Int.FloorToInt(positionToCheck))].directionOfMovement) {
            case Enums.Directions.up:
                target.y += distance;
                break;
            case Enums.Directions.upRight:
                target.x += distance;
                break;
            case Enums.Directions.rightUp:
                target.y += distance;
                break;
            case Enums.Directions.right:
                target.x += distance;
                break;
            case Enums.Directions.rightDown:
                target.y -= distance;
                break;
            case Enums.Directions.downRight:
                target.x += distance;
                break;
            case Enums.Directions.down:
                target.y -= distance;
                break;
            case Enums.Directions.downLeft:
                target.x -= distance;
                break;
            case Enums.Directions.leftDown:
                target.y -= distance;
                break;
            case Enums.Directions.left:
                target.x -= distance;
                break;
            case Enums.Directions.leftUp:
                target.y += distance;
                break;
            case Enums.Directions.upLeft:
                target.x -= distance;
                break;
            default:
                break;
        }
        this.transform.position = target;
    }

}
