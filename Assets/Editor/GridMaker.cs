using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridCreator))]
public class GridMaker : Editor
{
    private GridCreator gridCreator; // Reference to the GridCreator script

    private GameObject startCube; // Reference to the start cube
    private GameObject endCube; // Reference to the end cube
    private GameObject lastHighlightedCube; // Reference to the last highlighted cube
    public GameObject pathCubePrefab; // Reference to the path cube prefab

    public Material highlightMaterial; // Reference to the highlight material
    private Material lastHighlightedCubeOriginalMaterial; // Reference to the original material of the last highlighted cube

    public Material startMaterial; // Reference to the start material
    private Material startCubeOriginalMaterial; // Reference to the original material of the start cube
    public Material endMaterial; // Reference to the end material
    private Material endCubeOriginalMaterial; // Reference to the original material of the end cube
    public Material pathMaterial; // Reference to the path material

    private void OnEnable()
    {
        gridCreator = (GridCreator)target;
    }

    private void OnSceneGUI()
    {
        //Check if the GridCreator object is selected
        if (Selection.activeGameObject != gridCreator.gameObject)
        {
            // If the GridCreator object is not selected, return
            return;
        }

        Event guiEvent = Event.current;
        Vector3 mousePos = guiEvent.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;

        Ray ray = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject != null)
            {
                HighlightCube(hitObject);

                if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
                {
                    SelectCube(hitObject);
                    guiEvent.Use();
                }
            }
        }
        else if (lastHighlightedCube != null)
        {
            // If the mouse is not over any cube, restore the last highlighted cube's material
            lastHighlightedCube.GetComponent<Renderer>().sharedMaterial = lastHighlightedCubeOriginalMaterial;
            lastHighlightedCube = null;
            lastHighlightedCubeOriginalMaterial = null;
        }
    }

    private void HandleMouseEvents()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (startCube == null)
                {
                    startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    startCube.transform.position = hit.point;
                    startCube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    startCube.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (endCube == null)
                {
                    endCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    endCube.transform.position = hit.point;
                    endCube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    endCube.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    private void HighlightCube(GameObject cube)
    {
        if(cube != lastHighlightedCube)
        {
            // if the mouse has moved over to a new cube, restore the original material of the last highlighted cube
            if (lastHighlightedCube != null && lastHighlightedCubeOriginalMaterial != null)
            {
                // Restore the original material of the last highlighted cube
                lastHighlightedCube.GetComponent<Renderer>().sharedMaterial = lastHighlightedCubeOriginalMaterial;
            }

            // Store the original material of the current cube
            lastHighlightedCubeOriginalMaterial = cube.GetComponent<Renderer>().sharedMaterial;
            lastHighlightedCube = cube;

            // Change the material of the cube to a highlight material
            // need to have a reference to the highlight material here
            cube.GetComponent<Renderer>().sharedMaterial = highlightMaterial;
        }
    }

    private void SelectCube(GameObject cube)
    {
        if (startCube == null)
        {
            startCube = cube;
            startCubeOriginalMaterial = cube.GetComponent<Renderer>().sharedMaterial;

            // Change the material of the cube to a start material
            startCube.GetComponent<Renderer>().sharedMaterial = startMaterial;
        }
        else if (endCube == null)
        {
            endCube = cube;
            endCubeOriginalMaterial = cube.GetComponent<Renderer>().sharedMaterial;

            // Change the material of the cube to an end material
            endCube.GetComponent<Renderer>().sharedMaterial = endMaterial;
            CreatePath();
        }
    }

    private void CreatePath()
    {
        if (startCube != null && endCube != null)
        {
            // Create a path between the start and end cubes
            // ...
            // Calculate the direction and distance between the start and end cubes
            Vector3 direction = (endCube.transform.position - startCube.transform.position).normalized;
            float distance = Vector3.Distance(startCube.transform.position, endCube.transform.position);

            // Calculate the scale of the path cube
            Vector3 scale = new Vector3(4.5f, 1, distance + 4.5f);

            // Calculate the position of the path cube
            Vector3 position = startCube.transform.position + direction * distance / 2;

            // Calculate the rotation of the path cube
            Quaternion rotation = Quaternion.identity;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                // The path is going along the x-axis
                rotation = Quaternion.Euler(0, direction.x > 0 ? 90 : -90, 0);
            }

            // Create the path cube
            GameObject pathCube = Instantiate(pathCubePrefab, position, rotation);
            pathCube.GetComponent<Renderer>().sharedMaterial = pathMaterial;
            pathCube.transform.localScale = scale;

            // Parent the path cube to the current object for organization
            pathCube.transform.parent = gridCreator.transform;

            // Reset the start and end cubes
            startCube = null;
            endCube = null;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GridCreator gridCreator = (GridCreator)target;
        if (GUILayout.Button("Create Grid"))
        {
            gridCreator.CreateGrid();
        }
        if (GUILayout.Button("Clear Grid"))
        {
            gridCreator.ClearGrid();
        }
        if (GUILayout.Button("Delete Script"))
        {
            gridCreator.DeleteScript();
        }
    }
}

