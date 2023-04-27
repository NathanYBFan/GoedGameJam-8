using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadItem : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col) {
        if (!col.CompareTag("Item")) return;
        switch(col.name) {
            case "AsheItem(Clone)":
                break;
            default:
                break;
        }
        Destroy(col.gameObject);
    }
}
