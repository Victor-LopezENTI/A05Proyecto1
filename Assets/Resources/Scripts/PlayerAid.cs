using DG.Tweening;
using UnityEngine;

public class PlayerAid : MonoBehaviour
{
    private GameObject _destination;
    private int _fallCount;

    private void Awake()
    {
        _destination = transform.Find("Destination").gameObject;
        _fallCount = 0;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_destination) return;

        if (other.CompareTag("Player"))
        {
            _fallCount++;
            if (_fallCount >= 5)
            {
                PlayerStateMachine.instance.isPaused = true;
                PlayerStateMachine.instance.playerRb.excludeLayers = LayerMask.GetMask("Platforms");
                PlayerStateMachine.instance.playerRb.DOMove(_destination.transform.position, 1.5f).onComplete =
                    UnlockPlayer;
            }

            Debug.Log(_fallCount);
        }
    }

    private void UnlockPlayer()
    {
        PlayerStateMachine.instance.playerRb.excludeLayers = 0;
        PlayerStateMachine.instance.isPaused = false;
    }
}