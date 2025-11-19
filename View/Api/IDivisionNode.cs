using Godot;

namespace GodotMonoGeneral.View.Api;

/// <summary>
/// 可分裂节点。
/// </summary>
public interface IDivisionNode
{
    
}

public static partial class ViewExtension
{
    /// <summary>
    /// 扩展方法，分裂。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    public static T Divide<T>(this T node) where T : Node, IDivisionNode
    {
        var parent = node.GetParent();
        if (parent == null)
        {
            return null;
        }
        var next = node.Duplicate();
        parent.CallDeferred("add_child", next);
        return next as T;
    }
}