using GodotMonoGeneral.Logic.Module;
using GodotMonoGeneral.Logic.Utils;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 逻辑层门面类。
/// </summary>
public class LogicFacade
{
    private LogicFacade(){}

    protected static SaveLoadModule SaveLoad => SingletonFactory.GetSingleton<SaveLoadModule>();

}
