using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMove : MonoBehaviour
{
    [SerializeField] private bool isLandWalker;
    [SerializeField] private float timeToReachTarget;
    [SerializeField] private float movebuffer;
    private float timer = 0f;

    private MapManager mapManager;
    private List<int> directions = new List<int>();
    private Vector3 target;
    private Vector3 startPos;

    private void Awake() {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        InvokeRepeating("moveAnimal", 0, movebuffer);
    }

    void moveAnimal() {
        directions.Clear();
        Vector3Int animalPos = Vector3Int.FloorToInt(transform.position);
        animalPos.z = 0;
        startPos = transform.position;
        
        // Check if entity can move in the current tile
        if (!mapManager.GetTileIsLand(mapManager.GetGameMap().GetTile(animalPos)) && isLandWalker) return;
        else if (mapManager.GetTileIsLand(mapManager.GetGameMap().GetTile(animalPos)) && !isLandWalker) return;

        // Check up
        CheckDirection(animalPos, 0, 1, 0);
        // Check right
        CheckDirection(animalPos, 1, 0, 1);
        // Check down
        CheckDirection(animalPos, 0, -1, 2);
        // Check left
        CheckDirection(animalPos, -1, 0, 3);

        if (directions.Count == 0) return;
        int randomNumber = Random.Range(0, directions.Count);
        target = animalPos;

        switch(randomNumber) {
            case 0:
                target.y += 0.5f;
                break;
            case 1:
                target.x += 0.5f;
                break;
            case 2:
                target.y -= 0.5f; 
                break;
            case 3:
                target.x -= 0.5f;
                break;
        }
        target.z = Vector3Int.FloorToInt(transform.position).z;
        StartCoroutine(MoveToPosition());
    }

    private void CheckDirection(Vector3Int originalPos, int xOffset, int yOffset, int addNumber) {
        Vector3Int temp = originalPos;
        temp.x += xOffset;
        temp.y += yOffset;
        if (mapManager.GetTileIsLand(mapManager.GetGameMap().GetTile(temp))) directions.Add(addNumber);
    }

    public IEnumerator MoveToPosition() {
        timer = 0f;
        while(timer < 1) {
            timer += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPos, target, timer);
            yield return null;
        }
    }
}
