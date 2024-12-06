using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Fields.
    /// <summary>
    /// When the Unity engine quits the application, it destroys the game objects in an unpredictable order. In
    /// practice a singleton should only destroy when the application is closed, but if any class tries to access to
    /// the singleton Instance property after it is destroyed, a ghostly object may be created which may cause errors
    /// in a build, and also may persist in the scene when stopping the editor.
    /// This field is included to avoid this undesired behaviour, and ensures this ghostly object is not created, but
    /// it is mandatory to properly call the base OnDestroy method in a derived class.
    /// </summary>
    private static bool applicationIsQuitting;
    
    private static T instance;
    private bool isDuplicate;
    
    private static object lockObject = new object();
    
    // Properties.
    public static T Instance
    {
        get
        {
            if (Singleton<T>.applicationIsQuitting)
            {
                Debug.LogWarningFormat(
                    "Application quit has destroyed the Singleton of type " +
                    "{0}. It will not be created again, returning null.",
                    typeof(T).ToString());
                
                return null;
            }
            
            lock (Singleton<T>.lockObject)
            {
                // Check if static instance is initialized.
                if (Singleton<T>.instance == null)
                {
                    T[] singletonObjects = UnityEngine.Object.FindObjectsOfType<T>();
                    
                    // Check if there is at least one instance of the singleton object in the current scene.
                    if (singletonObjects.Length > 0)
                    {
                        Singleton<T>.instance = singletonObjects[0];
                        if (singletonObjects.Length > 1)
                        {
                            Debug.LogWarningFormat(
                                "More than one instance of Singleton of " +
                                "type {0} was found. Keeping the first and " +
                                "destroying the others.", typeof(T).ToString());
                        }
                        
                        for (int i = singletonObjects.Length - 1; i > 0; i--)
                        {
                            //((Singleton<T>)singletonObjects[i]).isDuplicate = true;
                            UnityEngine.Object.DestroyImmediate(singletonObjects[i].gameObject);
                        }
                        
                        return Singleton<T>.instance;
                    }
                    
                    // There are zero instances in the current scene. Creating from prefab attribute if possible.
                    PrefabAttribute attribute = Singleton<T>.PrefabAttribute;
                    if (attribute == null)
                    {
                        Debug.LogErrorFormat(
                            "Trying to create Singleton of type {0}, but " +
                            "prefab attribute is not defined.",
                            typeof(T).ToString());
                        
                        return null;
                    }
                    
                    string prefabName = attribute.Name;
                    if (string.IsNullOrEmpty(prefabName))
                    {
                        Debug.LogErrorFormat(
                            "Trying to create Singleton of type {0}, but " +
                            "prefab attribute has an empty name.",
                            typeof(T).ToString());
                        
                        return null;
                    }
                    
                    GameObject resourceObject = Resources.Load<GameObject>(prefabName);
                    if (resourceObject == null)
                    {
                        Debug.LogErrorFormat(
                            "Trying to create Singleton of type {0}, but " +
                            "cannot find a prefab named \"{1}\" in the " +
                            "resources folder.",
                            typeof(T).ToString(),
                            prefabName);
                        
                        return null;
                    }
                    
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(resourceObject);
                    gameObject.name = prefabName;
                    Singleton<T>.instance = gameObject.GetComponent<T>();
                    
                    if (Singleton<T>.instance == null)
                    {
                        Debug.LogWarningFormat(
                            "There is no component of type {0} in the " +
                            "instantiated object. A component of type {0} " +
                            "is being created. Please check the prefab and " +
                            "make sure if it has the required component " +
                            "with all the needed serialized parameters.",
                            typeof(T).ToString());
                        
                        Singleton<T>.instance = gameObject.AddComponent<T>();
                    }
                    
                    if (attribute.Persistent)
                        UnityEngine.Object.DontDestroyOnLoad(Singleton<T>.instance.gameObject);
                }
                
                // Global instance is initialized by this point, returning it.
                return Singleton<T>.instance;
            }
        }
    }
    
    private static PrefabAttribute PrefabAttribute
    {
        get { return Attribute.GetCustomAttribute(typeof(T), typeof(PrefabAttribute)) as PrefabAttribute; }
    }
    
    // Methods.
    protected virtual void Awake()
    {
        T singletonComponent = this.GetComponent<T>();
        
        if (Singleton<T>.instance == null)
        {
            Singleton<T>.instance = singletonComponent;
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }
        else if (Singleton<T>.instance != singletonComponent)
        {
            this.isDuplicate = true;
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
    
    protected virtual void OnDestroy()
    {
        if (this.isDuplicate)
            return;
        
        PrefabAttribute attribute = Singleton<T>.PrefabAttribute;
        if (attribute != null)
        {
            Singleton<T>.applicationIsQuitting |= attribute.Persistent;
        }
        else
        {
            Debug.LogErrorFormat("Prefab attribute is not defined for Singleton of type {0}.", typeof(T).ToString());
            Singleton<T>.applicationIsQuitting = true;
        }
    }
    
}