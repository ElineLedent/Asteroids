using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_Instance;

    private static bool s_IsDestroyed = false;

    public static T Instance
    {
        get
        {
            if (s_IsDestroyed)
            {
                Debug.Log("Singleton was accessed after being destroyed");
                return null;
            }

            if (s_Instance != null)
            {
                return s_Instance;
            }
            else
            {
                Debug.Assert(Resources.FindObjectsOfTypeAll(typeof(T)).Length < 2, "Found too many objects of type " + typeof(T));

                s_Instance = FindObjectOfType<T>();

                if (s_Instance == null)
                {
                    GameObject emptyGameObject = new GameObject();
                    s_Instance = emptyGameObject.AddComponent<T>();
                }
                else
                {
                    Debug.Log("Found an object of type " + typeof(T));
                }

                return s_Instance;
            }
        }
    }

    private void Awake()
    {
        // Make sure static variable is reverted to false
        s_IsDestroyed = false;
    }

    private void OnDestroy()
    {
        s_IsDestroyed = true;
    }
}