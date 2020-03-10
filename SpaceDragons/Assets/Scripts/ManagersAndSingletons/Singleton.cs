using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance;
    public bool isPersistant;

    public virtual void Awake()
    {
        if (isPersistant)
        {
            if (Instance == null)
            {
                Instance = this as T;
                transform.SetParent(null);  
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Instance = this as T;
        }
    }
}