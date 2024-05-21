using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TriggerTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            SetTilemapTransparency(0f); // Set alpha to 0
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTilemapTransparency(255f); // Reset alpha to 1 when player exits
        }
    }

    void SetTilemapTransparency(float alpha)
    {
        Color color = tilemap.color;
        color.a = alpha;
        tilemap.color = color;
    }
}
