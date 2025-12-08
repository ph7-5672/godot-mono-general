using System;
using System.Collections.Generic;

namespace GodotMonoGeneral.Logic.ECS;

/// <summary>
/// ECS快照信息。
/// </summary>
public class ECSSnapshot
{
    /// <summary>
    /// 保存时的日期时间。
    /// </summary>
    public DateTime date;
    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<SparseSnapshot> sparseSnapshots;

}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class SparseSnapshot
{
    /// <summary>
    /// SparseSet的类型。
    /// </summary>
    public string type;
    /// <summary>
    /// 组件字典。
    /// </summary>
    public Dictionary<int, object> components;
}