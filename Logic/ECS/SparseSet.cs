namespace GodotMonoGeneral.Logic.ECS;

using System;
using System.Collections.Generic;
using System.Linq;
using GodotMonoGeneral.Utils;

/// <summary>
/// 内存高效的ECS存储。
/// </summary>
/// <typeparam name="T"></typeparam>
public class SparseSet<T> : ISparseSet where T : struct
{
    const int initialCapacity = 64;
    /// <summary>
    /// 紧凑存储实际数据。
    /// </summary>
    private T[] components;
    /// <summary>
    /// 索引映射。
    /// </summary>
    private int[] indics;
    /// <summary>
    /// 实体id数组。
    /// </summary>
    private int[] entities;
    /// <summary>
    /// 激活的组件数量。
    /// </summary>
    public int Count { get; private set; }
    /// <summary>
    /// 组件容量。
    /// </summary>
    public int Capacity { get; private set; }

    public SparseSet()
    {
        components = new T[initialCapacity];
        entities = new int[initialCapacity];
        indics = new int[initialCapacity];
        Capacity = initialCapacity;
        Clear();
    }

    /// <summary>
    /// 添加组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="component"></param>
    public void Add(int entityId, ref T component)
    {
        if (Has(entityId))
        {
            throw new InvalidOperationException($"Entity {entityId} already has component {typeof(T).Name}");
        }
        if (entityId >= indics.Length)
        {
            var capacity = Math.Max(entityId + 1, indics.Length * 2);
            Array.Resize(ref indics, capacity);
        }
        if (Count >= Capacity)
        {
            // 自动扩容。
            Capacity *= 2;
            Array.Resize(ref entities, Capacity);
            Array.Resize(ref components, Capacity);
        }
        var index = Count;
        indics[entityId] = index;
        entities[index] = entityId;
        components[index] = component;
        ++Count;
    }

    /// <summary>
    /// 判断实体是否拥有组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public bool Has(int entityId)
    {
        if (entityId >= indics.Length)
        {
            return false;
        }
        var index = indics[entityId];
        return index != -1 && index < Count;
    }

    /// <summary>
    /// 查询获取组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public ref T Get(int entityId)
    {
        if (!Has(entityId))
        {
            throw new KeyNotFoundException($"Entity {entityId} does not have component {typeof(T).Name}");
        }

        var index = indics[entityId];
        return ref components[index];
    }

    /// <summary>
    /// 删除组件。
    /// </summary>
    /// <param name="entityId"></param>
    public void Delete(int entityId)
    {
        var index = indics[entityId];
        if (index == -1)
        {
            return;
        }
        var lastIndex = Count - 1;
        // 将最后一个元素移到被删除的位置。
        entities[index] = entities[lastIndex];
        components[index] = components[lastIndex];
        entities[lastIndex] = -1;
        // 清理。
        indics[entityId] = -1;
        --Count;
    }

    /// <summary>
    /// 清空。
    /// </summary>
    public void Clear()
    {
        Array.Fill(indics, -1);
        Array.Fill(entities, -1);
        Count = 0;
    }
    /// <summary>
    /// 遍历委托。
    /// </summary>
    /// <param name="entity">实体id</param>
    /// <param name="component">组件数据</param>
    public delegate void RefAction(int entity, ref T component);
    /// <summary>
    /// 遍历所有实体和组件。
    /// </summary>
    /// <param name="action"></param>
    public void ForEach(ref RefAction action)
    {
        for (int i = 0; i < Count; i++)
        {
            var entity = entities[i];
            ref var component = ref components[i];
            action(entity, ref component);
        }
    }

    /// <summary>
    /// 获取快照信息。
    /// </summary>
    /// <returns>快照数据</returns>
    public SparseSnapshot GetSnapshot()
    {
        var snapshot = new SparseSnapshot
        {
            type = typeof(SparseSet<T>).AssemblyQualifiedName,
        };
        var dict = new Dictionary<int, object>();
        for (int i = 0; i < Count; i++)
        {
            var entity = entities[i];
            var component = components[i];
            if (entity == -1)
            {
                break;
            }
            dict.Add(entity, component);
        }
        snapshot.components = dict;
        return snapshot;
    }

    /// <summary>
    /// 加载快照。
    /// </summary>
    /// <param name="snapshot">快照数据</param>
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

}