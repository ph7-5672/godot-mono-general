using Godot;

namespace GodotMonoGeneral.Utils;

public static class ViewExtension
{
    /// <summary>
    /// 扩展方法，分裂。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    public static T Divide<T>(this T node) where T : Node
    {
        var parent = node.GetParent();
        if (parent == null)
        {
            return null;
        }
        var next = node.Duplicate();
        parent.AddChild(next);
        return next as T;
    }

    /// <summary>
    /// 打开场景并添加到指定节点下。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scenePath">指定场景路径</param>
    /// <returns></returns>
    public static T OpenScene<T>(this Node node, string scenePath) where T : Node
    {
        var packedScene = SingletonFactory.GetSingleton(() => GD.Load<PackedScene>(scenePath), scenePath);
        var t = packedScene.Instantiate<T>();
        node.AddChild(t);
        return t;
    }
}