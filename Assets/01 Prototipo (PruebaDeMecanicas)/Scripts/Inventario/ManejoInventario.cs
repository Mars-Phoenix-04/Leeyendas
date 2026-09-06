using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemInv
{
    public string NombreItem;
    public int Cantidad;
}


public class ManejoInventario : MonoBehaviour
{
    public HashSet<ItemInv> inventario = new HashSet<ItemInv>();

    public void AgregarItem(string NombreItem)
    {
        ItemInv ItemExistente = inventario.FirstOrDefault(item => item.NombreItem == NombreItem);

        if (ItemExistente != null)
        {
            ItemExistente.Cantidad++;
        }
        else
        {
            ItemInv NuevoItem = new ItemInv { NombreItem = NombreItem, Cantidad = 1 };
            inventario.Add(NuevoItem);
        }
    }

    public bool TieneItem(string NombreItem)
    {
        ItemInv ItemExistente = inventario.FirstOrDefault(item => item.NombreItem == NombreItem);
        return ItemExistente != null && ItemExistente.Cantidad > 0;
    }
}
