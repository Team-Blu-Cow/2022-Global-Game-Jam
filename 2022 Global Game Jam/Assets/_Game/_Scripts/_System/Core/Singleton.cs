using UnityEngine;

namespace blu
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }

        protected virtual void Awake() => instance = this as T;

        protected virtual void OnApplicationQuit()
        {
            instance = null;
            Destroy(gameObject);
        }
    }

    public abstract class SingletonPersistant<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }

    public class Module : Singleton<Module>
    {
        public virtual void Initialize()
        {
            Debug.Log("[Module]: Initializing " + GetType().ToString());
        }
    }
}