using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace blu
{
    public class App : MonoBehaviour
    {
        [HideInInspector] private static App instance = null;
        private blu.ModuleManager m_moduleManager = new blu.ModuleManager();
        private List<Module> m_loadedModules = new List<Module>();

        public static List<Module> LoadedModules { get => instance.m_loadedModules; }
        public static Transform Transform { get => instance.transform; }

        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            else instance = this;

            DontDestroyOnLoad(gameObject);

            AddModule<InputModule>();
            AddModule<SceneModule>();
        }

        //<summary>
        // reflection is needed due to altering the contents
        // of non static containers.
        //
        // ...yes Matthew, this comment is for you.
        //
        // ...no I don't care if this is stupid, it
        // works and that all I care about. Now go
        // and do the work that jay told you to do
        //  - Adam <3
        //</summary>
        public static T GetModule<T>() where T : blu.Module
        {
            MethodInfo method = typeof(blu.ModuleManager).GetMethod(nameof(GetModule));
            method = method.MakeGenericMethod(typeof(T));
            return (T)method.Invoke(instance.m_moduleManager, null);
        }

        public static void AddModule<T>() where T : blu.Module
        {
            MethodInfo method = typeof(blu.ModuleManager).GetMethod(nameof(AddModule));
            method = method.MakeGenericMethod(typeof(T));
            method.Invoke(instance.m_moduleManager, null);
        }
    }
}