using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blu
{
    public class ModuleManager
    {
        public T GetModule<T>() where T : Module
        {
            return (T)blu.App.LoadedModules.Find(x => x.GetType() == typeof(T));
        }

        public void AddModule<T>() where T : Module
        {
            // base module type check
            if (typeof(T) == typeof(Module))
            {
                Debug.LogWarning("[AddModule()]: Attempted instantiation of abstract module type: " + typeof(T).ToString());
                return;
            }

            // Check for duplicate modules
            foreach (Module module in blu.App.LoadedModules)
            {
                if (module.GetType() == typeof(T))
                {
                    Debug.Log("[AddModule()]: Duplicate Module: " + typeof(T).ToString());
                    return;
                }
            }

            GameObject.Instantiate(Resources.Load<GameObject>("App/Modules/" + typeof(T).ToString())).transform.parent = blu.App.Transform;
            blu.App.LoadedModules.Add(blu.App.Transform.GetComponentInChildren<T>());

            App.GetModule<T>().Initialize();
        }
    }
}