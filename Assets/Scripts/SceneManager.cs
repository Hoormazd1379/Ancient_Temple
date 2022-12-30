using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Declare a serialized field for the game level number
    [SerializeField]
    public int gameLevelNumber;

    // Declare a serialized field for the Tile prefab
    [SerializeField]
    private GameObject[] Tile = new GameObject[1];

    // Declare a serialized field for the Wall prefab
    [SerializeField]
    private GameObject[] Wall = new GameObject[3];

    // Declare a serialized field for the Player prefab
    [SerializeField]
    private GameObject Player;

    // Declare a serialized field for spacing between tiles
    [SerializeField]
    private float spacing = 1.0f;

    // Declare a serialized field for wall Y offset
    [SerializeField]
    private float wallOffset = 0.3f;

    // Create an array to store all of the instantiated tiles
    private GameObject[] Tiles;

    // Create a list to store the positions of the instantiated tiles
    private List<Vector3> tilePositions = new List<Vector3>();

    // Create a list to store the instantiated walls
    private List<GameObject> Walls = new List<GameObject>();

    // Returns a random element of a given array
    public T GetRandomElement<T>(T[] array)
    {
        // Generate a random index
        int randomIndex = UnityEngine.Random.Range(0, array.Length);

        // Return the element at the random index
        return array[randomIndex];
    }

    void GenerateWalls()
    {
        // Loop through each tile in the Tiles array
        foreach (GameObject tile in Tiles)
        {
            // Get the position of the current tile
            Vector3 tilePos = tile.transform.position;

            // Instantiate a Wall prefab at the north edge of the current tile
            if (!CheckForNeighbor(tile, "north"))
            Walls.Add(Instantiate(GetRandomElement(Wall), new Vector3(tilePos.x, tilePos.y + wallOffset, tilePos.z + spacing / 2), Quaternion.Euler(0, 90, 0)));

            // Instantiate a Wall prefab at the east edge of the current tile
            if (!CheckForNeighbor(tile, "east"))
            Walls.Add(Instantiate(GetRandomElement(Wall), new Vector3(tilePos.x + spacing / 2, tilePos.y + wallOffset, tilePos.z), Quaternion.Euler(0, 180, 0)));

            // Instantiate a Wall prefab at the south edge of the current tile
            if (!CheckForNeighbor(tile, "south"))
            Walls.Add(Instantiate(GetRandomElement(Wall), new Vector3(tilePos.x, tilePos.y + wallOffset, tilePos.z - spacing / 2), Quaternion.Euler(0, 270, 0)));

            // Instantiate a Wall prefab at the west edge of the current tile
            if (!CheckForNeighbor(tile, "west"))
            Walls.Add(Instantiate(GetRandomElement(Wall), new Vector3(tilePos.x - spacing / 2, tilePos.y + wallOffset, tilePos.z), Quaternion.Euler(0, 0, 0)));
        }
    }

    // Check if the given tile has a neighboring tile in a given direction
    bool CheckForNeighbor(GameObject tile, string direction)
    {
        // Get the position of the current tile
        Vector3 tilePos = tile.transform.position;

        // Check the neighboring tile in the specified direction
        if (direction == "north")
        {
            // Check if there is a tile at the north edge of the current tile
            if (tilePositions.Contains(new Vector3(tilePos.x, tilePos.y, tilePos.z + spacing)))
                return true;
        }
        else if (direction == "east")
        {
            // Check if there is a tile at the east edge of the current tile
            if (tilePositions.Contains(new Vector3(tilePos.x + spacing, tilePos.y, tilePos.z)))
                return true;
        }
        else if (direction == "south")
        {
            // Check if there is a tile at the south edge of the current tile
            if (tilePositions.Contains(new Vector3(tilePos.x, tilePos.y, tilePos.z - spacing)))
                return true;
        }
        else if (direction == "west")
        {
            // Check if there is a tile at the west edge of the current tile
            if (tilePositions.Contains(new Vector3(tilePos.x - spacing, tilePos.y, tilePos.z)))
                return true;
        }
        return false;
    }


    // Instantiate the game tiles
    void GenerateTiles()
    {
        // Set the starting position for the tiles
        Vector3 startPos = new Vector3(0, 0, 0);

        // Set the Y position for the tiles
        float yPos = 0.0f;

        // Instantiate tiles in a random pattern based on the game level number
        for (int i = 0; i < gameLevelNumber; i++)
        {
            // Randomly choose a direction for the next tile
            int directionX = Random.Range(0, 2);
            int directionZ = Random.Range(0, 2);

            // If the direction is 0, move the tile to the left
            if (directionX == 0)
            {
                if (directionZ == 0) {
                    startPos.x -= spacing;
                }
                else {
                    startPos.z -= spacing;
                }

            }
            // If the direction is 1, move the tile to the right
            else
            {
                if (directionZ == 0) {
                    startPos.x += spacing;
                }
                else {
                    startPos.z += spacing;
                }
            }

            // Check if the current start position has already been used for a tile
            while (tilePositions.Contains(startPos))
            {
                // If the start position has already been used, move the tile back to the previous position and try again
                if (directionX == 0)
                {
                    if (directionZ == 0) {
                        startPos.x += spacing;
                    }
                    else {
                        startPos.z += spacing;
                    }

                }
                else
                {
                    if (directionZ == 0) {
                        startPos.x -= spacing;
                    }
                    else {
                        startPos.z -= spacing;
                    }
                }
            }

            // If the start position has not been used, add it to the list of used positions
            tilePositions.Add(startPos);

            // Instantiate the Tile prefab at the current position
            GameObject tile = Instantiate(GetRandomElement(Tile), startPos, Quaternion.identity);

            //Add tile to the Tiles array
            Tiles[i] = tile;

            // Set the Y position of the tile to the fixed Y position
            tile.transform.position = new Vector3(tile.transform.position.x, yPos, tile.transform.position.z);

        }
    }

    void GeneratePlayer()
    {
        // Get the position of the first tile
        Vector3 tilePos = Tiles[0].transform.position;

        // Instantiate the Player prefab at the middle of the first tile
        Player.transform.position = new Vector3(tilePos.x, tilePos.y + 1.5f, tilePos.z);
    }



    // Use this for initialization
    void Start()
    {
        Tiles = new GameObject[gameLevelNumber];

        GenerateTiles();
        GenerateWalls();

        GeneratePlayer();
    }
}
