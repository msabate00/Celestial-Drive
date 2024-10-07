using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadShip : MonoBehaviour
{
    private List<PlacedObjectTypeSO> placedObjectTypeSOList; // Lista con todos los tipos de objetos disponibles

    private void Start()
    {
        placedObjectTypeSOList = BlockList.Instance.GetPlacedObjectTypeSOList();
    }

    // Guardar la configuración actual de todos los objetos colocados
    private void SaveAllPlacedObjects(string filePath)
    {
        // Encuentra todos los objetos en la escena con el componente PlacedObject
        PlacedObject[] allPlacedObjects = FindObjectsOfType<PlacedObject>();

        // Crea una lista de datos de los objetos
        List<PlacedObjectData> placedObjectDataList = new List<PlacedObjectData>();
        foreach (PlacedObject placedObject in allPlacedObjects)
        {
            PlacedObjectData data = new PlacedObjectData(placedObject);
            placedObjectDataList.Add(data);
        }

        // Serializa la lista a JSON
        SaveData saveData = new SaveData(placedObjectDataList);
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, json);

        Debug.Log("Datos guardados en: " + filePath);
    }

    // Cargar la configuración guardada y recrear los objetos
    public void LoadAllPlacedObjects(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Archivo de guardado no encontrado en: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        // Limpiar objetos existentes antes de cargar nuevos
        GridBuildingSystem.Instance.CleanAllPlacedObjects();

        // Recorrer cada objeto guardado y recrearlo en la cuadrícula
        foreach (PlacedObjectData objectData in saveData.placedObjects)
        {
            PlacedObjectTypeSO placedObjectTypeSO = BlockList.Instance.GetPlacedObjectTypeByName(objectData.nameString);
            PlacedObjectTypeSO.Dir dir = objectData.dir;

            Vector3 worldPosition = new Vector3(objectData.position.x, objectData.position.y, objectData.position.z);
            Vector2Int origin = new Vector2Int(objectData.originX, objectData.originY);

            // Crear el objeto usando la información recuperada
            PlacedObject placedObject = PlacedObject.Create(worldPosition, origin, dir, placedObjectTypeSO, objectData.gridIndex);

            // Obtener las posiciones de la cuadrícula y establecerlas
            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            for (int i = 0; i < placedObjectTypeSO.GetLabel(); i++)
            {
                int gridIndex = objectData.gridIndex + i;
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    GridBuildingSystem.Instance.GetGridByIndex(gridIndex).GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
            }
        }

        Debug.Log("Juego cargado desde: " + filePath);
    }

    // Método para buscar el tipo de objeto por nombre en la lista
    private PlacedObjectTypeSO FindPlacedObjectTypeSOByName(string name)
    {
        foreach (PlacedObjectTypeSO placedObjectTypeSO in placedObjectTypeSOList)
        {
            if (placedObjectTypeSO.nameString == name)
            {
                return placedObjectTypeSO;
            }
        }
        Debug.LogError($"No se encontró un PlacedObjectTypeSO con el nombre: {name}");
        return null;
    }

    public void SaveShip(int id = 0)
    {
        GridBuildingSystem.Instance.DeselectObjectType();

        string filePath = Application.persistentDataPath + "/ship_" + id + ".json";
        SaveAllPlacedObjects(filePath);
        Debug.Log("Juego guardado en: " + filePath);
    }

    public void LoadShip(int id = 0)
    {
        string filePath = Application.persistentDataPath + "/ship_" + id + ".json";
        LoadAllPlacedObjects(filePath);
        Debug.Log("Juego cargado desde: " + filePath);
    }
}

// Clase auxiliar para serializar listas de objetos
[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;

    public SerializationWrapper(List<T> items)
    {
        this.items = items;
    }
}

// Clase contenedora para todos los datos a guardar
[System.Serializable]
public class SaveData
{
    public List<PlacedObjectData> placedObjects;

    public SaveData(List<PlacedObjectData> placedObjects)
    {
        this.placedObjects = placedObjects;
    }
}

