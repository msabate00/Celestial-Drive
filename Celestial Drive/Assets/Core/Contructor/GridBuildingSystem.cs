using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;

    private List<GridXZ<GridObject>> grids = new List<GridXZ<GridObject>>();
    private GridXZ<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Front;
    private int gridIndex;


    private Material roofMaterial, floorMaterial;

    public int size = 5;
    public float cellSize = 5;
    public bool putCenterPrincipalObject = true;
    public bool showDebug = false;
    public bool CameraAjust = true;
    public Transform floorGridToRay, roofGridToRay;

    private bool isDemolishActive = false;

    public static GridBuildingSystem Instance { get; private set; }
    public event EventHandler OnSelectedChanged;
    //public event EventHandler OnObjectPlaced;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        
        int gridWidth = size;
        int gridHeight = size;
        int gridVerticals = size;

        placedObjectTypeSO = placedObjectTypeSOList[0];

        roofMaterial = roofGridToRay.GetComponent<MeshRenderer>().sharedMaterial;
        floorMaterial = floorGridToRay.GetComponent<MeshRenderer>().sharedMaterial;


        for (int i = 0; i < gridVerticals; i++)
        {
            Vector3 offSet = new Vector3(0, i * cellSize, 0);
            grids.Add(new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, offSet, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z), showDebug));
        }

        if (CameraAjust)
        {
            Camera.main.transform.position = new Vector3(((grids.Count / 2)) * cellSize, ((grids.Count / 2) + 2) * cellSize, Camera.main.transform.position.z);
        }
        roofMaterial.SetFloat("_TilingFino", size);
        floorMaterial.SetFloat("_TilingFino", size);


        gridIndex = Mathf.FloorToInt(grids.Count / 2) + 1;
        grid = grids[gridIndex];

        floorGridToRay.position = new Vector3((cellSize * size) / 2, ((gridIndex) * cellSize) - ((cellSize * size) / 2), (cellSize * size) / 2);
        roofGridToRay.position = new Vector3((cellSize * size)/2, ((gridIndex + 1) * cellSize) + ((cellSize * size) / 2), (cellSize * size) / 2);

        floorGridToRay.localScale = new Vector3(cellSize * size, cellSize * size, cellSize * size);
        roofGridToRay.localScale = new Vector3(cellSize * size, cellSize * size, cellSize * size);


        if (putCenterPrincipalObject)
        {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            int x = Mathf.FloorToInt(size / 2)-1;
            
            int z = Mathf.FloorToInt(size / 2)-1;
            
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            
            try
            {
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, z), dir);
                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
            }
            catch (System.Exception)
            {
                Debug.LogError("EL OBJETO DEBE DE TENER EL COMPONENTE PLACED_OBJECT");
            }
        }


    }

    public void Start()
    {
       
    }


    private void Update()
    {

        HandleNormalObjectPlacement();
        HandleLayers();
        HandleDemolish();
        HandleDirRotation();
        if (Input.GetKeyDown(KeyCode.X)) {
            isDemolishActive = !isDemolishActive;
        }
        if (Input.GetMouseButtonDown(1))
        {
            DeselectObjectType();
        }
    }



    private void HandleNormalObjectPlacement()
    {
        if (Input.GetMouseButtonDown(0) && !isDemolishActive)
        {
            

            try
            {
                Vector3 pos = Mouse3D.GetMouseWorldPosition();
                if (pos != Vector3.zero)
                {


                    grid.GetXZ(pos, out int x, out int z);
                    List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, z), dir);

                    bool canBuild = true;


                    for (int i = 0; i < placedObjectTypeSO.GetLabel(); i++)
                    {
                        if (gridIndex + i >= grids.Count)
                        {
                            //Debug.Log("GI: " + gridIndex + " I: " + i + "  R: " + gridIndex + i);
                            break;
                        }
                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            //if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                            if (!grids[gridIndex + i].GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                            {
                                canBuild = false;
                                break;
                            }
                        }

                    }


                    if (canBuild)
                    {
                        Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                        Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                        try
                        {
                            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);
                            for (int i = 0; i < placedObjectTypeSO.GetLabel(); i++)
                            {
                                if (gridIndex + i >= grids.Count) { break; }
                                foreach (Vector2Int gridPosition in gridPositionList)
                                {
                                    //grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                                    grids[gridIndex + i].GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                                }
                            }

                        }
                        catch (System.Exception)
                        {
                            Debug.LogError("EL OBJETO DEBE DE TENER EL COMPONENTE PLACED_OBJECT");
                        }


                    }
                    else
                    {
                        Debug.Log("NO SE PUEDE CONTRUIR");
                    }
                }
            }
            catch (System.Exception)
            {

            }
            FindObjectOfType<UI_StatsRadarChart>().GetComponent<UI_StatsRadarChart>().GetAndUpdateAll();
        }
    }

    private void HandleDirRotation() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
        
    }

    

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

        if (mousePosition != Vector3.zero)
        {

            grid.GetXZ(mousePosition, out int x, out int z);

            if (placedObjectTypeSO != null)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                return placedObjectWorldPosition;
            }
            else
            {
                return mousePosition;
            }
        }
        else {
            return mousePosition;
        }
    }
    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectTypeSO != null)
        {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    {
        return placedObjectTypeSO;
    }
    public void SelectPlacedObject(PlacedObjectTypeSO placedObjectTypeSO)
    {
        this.placedObjectTypeSO = placedObjectTypeSO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    public void SetDemolishActive()
    {
        placedObjectTypeSO = null;
        isDemolishActive = true;
        RefreshSelectedObjectType();
    }

    public bool IsDemolishActive()
    {
        return isDemolishActive;
    }




    private void HandleDemolish()
    {
        if (isDemolishActive && Input.GetMouseButtonDown(0))
        {
            
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();

            if (mousePosition != Vector3.zero)
            {

                PlacedObject placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
                if (placedObject != null)
                {
                    // Demolish
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
                FindObjectOfType<UI_StatsRadarChart>().GetComponent<UI_StatsRadarChart>().GetAndUpdateAll();
            }
        }
    }

    private void HandleLayers() {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gridIndex -= 1;
            if (gridIndex < 0) { gridIndex = grids.Count - 1; }
            grid = grids[gridIndex];
            floorGridToRay.position = new Vector3(floorGridToRay.position.x, ((gridIndex) * cellSize) - ((cellSize * size) / 2), floorGridToRay.position.z);
            roofGridToRay.position = new Vector3(roofGridToRay.position.x, ((gridIndex + 1) * cellSize) + ((cellSize * size) / 2), roofGridToRay.position.z);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            gridIndex += 1;
            if (gridIndex >= grids.Count) { gridIndex = 0; }
            grid = grids[gridIndex];
            floorGridToRay.position = new Vector3(floorGridToRay.position.x, ((gridIndex) * cellSize) - ((cellSize * size) / 2), floorGridToRay.position.z);
            roofGridToRay.position = new Vector3(roofGridToRay.position.x, ((gridIndex + 1) * cellSize) + ((cellSize * size) / 2), roofGridToRay.position.z);
        }
    }


    private void DeselectObjectType()
    {
        placedObjectTypeSO = null;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }


    private void RefreshSelectedObjectType()
    {
        //UpdateCanBuildTilemap();

        if (placedObjectTypeSO == null)
        {
            //TilemapVisual.Instance.Hide();
        }
        else
        {
            //TilemapVisual.Instance.Show();
        }

        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }







    public class GridObject
    {

        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;


        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedObject(PlacedObject transform)
        {
            this.placedObject = transform;
            grid.TriggerGridObjectChanged(x, z);
        }
        public PlacedObject GetPlacedObject()
        {
            return this.placedObject;
        }
        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }


        public override string ToString()
        {
            return x + ", " + z + " \n" + placedObject;
        }

    }




}
