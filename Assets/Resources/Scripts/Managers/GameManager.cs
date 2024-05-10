using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern

    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                // Load the prefab from the Resources folder
                var prefab = Resources.Load<GameObject>("Prefabs/Managers/GameManager");

                // Instantiate the prefab
                var inScene = Instantiate<GameObject>(prefab);

                // Get the manager component from the prefab
                _Instance = inScene.GetComponentInChildren<GameManager>();

                // If the component is not found, add it to the prefab
                if (!_Instance)
                    _Instance = inScene.AddComponent<GameManager>();

                DontDestroyOnLoad(_Instance.transform.root.gameObject);
            }
            return _Instance;
        }
    }

    #endregion

    public List<Hook> hooks { get; private set; }

    private void Awake()
    {
        hooks = new List<Hook>();
    }

    public void AddHook(Hook hook)
    {
        hooks.Add(hook);
    }
    
    public void RemoveHook(Hook hook)
    {
        hooks.Remove(hook);
    }
    
    public void SwitchHooksState()
    {
        foreach (var hook in hooks)
        {
            hook.ChangeState();
        }
    }
}