using System.Text.Json;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 常量定义类。
/// </summary>
public class Constants
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new(){IncludeFields = true};
}