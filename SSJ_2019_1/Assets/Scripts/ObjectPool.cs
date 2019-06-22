using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static List<ObjectPool> activeItem = new List<ObjectPool>();
    private static List<ObjectPool> pooledItems = new List<ObjectPool>();
    private static GameObject holder;
    public Vector3 dir;
    public float killAt;
    public static ObjectPool GetObject() {
        if (pooledItems.Count == 0) {
            PopulatePool(100);
        }
        ObjectPool obj = pooledItems[0];
        pooledItems.RemoveAt(0);
        activeItem.Add(obj);
        obj.gameObject.SetActive(true);
        return obj;
    }
    public static void PopulatePool(int count) {
        if (holder == null)
            holder = new GameObject("poolHolder");
        for (int x = 0; x < count; x++) {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.parent = holder.transform;
            go.SetActive(false);
            ObjectPool obj = go.AddComponent<ObjectPool>();
            pooledItems.Add(obj);
        }
        Debug.Log("Pooling more! pool is at "+(pooledItems.Count+activeItem.Count));
    }
    public void MarkComplete() {
        activeItem.Remove(this);
        pooledItems.Add(this);
        gameObject.SetActive(false);
    }
    public void Update() {
        transform.position += dir * Time.deltaTime;
        if (Time.time > killAt)
            MarkComplete();
    }
}
