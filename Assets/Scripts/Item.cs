using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Hair,
    Hat,
    Shirt,
    Pants,
    Shoes
}

[CreateAssetMenu(menuName = "Item Data")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public int id;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public int Price;
    public int sellAmount;
}
