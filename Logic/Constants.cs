using System.Text.Json;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 常量定义类。
/// </summary>
public class Constants
{
    /// <summary>
    /// 游戏存档槽位。
    /// </summary>
    public const int GAME_SAVE_CAPACITY = 8;
    public static readonly JsonSerializerOptions JsonSerializerOptions = new(){IncludeFields = true};
}