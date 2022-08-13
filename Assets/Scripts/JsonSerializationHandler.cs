using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace DungeonMerchant.FileIO
{
    public static class JsonSerializationHandler
{
    private static string _pathToResourcesDirectory = "JSON/";
    private static readonly string _pathToDataDirectory = Path.Combine ( Application.dataPath, "GameData" );

    public static async Task Initialise ( )
    {
        await ResolveDataDirectoryAsync ( );
    }

    public static async Task ResolveDataDirectoryAsync ( )
    {
        if( !Directory.Exists ( _pathToDataDirectory ) )
            Directory.CreateDirectory ( _pathToDataDirectory );

        await UniTask.DelayFrame ( 1 );

            Debug.Log(_pathToDataDirectory);
    }

    public static async Task SerializeObjectToDataDirectory ( object objectToSerialize, string fileName )
    {
        var serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented
        };

        var fullPath = Path.Combine ( _pathToDataDirectory, fileName );

        using var streamWriter = new StreamWriter ( $@"{fullPath}" );
        using var jsonWriter = new JsonTextWriter ( streamWriter );
        
        serializer.Serialize ( streamWriter, objectToSerialize );

        await UniTask.DelayFrame ( 1 );
    }

    public static async Task<T> DeserializeObjectFromDataDirectory<T> ( string fileName )
    {
        var serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented
        };

        var fullPath = Path.Combine ( _pathToDataDirectory, fileName );

        using StreamReader streamReader = File.OpenText ( $@"{fullPath}" );
        var objectToReturn = serializer.Deserialize ( streamReader, typeof ( T ) );

        await UniTask.DelayFrame ( 1 );
        
        return (T)objectToReturn;
    }

    public static async Task < T > DeserializeObjectFromResourcesDirectory < T > ( string fileName )
    {
        var filePath = string.Concat( _pathToResourcesDirectory ,fileName.Replace ( ".json", "" ));

        var loadedFile = Resources.Load<TextAsset> ( filePath );
        var jsonText = loadedFile.text;

        var objectToReturn = JsonConvert.DeserializeObject < T > ( jsonText );

        await UniTask.DelayFrame ( 1 );

        return( T ) objectToReturn;
    }
}
}

