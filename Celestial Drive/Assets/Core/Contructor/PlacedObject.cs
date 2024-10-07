using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    static PlacedObject actual;
    public string nameString;
    private int gridIndex;

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO, int gridIndex) {
        
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));
        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        //print(placedObject);
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.nameString = placedObjectTypeSO.nameString;
        placedObject.gridIndex = gridIndex;

        actual = placedObject;

        return placedObject;
    }
    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;

   

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }
    public PlacedObjectTypeSO.Dir GetDir()
    {
        return dir;
    }

    public int GetLabel() {
        return placedObjectTypeSO.GetLabel();
    }
    

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public Vector2Int GetOrigin()
    {
        return origin;
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO()
    {
        return placedObjectTypeSO;
    }

    public int GetGridIndex()
    {
        return gridIndex;
    }

}
