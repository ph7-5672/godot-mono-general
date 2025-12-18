using System.Text.Json;
using Godot;

namespace GodotMonoGeneral.Utils;

/// <summary>
/// 文件读写助手。
/// </summary>
public static class IOHelper
{

    static readonly JsonSerializerOptions jsonSerializerOptions = new(){IncludeFields = true};

    /// <summary>
    /// 将文本写入文件。
    /// </summary>
    /// <param name="text">文本内容</param>
    /// <param name="file">文件路径</param>
    public static void WriteText(string text, string file)
    {
        using var fileAccess = FileAccess.Open(file, FileAccess.ModeFlags.Write);
        if (fileAccess == null)
        {
            return;
        }
        fileAccess.StoreString(text);
    }

    /// <summary>
    /// 从文件中读取文本。
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string ReadText(string file)
    {
        if (!FileAccess.FileExists(file))
        {
            return null;
        }
        using var fileAccess = FileAccess.Open(file, FileAccess.ModeFlags.Read);
        if (fileAccess == null)
        {
            return null;
        }
        return fileAccess.GetAsText();
    }

    /// <summary>
    /// 将godot对象转为json写入文件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="file"></param>
    public static void WriteGodotJson<T>(T obj, string file) where T : GodotObject
    {
        string jsonText = Json.Stringify(obj);
        WriteText(jsonText, file);
    }
    
    /// <summary>
    /// 从文件读取json对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
    public static T ReadGodotJson<T>(string file) where T : GodotObject
    {
        var jsonText = ReadText(file);
        return Json.ParseString(jsonText) as T;
    }
    
    /// <summary>
    /// 将对象转为json写入文件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="file"></param>
    public static void WriteJson<T>(T obj, string file)
    {
        var json = JsonSerializer.Serialize(obj, jsonSerializerOptions);
        WriteText(json, file);
    }

    /// <summary>
    /// 从文件读取json对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
    public static T ReadJson<T>(string file)
    {
        var json = ReadText(file);
        return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
    }

    /// <summary>
    /// Json转对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <returns></returns>
    public static T ToObject<T>(JsonElement element)
    {
        return element.Deserialize<T>(jsonSerializerOptions);
    }

}
