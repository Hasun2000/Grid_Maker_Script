using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField, Min(1f)] private int width = 10; // Length of the grid (X-axis)
    [SerializeField, Min(1f)] private int length = 5; // Width of the grid (Z-axis)
    [SerializeField, Min(4f)] float spacing = 4.5f; // Spacing between cubes
    public GameObject GridCellPrefab; // Prefab for the cube object

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        // Clear any existing cubes before creating a new grid
        ClearGrid();

        // Loop through the grid dimensions
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                // Calculate cube position based on spacing and offset for centering
                float xPos = transform.position.x + (x - (width - 1f) / 2f) * spacing;
                float yPos = transform.position.y;
                float zPos = transform.position.z + (z - (length - 1f) / 2f) * spacing;
                Vector3 position = new Vector3(xPos, yPos, zPos);

                // Create a new cube instance from the prefab
                GameObject cube = Instantiate(GridCellPrefab, position, Quaternion.identity);

                // Parent the cube to the current object for organization
                cube.transform.parent = transform;
            }
        }
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        List<GameObject> cubesToRemove = new List<GameObject>();
        
        // Add all child objects to the list
        foreach (Transform child in transform)
        {
            cubesToRemove.Add(child.gameObject);
        }

        // Loop through the list and destroy each object
        foreach (GameObject cube in cubesToRemove)
        {
            DestroyImmediate(cube);
        }
    }

    [ContextMenu("Delete Script")]
    public void DeleteScript()
    {
        // Remove the script from the object
        DestroyImmediate(this);
    }
}
