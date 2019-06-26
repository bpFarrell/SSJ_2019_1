using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private static Canvas _MainCanvas;
    private static GraphicRaycaster _MainRaycaster;
    public RectTransform rectTransform {
        get {
            return (RectTransform)transform;
        }
    }
    /// <summary>
    /// Defined in UIMonoBehavior base class. Uses FindGameObjectWithTag() to define this static variable on first use. (Sorry Brandon)...
    /// </summary>
    public static Canvas MainCanvas
    {
        get
        {
            return _MainCanvas ?? (_MainCanvas = GetMainCanvas());
        }
    }
    
    /// <summary>
    /// Retrieves attached GraphicRaycaster on the MainCanvas Object.
    /// </summary>
    public static GraphicRaycaster MainRaycaster
    {
        get
        {
            return _MainRaycaster ?? (_MainRaycaster = GetMainRaycaster());
        }
    }

    private static GraphicRaycaster GetMainRaycaster()
    {

        return _MainCanvas.GetComponent<GraphicRaycaster>();
    }

    /// <summary>
    /// Returns True if either the MainCanvas or the provided local canvas return any hit info
    /// at the provided input location.
    /// </summary>
    /// <param name="canvas">Canvas to queury.</param>
    /// <param name="input">The screen space position for the query.</param>
    /// <returns>True if a UI element was hit on either canvas</returns>
    public static bool RaycastBothCanvasForUI(Canvas canvas, Vector3 input)
    {
        return (RaycastMainCanvasForUI(input) || RaycastLocalCanvasForUI(canvas, input));
    }
    /// <summary>
    /// Returns True if the MainCanvas has any UI Elements at the provided screen space position.
    /// </summary>
    /// <param name="input">The screen space position for the query.</param>
    /// <returns>True if a UI element was hit in the Arena canvas canvas</returns>
    public static bool RaycastMainCanvasForUI(Vector3 input)
    {
        return RaycastLocalCanvasForUI(MainCanvas, input);
    }
    /// <summary>
    /// Returns True if the provided canvas has any UI Elements at the provided screen space position.
    /// </summary>
    /// <param name="canvas">Canvas to queury.</param>
    /// <param name="input">The screen space position for the query.</param>
    /// <returns>True if a UI element was hit in the provided canvas</returns>
    public static bool RaycastLocalCanvasForUI(Canvas canvas, Vector3 input)
    {
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> hit = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(null);
        ped.position = input;
        gr.Raycast(ped, hit);
        return hit.Count > 0;
    }
    /// <summary>
    /// Returns a screenspace Rect from a RectTransform
    /// </summary>
    /// <param name="rectTransform">RectTransform to generate a Rect from</param>
    /// <returns></returns>
    public static Rect RectTransformToScreenSpace(RectTransform rectTransform)
    {
        Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
        var position = rectTransform.position;
        return new Rect(position.x, Screen.height - position.y, size.x, size.y);
    }
    private static Canvas GetMainCanvas()
    {
        Canvas target = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        if (target == null) Debug.Log("#MainCanvas#There is no object in scene with \"MainCanvas\" Tag. Please assign a canvas with the Tag.");
        return target;
    }
    private RectTransform GetRectTransform()
    {
        RectTransform target = GetComponent<RectTransform>();
        if (target == null) Debug.Log("#MainCanvas#UI Element \"" + transform.name + "\" does not have a RectTransform");
        return target;
    }
}
