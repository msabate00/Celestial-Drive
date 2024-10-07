using UnityEngine;

[System.Serializable]
public class PlacedObjectData
{
    public string nameString;
    public Vector3 position;
    public PlacedObjectTypeSO.Dir dir;
    public int originX, originY;
    public int gridIndex;

    public PlacedObjectData(PlacedObject placedObject)
    {
        nameString = placedObject.GetPlacedObjectTypeSO().nameString;
        position = placedObject.transform.position;
        dir = placedObject.GetDir();
        originX = placedObject.GetOrigin().x;
        originY = placedObject.GetOrigin().y;
        gridIndex = placedObject.GetGridIndex();
    }
}