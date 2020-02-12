using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject RoadTile;
    public GameObject GrassTile;
    public GameObject[] BuildingTiles;
    public byte NumberOfRows;
    public byte NumberOfColumns;
    public byte TileSize;
    public float TileMoveSpeed;
    public PlayerController playerController;

    GameObject[] LastRow;
    uint newRowSpawnFixedUpdates;
    byte currentFixedUpdates = 0;
    System.Random random = new System.Random();

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        newRowSpawnFixedUpdates = (uint)((float)TileSize / TileMoveSpeed);
        LastRow = new GameObject[NumberOfColumns];
        for (int z = 0; z < NumberOfRows; z++)
        {
            SpawnRow(z * TileSize);
        }
    }

    private void SpawnRow(float z)
    {
        for (int x = 0; x < NumberOfColumns; x++)
        {
            GameObject tile = null;
            if (x < (NumberOfColumns / 2) - 1 || x > (NumberOfColumns / 2) + 1)
            {
                tile = BuildingTiles[random.Next(0, 2)];
            }
            else if (x == (NumberOfColumns / 2) - 1 || x == (NumberOfColumns / 2) + 1)
            {
                tile = GrassTile;
            }
            else if (x == NumberOfColumns / 2)
            {
                tile = RoadTile;
            }

            var tempTile = Instantiate(tile);
            var position = new Vector3((x - (NumberOfColumns / 2)) * TileSize, 0, z);
            tempTile.transform.position = position;
            tempTile.transform.parent = transform;
            LastRow[x] = tempTile;
        }
    }

    void Update()
    {

    }

    // Happens every 0.2 seconds (Edit > Project Settings > Time> Fixed Timestep)
    void FixedUpdate()
    {
        if(playerController.isDead)
        {
            TileMoveSpeed = 0;
        }
            
        foreach (Transform childTransform in transform)
        {
            childTransform.position += new Vector3(0, 0, -TileMoveSpeed);
        }

        currentFixedUpdates++;
        if (currentFixedUpdates == newRowSpawnFixedUpdates)
        {
            SpawnRow((NumberOfRows - 1) * TileSize);
            currentFixedUpdates = 0;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("MapElement"))
        {
            Destroy(collider.gameObject);
        }
    }
}