using System.Collections.Generic;
using UnityEngine;

public class MySpawner : MonoBehaviour
{
    private static Dictionary<string, Queue<GameObject>> _spawnDict;
    private static Transform _spawnContainer;


    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
    {
        if (_spawnContainer == null)
        {
            _spawnDict = new Dictionary<string, Queue<GameObject>>();
            _spawnContainer = new GameObject("SpawnContainer").transform;
        }

        if (!_spawnDict.ContainsKey(prefab.name))
        {
            _spawnDict.Add(prefab.name, new Queue<GameObject>());
        }

        if (_spawnDict[prefab.name].TryDequeue(out var currentItem))
        {
            currentItem.transform.SetParent(null);
            currentItem.transform.SetPositionAndRotation(position, rotation);
            currentItem.SetActive(true);
        }
        else
        {
            if (prefab is GameObject prefabGameObject)
            {
                currentItem = Instantiate(prefabGameObject, position, rotation);
            }
            else
            {
                currentItem = Instantiate((prefab as MonoBehaviour).gameObject, position, rotation);
            }
        }

        if (prefab is GameObject)
        {
            return (T)(Object)currentItem;
        }

        return currentItem.GetComponent<T>();
    }

    public static void Despawn(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(_spawnContainer);
        _spawnDict[go.name.Replace("(Clone)", string.Empty)].Enqueue(go);
    }
}
