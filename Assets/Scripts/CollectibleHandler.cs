using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHandler : MonoBehaviour
{
    public void PlaceItem(EquipHandler.Slot slot, Sprite sprite)
    {
        transform.GetChild((int)slot).GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
