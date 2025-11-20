using Godot;

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
}