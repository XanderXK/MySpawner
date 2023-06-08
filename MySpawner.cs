using System.Collections.Generic;
using UnityEngine;

public class MySpawner : MonoBehaviour
{
    private static Dictionary<string, List<GameObject>> spawnDict;
    private static Transform spawnContainer;


    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
    {
        if (spawnContainer == null)
        {
            spawnDict = new Dictionary<string, List<GameObject>>();
            spawnContainer = new GameObject("SpawnContainer").transform;
        }

        if (!spawnDict.ContainsKey(prefab.name))
        {
            spawnDict.Add(prefab.name, new List<GameObject>());
        }

        var itemList = spawnDict[prefab.name];
        var currentItem = itemList.Find(item => !item.activeSelf && item.transform.parent == spawnContainer);
        if (currentItem == null)
        {
            if (prefab is GameObject prefabGameObject)
            {
                currentItem = Instantiate(prefabGameObject, position, rotation);
            }
            else
            {
                currentItem = Instantiate((prefab as MonoBehaviour).gameObject, position, rotation);
            }

            itemList.Add(currentItem);
        }
        else
        {
            currentItem.transform.SetPositionAndRotation(position, rotation);
            currentItem.transform.SetParent(null);
            currentItem.SetActive(true);
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
        go.transform.SetParent(spawnContainer);
    }
}
