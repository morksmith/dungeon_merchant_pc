using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public Item item;

    private void Start()
    {
        DoTheSave();
    }

    public async void DoTheSave()
    {
        DungeonMerchant.FileIO.JsonSerializationHandler.ResolveDataDirectoryAsync();
        DungeonMerchant.FileIO.JsonSerializationHandler.SerializeObjectToDataDirectory(item, "DMSave.json");
        Debug.Log("Did the save thing");
    }
}
