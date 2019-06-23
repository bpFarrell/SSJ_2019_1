using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    protected static T _Instance = null;

    public static T instance
    {
        get
        {
            T[] objects;
            if (_Instance == null)
            {
                objects = FindObjectsOfType<T>();
                if (objects.Length <= 0) return _Instance = null;
                else if (objects.Length > 1)
                {
                    for (int i = 1; i < objects.Length; i++)
                    {
                        Debug.Log("Destroying duplicates of " + typeof(T).ToString() + ". Copies of SingletonBehaviours are not advised.");
                        Destroy(objects[i]);
                    }
                }
                return (_Instance = objects[0]);
            }
            return _Instance;
        }
    }
    private void OnDisable()
    {
        _Instance = null;
    }
    private void OnEnable()
    {

    }
}
public abstract class UISingletonBehaviour<T> : UIMonoBehaviour where T : UISingletonBehaviour<T>
{
    protected static T _Instance = null;

    public static T instance
    {
        get
        {
            T[] objects;
            if (_Instance == null)
            {
                objects = FindObjectsOfType<T>();
                if (objects.Length <= 0) return _Instance = null;
                else if (objects.Length > 1)
                {
                    for (int i = 1; i < objects.Length; i++)
                    {
                        Debug.Log("Destroying duplicates of " + typeof(T).ToString() + ". Copies of SingletonBehaviours are not advised.");
                        Destroy(objects[i]);
                    }
                }
                return (_Instance = objects[0]);
            }
            return _Instance;
        }
    }
    private void OnDisable()
    {
        _Instance = null;
    }
    private void OnEnable()
    {

    }
}

public abstract class UIMonoBehaviour : MonoBehaviour {
    public RectTransform rectTransform {
        get {
            return (RectTransform)transform;
        }
    }
}
