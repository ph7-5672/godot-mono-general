using System;
using System.Collections.Generic;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic;

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
    /// <param name="entityId">实体id</param>
    public void DeleteComponent<T>(int entityId) where T : struct
    {
        var sparseSet = RegisterComponent<T>();
        sparseSet.Delete(entityId);
    }

    /// <summary>
    /// 实体删除所有组件。
    /// </summary>
    /// <param name="entityId">实体id</param>
    public void DeleteComponents(int entityId)
    {
        foreach (var sparse in sparses)
        {
            sparse.Delete(entityId);
        }
    }

    /// <summary>
    /// 获取快照。
    /// </summary>
    /// <returns>快照数据</returns>
    public ECSSnapshot GetSnapshot()
    {
        var snapshot = new ECSSnapshot
        {
            date = DateTime.Now,
            sparseSnapshots = GetSparseSnapshots()
        };
        return snapshot;
    } 
    /// <summary>
    /// 获取所有稀疏集合的快照。
    /// </summary>
    /// <returns>快照集合</returns>
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
    /// <param name="snapshot">快照数据</param>
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

    /// <summary>
    /// 获取存档路径。
    /// </summary>
    /// <param name="index">存档索引</param>
    /// <returns>路径字符串</returns>
    private string GetSavePath(int index)
    {
        return $"res://Saves/{index}.save";
    }

    /// <summary>
    /// 加载指定索引的存档。
    /// </summary>
    /// <param name="index">存档索引</param>
    public void LoadSave(int index)
    {
        if (index < 0)
        {
            return;
        }
        var path = GetSavePath(index);
        var snapshot = IOHelper.ReadJson<ECSSnapshot>(path);
        LoadSnapshot(snapshot);
        EventBus.Raise("load_save", index); // 发送事件。
    }

    /// <summary>
    /// 保存游戏到指定存档。
    /// </summary>
    /// <param name="index">存档索引</param>
    public void SaveGame(int index)
    {
        if (index < 0)
        {
            return;
        }
        var path = GetSavePath(index);
        var snapshot = GetSnapshot();
        IOHelper.WriteJson(snapshot, path);
        EventBus.Raise("save_game", index); // 发送事件。
    }
   
}