using System;
using System.Collections.Generic;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic.ECS;

public class ECSWorld
{
    private int nextEntityId = 0;
    private readonly Queue<int> freeIds = new();
    private readonly HashSet<int> activeIds = [];
    private readonly HashSet<ISparseSet> sparses = [];
    public int ActiveCount => activeIds.Count;
    public int AvailableCount => freeIds.Count;

    /// <summary>
    /// 创建实体。
    /// </summary>
    /// <returns></returns>
    public int CreateEntity()
    {
        if (freeIds.Count > 0)
        {
            int recycledId = freeIds.Dequeue();
            activeIds.Add(recycledId);
            return recycledId;
        }
        
        int newId = nextEntityId++;
        activeIds.Add(newId);
        return newId;
    }
    
    /// <summary>
    /// 删除实体，回收id。
    /// </summary>
    /// <param name="id"></param>
    public void DeleteEntity(int id)
    {
        if (activeIds.Remove(id))
        {
            freeIds.Enqueue(id);
            foreach (var sparse in sparses)
            {
                sparse.Delete(id);
            }
        }
    }

    private SparseSet<T> RegisterComponent<T>() where T : struct
    {
        var sparseSet = SingletonFactory.GetSingleton<SparseSet<T>>();
        sparses.Add(sparseSet);
        return sparseSet;
    }

    /// <summary>
    /// 判断实体是否拥有指定类型组件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public bool HasComponent<T>(int entityId) where T : struct
    {
        var sparseSet = RegisterComponent<T>();
        return sparseSet.Has(entityId);
    }

    /// <summary>
    /// 实体获取指定类型组件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public ref T GetComponent<T>(int entityId) where T : struct
    {
        var sparseSet = RegisterComponent<T>();
        return ref sparseSet.Get(entityId);
    }
    
    /// <summary>
    /// 实体添加或修改组件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityId"></param>
    /// <param name="component"></param>
    public void AddComponent<T>(int entityId, ref T component) where T : struct
    {
        var sparseSet = RegisterComponent<T>();
        sparseSet.Add(entityId, ref component);
    }

    /// <summary>
    /// 实体删除组件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityId"></param>
    public void DeleteComponent<T>(int entityId) where T : struct
    {
        var sparseSet = RegisterComponent<T>();
        sparseSet.Delete(entityId);
    }

    /// <summary>
    /// 实体删除所有组件。
    /// </summary>
    /// <param name="entityId"></param>
    public void DeleteComponents(int entityId)
    {
        foreach (var sparse in sparses)
        {
            sparse.Delete(entityId);
        }
    }

    /// <summary>
    /// 查询单个组件集合。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public (int, T)[] Query<T>() where T : struct
    {
        var sparseSet = RegisterComponent<T>();
        return sparseSet.GetAll();
    }

    // /// <summary>
    // /// 获取包含指定组件的所有实体id。
    // /// </summary>
    // /// <typeparam name="T"></typeparam>
    // /// <returns></returns>
    // public IEnumerable<int> GetEntities<T>() where T : struct
    // {
    //     var sparseSet = RegisterComponent<T>();
    //     return sparseSet.GetEntities();
    // }

    // /// <summary>
    // /// 查询符合条件的组件。
    // /// </summary>
    // /// <typeparam name="T"></typeparam>
    // /// <param name="filter"></param>
    // /// <returns></returns>
    // public IEnumerable<T> QueryComponents<T>(Func<T, bool> filter) where T : struct
    // {
    //     var sparseSet = RegisterComponent<T>();
    //     return sparseSet.Query(filter);
    // }


    public ECSSnapshot GetSnapshot()
    {
        var snapshot = new ECSSnapshot
        {
            date = DateTime.Now,
            sparseSnapshots = GetSparseSnapshots()
        };
        return snapshot;
    } 

    IEnumerable<SparseSnapshot> GetSparseSnapshots()
    {
        foreach (var sparse in sparses)
        {
            yield return sparse.GetSnapshot();
        }
    }

    /// <summary>
    /// 加载快照。
    /// </summary>
    /// <param name="snapshot"></param>
    public void LoadSnapshot(ECSSnapshot snapshot)
    {
        foreach (var item in snapshot.sparseSnapshots)
        {
            var type = Type.GetType(item.type);
            var sparse = (ISparseSet) SingletonFactory.GetSingleton(type);
            sparses.Add(sparse);
            sparse.LoadSnapshot(item);
        }
    }
   
}