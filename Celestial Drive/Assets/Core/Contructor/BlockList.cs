using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour
{
    public static BlockList Instance { get; private set; }
    // Start is called before the first frame update

    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }



    public List<PlacedObjectTypeSO> GetPlacedObjectTypeSOList()
    {
        return placedObjectTypeSOList;
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeByName(string name)
    {
        foreach (PlacedObjectTypeSO placedObjectTypeSO in placedObjectTypeSOList)
        {
            if (placedObjectTypeSO.nameString == name)
            {
                return placedObjectTypeSO;
            }
        }
        Debug.LogError("Tipo de objeto no encontrado: " + name);
        return null;
    }

}
