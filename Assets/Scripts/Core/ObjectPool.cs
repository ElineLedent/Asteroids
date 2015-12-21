using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool for GameObjects instantiated from prefab
/// </summary>
public class ObjectPool
{
    private List<GameObject> m_Pool;
    private GameObject m_Prefab;
    private GameObject m_PoolParent;

    public ObjectPool(GameObject prefab, int amount)
    {
        m_Prefab = prefab;

        // Create and initialize the bullet pool
        m_Pool = new List<GameObject>();

        m_PoolParent = new GameObject();
        m_PoolParent.name = m_Prefab.name + "Pool";

        AddPoolObjects(amount);
    }

    public GameObject GetFreePoolObject()
    {
        // Search for available bullet
        for (int i = 0; i < m_Pool.Count; ++i)
        {
            if (!m_Pool[i].activeInHierarchy)
            {
                // Return from function when available bullet is found
                return m_Pool[i];
            }
        }

        // If no available bullet was found, double bullet pool size
        GameObject poolObject = AddPoolObjects(m_Pool.Count);

        Debug.LogWarning("Pool size not sufficient. Doubled the amount of pool objects");

        return poolObject;
    }

    private GameObject AddPoolObjects(int amount)
    {
        GameObject poolObject = null;

        for (int i = 0; i < amount; ++i)
        {
            poolObject = GameObject.Instantiate(m_Prefab);
            poolObject.transform.SetParent(m_PoolParent.transform);
            poolObject.SetActive(false);
            m_Pool.Add(poolObject);
        }

        // Return last created bullet
        return poolObject;
    }
}