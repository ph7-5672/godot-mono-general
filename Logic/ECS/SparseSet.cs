namespace GodotMonoGeneral.Logic.ECS;

using System;

/// <summary>
/// 内存高效的ECS存储。
/// </summary>
/// <typeparam name="T"></typeparam>
public class SparseSet<T> where T : struct
{
    /// <summary>
    /// 紧凑存储实际数据。
    /// </summary>
    private readonly T[] components;

    /// <summary>
    /// 索引映射。
    /// </summary>
    private readonly int[] indics;

    private int count;

    public int Count => count;

    public SparseSet(int capacity = Constants.ENTITY_MAX_COUNT)
    {
        components = new T[capacity];
        indics = new int[capacity];
        Array.Fill(indics, -1);
    }


    /// <summary>
    /// 添加或修改组件。
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="component"></param>
    public void AddOrUpdate(int entityId, T component)
    {
        int index;
        if (Has(entityId))
        {
            index = indics[entityId];
        }
        else
        {
            index = count;
            indics[entityId] = index;
            ++count;
        }
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
    public T Get(int entityId)
    {
        if (!Has(entityId))
        {
            return default;
        }
        var index = indics[entityId];
        return components[index];
    }

    public T this[int index]
    {
        get
        {
            return components[index];
        }

    }



}