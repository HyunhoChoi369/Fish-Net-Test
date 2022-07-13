using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Create();
            }

            return instance;
        }
        protected set => instance = value;
    }

    public static void Create()
    {
        if (instance == null)
        {
            GameObject prefab = Resources.Load(typeof(T).Name) as GameObject;
            if (prefab == null)
            {
                Debug.Assert(false, $"can't find {typeof(T).Name} Prefab");
                return;
            }

            GameObject obj = Instantiate(prefab);
            DontDestroyOnLoad(obj);
            instance = obj.GetComponent<T>();
        }
    }
}

