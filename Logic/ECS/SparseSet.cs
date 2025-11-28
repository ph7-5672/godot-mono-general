namespace GodotMonoGeneral.Logic.ECS;

using System;
using System.Collections.Generic;
using GodotMonoGeneral.Utils;

/// <summary>
/// 内存高效的ECS存储。
/// </summary>
/// <typeparam name="T"></typeparam>
public class SparseSet<T> : ISparseSet where T : struct
{
    public const int ENTITY_MAX_COUNT = 8192;

    /// <summary>
    /// 紧凑存储实际数据。
    /// </summary>
    private readonly T[] components;

    /// <summary>
    /// 索引映射。
    /// </summary>
    private readonly int[] indics;

    /// <summary>
    /// 激活的组件数量。
    /// </summary>
    private int count;

    public int Count => count;

    public SparseSet()
    {
        components = new T[ENTITY_MAX_COUNT];
        indics = new int[ENTITY_MAX_COUNT];
        Clear();
    }


    /// <summary>
    /// 添加组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="component"></param>
    public void Add(int entityId, ref T component)
    {
        var index = count;
        indics[entityId] = index;
        ++count;
        components[index] = component;
    }

    /// <summary>
    /// 判断实体是否拥有组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public bool Has(int entityId)
    {
        return indics[entityId] != -1;
    }

    /// <summary>
    /// 查询获取组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public ref T Get(int entityId)
    {
        var index = indics[entityId];
        return ref components[index];
    }

    /// <summary>
    /// 删除组件。
    /// </summary>
    /// <param name="entityId"></param>
    public void Delete(int entityId)
    {
        indics[entityId] = -1;
    }

    public void Clear()
    {
        Array.Fill(indics, -1);
        Array.Fill(components, new T());
        count = 0;
    }

    /// <summary>
    /// 获取拥有该组件的实体id集合。
    /// </summary>
    /// <returns></returns>
    public IEnumerable<int> GetEntities()
    {
        for (int i = 0; i < indics.Length; i++)
        {
            var index = indics[i];
            if (index != -1)
            {
                yield return i;
            }
        }
    }

    public IEnumerable<T> GetComponents()
    {
        var entities = GetEntities();
        foreach (var entity in entities)
        {
            var component = Get(entity);
            yield return component;
        }
    }

    /// <summary>
    /// 获取快照信息。
    /// </summary>
    /// <returns></returns>
    public SparseSnapshot GetSnapshot()
    {
        var snapshot = new SparseSnapshot
        {
            type = typeof(SparseSet<T>).AssemblyQualifiedName,
        };
        var entities = GetEntities();
        var dict = new Dictionary<int, object>();
        foreach (var entity in entities)
        {
            var component = Get(entity);
            dict.Add(entity, component);
        }
        snapshot.components = dict;
        return snapshot;
    }


    /// <summary>
    /// 加载快照。
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="components"></param>
    public void LoadSnapshot(SparseSnapshot snapshot)
    {
        Clear();
        foreach (var (entity, component) in snapshot.components)
        {
            if (component is System.Text.Json.JsonElement element)
            {
                var t = IOHelper.ToObject<T>(element);
                Add(entity, ref t);
            }
        }
    }

    /// <summary>
    /// 根据索引获取组件。
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T this[int index]
    {
        get
        {
            return components[index];
        }

    }



}