using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridElementController : MonoBehaviour
{
    public Image visual;
    public TextMeshProUGUI toolTip;

    private ContructorUIElement contructorUIElement;
    private GridUISystem gridUISystem;
    private GridBuildingSystem gridBuildingSystem;


    private void Awake()
    {
        gridUISystem = FindObjectOfType<GridUISystem>();
        gridBuildingSystem = FindObjectOfType<GridBuildingSystem>();
    }


    public void SetConstructorUIElement(ContructorUIElement contructorUIElement) {
        this.contructorUIElement = contructorUIElement;
        this.visual.sprite = contructorUIElement.visual;
        toolTip.text = contructorUIElement.nameString;

        GetComponent<Button>().onClick.AddListener(OnClick);

    }

    void OnClick() {
        if (contructorUIElement.type == ContructorUIElement.Type.Button)
        {
            gridUISystem.SetMenuType(contructorUIElement.typeButton);
        }
        else {
            gridBuildingSystem.SelectPlacedObject(contructorUIElement.placedObjectTypeSO);
            //SIGNIFICA QUE ES UN BLOQUE NORMAL Y NO UN BOTON
           
        }
        
    }


}
