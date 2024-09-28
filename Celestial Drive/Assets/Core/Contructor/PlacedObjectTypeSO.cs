using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlacedObjectTypeSO : ScriptableObject
{
    
    public enum Dir{Front,Left,Back,Right}

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width = 1;
    public int height = 1;
    public int labels = 1;


    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Front:     return Dir.Left;
            case Dir.Left:      return Dir.Back; 
            case Dir.Back:      return Dir.Right;
            case Dir.Right:     return Dir.Front;
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        switch (dir) {
            default:
            case Dir.Front:
            case Dir.Back:

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;

            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++) {
                    for (int y = 0; y < width; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    
    
    }
    public int GetLabel() {
        return labels;
    }

    public int GetRotationAngle(Dir dir) {
        switch (dir) {
            default:
            case Dir.Front: return 0;
            case Dir.Right: return 90;
            case Dir.Back:  return 180;
            case Dir.Left:  return 270;
        }
    }
    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Front: return new Vector2Int(0,0);
            case Dir.Right: return new Vector2Int(0, width);
            case Dir.Back: return new Vector2Int(width, height);
            case Dir.Left: return new Vector2Int(height, 0);
        }
    }

    




}
