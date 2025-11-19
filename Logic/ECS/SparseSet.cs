namespace GodotMonoGeneral.Logic.ECS;

using System;
using System.Collections.Generic;

/// <summary>
/// 内存高效的ECS存储。
/// </summary>
/// <typeparam name="T"></typeparam>
public class SparseSet<T> : ISparseSet where T : struct
{
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

    public SparseSet(int capacity)
    {
        components = new T[capacity];
        indics = new int[capacity];
        Array.Fill(indics, -1);
    }

    public SparseSet() : this(Constants.ENTITY_MAX_COUNT)
    {
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

    /// <summary>
    /// 获取所有激活的组件。
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