using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private bool topHook = false;
    TopHooksBehaviour TopHooksBehaviour;

    private void Awake()
    {
        TopHooksBehaviour = GetComponent<TopHooksBehaviour>();
    }

    private void FixedUpdate()
    {
        TopHooksBehaviour.enabled = topHook;
    }
}