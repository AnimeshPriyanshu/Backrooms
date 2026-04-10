using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings (Must be odd numbers)")]
    public int width = 21;
    public int height = 21;
    public float tileSize = 1f;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab; // Optional

    private int[,] maze;
    
    // 1 represents a wall, 0 represents an open path

    private void Start()
    {
        // Force odd dimensions for the algorithm to work properly
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        GenerateMaze();
        DrawMaze();
    }

    private void GenerateMaze()
    {
        maze = new int[width, height];
        
        // Fill the entire grid with walls initially
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        // Start carving paths from (1, 1)
        CarvePath(1, 1);
        
        // Create an exit portal slot somewhere (e.g. at the top right)
        maze[width - 2, height - 2] = 0; 
    }

    private void CarvePath(int x, int y)
    {
        maze[x, y] = 0;

        // Shuffle directions to create a randomized maze
        int[] directions = { 0, 1, 2, 3 }; // 0: Up, 1: Right, 2: Down, 3: Left
        Shuffle(directions);

        foreach (int dir in directions)
        {
            int nx = x;
            int ny = y;

            if (dir == 0) ny += 2;      // Up
            else if (dir == 1) nx += 2; // Right
            else if (dir == 2) ny -= 2; // Down
            else if (dir == 3) nx -= 2; // Left

            // Check if within bounds and if it's currently a wall
            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 1)
            {
                // Carve a path through the wall separating the cells
                maze[x + (nx - x) / 2, y + (ny - y) / 2] = 0; 
                CarvePath(nx, ny);
            }
        }
    }

    private void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                
                if (maze[x, y] == 1)
                {
                    if (wallPrefab != null)
                        Instantiate(wallPrefab, position, Quaternion.identity, transform);
                }
                else
                {
                    if (floorPrefab != null)
                        Instantiate(floorPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
