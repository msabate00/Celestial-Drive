using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ContructorUIElement : ScriptableObject
{
    public enum Type { Cabina = 0, Arma = 1, Bloque = 2, Button = 100}

    public string nameString;
    public Sprite visual;
    public PlacedObjectTypeSO placedObjectTypeSO;
    public Type type;

    [Tooltip("Opcional si no es de tipo Button")]
    public Type typeButton = Type.Button;
    



    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return this.placedObjectTypeSO;
    }
    public string GetNameString() {
        return nameString;
    }

#pragma warning disable CS0108 // El miembro oculta el miembro heredado. Falta una contraseña nueva
    public Type GetType() {
#pragma warning restore CS0108 // El miembro oculta el miembro heredado. Falta una contraseña nueva
        return this.type;
    }
    public Sprite GetVisual() {
        return this.visual;
    }
    public Type GetButtonType() {
        return this.typeButton;
    }


}
