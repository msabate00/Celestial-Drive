using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ContructorUIElement;

public class GridUISystem : MonoBehaviour
{
    public Transform menuItemParent, menuButtonParent;
    public GameObject templateElement;

    public List<ContructorUIElement> buttonsElements;

    public List<ContructorUIElement> blockElements;
    public List<ContructorUIElement> weaponElements;
    public List<ContructorUIElement> cabinElements;


    private Type activeMenu;
    private List<Transform> instancesObjects = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {

        foreach (ContructorUIElement e in buttonsElements) {
            GameObject obj = Instantiate(templateElement, menuButtonParent);

            obj.GetComponent<GridElementController>().SetConstructorUIElement(e);
            
            //instancesObjects.Add(obj.transform);
        }

        SetMenuType(Type.Arma);
    }

    
    




    public void SetMenuType(Type type) {
        activeMenu = type;
        OnChangeMenuType();
    }


    private void OnChangeMenuType() {

        foreach (Transform o in instancesObjects) {
            Destroy(o.gameObject);
        }
        instancesObjects.Clear();

        switch (this.activeMenu) {
            case Type.Bloque:
                InstantiatesObjects(blockElements);
                break;
            case Type.Arma:
                InstantiatesObjects(weaponElements);
                break;
            case Type.Cabina:
                InstantiatesObjects(cabinElements);
                break;

        }
    }


    private void InstantiatesObjects(List<ContructorUIElement> list) {

        foreach (ContructorUIElement e in list) {
            GameObject obj = Instantiate(templateElement, menuItemParent);

            obj.GetComponent<GridElementController>().SetConstructorUIElement(e);
            instancesObjects.Add(obj.transform);
        }

        
    }


}
