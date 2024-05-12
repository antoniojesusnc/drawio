using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMB<PoolManager>
{
    public class Pool
    {
        public GameObject           m_Prefab;
        public Stack<GameObject>    m_FreeInstances;
    }

    private Dictionary<string, Pool> m_Pools;

    public static void CreatePool(string _Name, GameObject _Prefab, int _Count)
    {
        Instance.CreatePool_Internal(_Name, _Prefab, _Count);
    }

    private void CreatePool_Internal(string _Name, GameObject _Prefab, int _Count)
    {
        if (m_Pools == null)
            m_Pools = new Dictionary<string, Pool>();

        if (m_Pools.ContainsKey(_Name))
        {
            Debug.LogError("A pool already exists for the object " + _Name);
            return;
        }

        // Create the new pool
        Pool newPool = new Pool();
        newPool.m_Prefab = _Prefab;
        newPool.m_FreeInstances = new Stack<GameObject>();

		// Create initial instances
		if (newPool.m_Prefab != null) 
		{
			for (int i=0; i< _Count; ++i)
			{
				GameObject newInstance = Instantiate(newPool.m_Prefab, transform) as GameObject;
				newInstance.SetActive(false);
				newPool.m_FreeInstances.Push(newInstance);
			}
		}
		else
			Debug.LogWarning("There is no prefab for the pool " + _Name);

        m_Pools.Add(_Name, newPool);
    }

    public static void FreePool(string _Id)
    {
        Instance.FreePool_Internal(_Id);
    }

    private void FreePool_Internal(string _Id)
    {
        if (m_Pools == null || !m_Pools.ContainsKey(_Id))
            return;

        Pool pool = m_Pools[_Id];

        // Destroy all instances in this pool
        while (pool.m_FreeInstances.Count > 0)
            Destroy(m_Pools[_Id].m_FreeInstances.Pop());

        // Remove the pool from the dictionary
        m_Pools.Remove(_Id);
    }

    public static void FreeAllPools()
    {
        Instance.FreeAllPools_Internal();
    }

    private void FreeAllPools_Internal()
    {
        if (m_Pools == null || m_Pools.Count == 0)
            return;

        Dictionary<string, Pool>.KeyCollection keys = m_Pools.Keys;

        foreach (string key in keys)
            FreePool(key);
    }

    public static GameObject GetInstance(string _Id)
    {
        return Instance.GetInstance_Internal(_Id);
    }

    private GameObject GetInstance_Internal(string _Id)
    {
        if (m_Pools == null || !m_Pools.ContainsKey(_Id))
        {
            Debug.LogError("There is no pool for the object " + _Id + " ! can't return any object");
            return null;
        }

        Pool pool = m_Pools[_Id];

        // No more instances in this pool, need to create a new one
        if (pool.m_FreeInstances.Count == 0)
            return Instantiate(pool.m_Prefab) as GameObject;

        GameObject freeInstance = pool.m_FreeInstances.Pop();
        freeInstance.SetActive(true);
        return freeInstance;
    }

    public static void FreeInstance(string _Id, GameObject _Instance)
    {
        Instance.FreeInstance_Internal(_Id, _Instance);
    }

    private void FreeInstance_Internal(string _Id, GameObject _Instance)
    {
        if (m_Pools == null || !m_Pools.ContainsKey(_Id))
        {
            Debug.LogError("There is no pool for the object " + _Id + " ! can't free this object");
            return;
        }

        _Instance.SetActive(false);
        _Instance.transform.SetParent(transform);

        m_Pools[_Id].m_FreeInstances.Push(_Instance);
    }
}
