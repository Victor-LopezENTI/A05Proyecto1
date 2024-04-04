using System.Collections;
using System.Collections.Generic;
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
}