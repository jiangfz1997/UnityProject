using System.Collections.Generic;

[System.Serializable]
public class ItemConfigEntry
{
    public int id;
    public string itemName;
    public string addressableKey;
}

[System.Serializable]
public class ItemConfigData
{
    public List<ItemConfigEntry> items;
}
