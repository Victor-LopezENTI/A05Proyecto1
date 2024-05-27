using DG.Tweening;
using UnityEngine;

public class PlayerAid : MonoBehaviour
{
    private GameObject _destination;
    private ParticleSystem _particleSystem;
    private int _fallCount;
    [SerializeField] private int fallCountMax;

    private void Awake()
    {
        _destination = transform.Find("Destination").gameObject;
        _destination.SetActive(false);
        _particleSystem = _destination.GetComponent<ParticleSystem>();
        _fallCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_destination) return;

        if (other.CompareTag("Player"))
        {
            _fallCount++;
            if (_fallCount >= fallCountMax)
            {
                MoveToDestination();
            }
        }
    }

    private void MoveToDestination()
    {
        _destination.SetActive(true);
        _particleSystem.Play();
        PlayerStateMachine.instance.isPaused = true;
        PlayerStateMachine.instance.playerRb.excludeLayers = LayerMask.GetMask("Platforms");
        PlayerStateMachine.instance.playerRb.DOMove(_destination.transform.position, 1.5f).onComplete =
            UnlockPlayer;
    }

    private void UnlockPlayer()
    {
        _destination.SetActive(false);
        _particleSystem.Stop();
        PlayerStateMachine.instance.playerRb.excludeLayers = 0;
        PlayerStateMachine.instance.isPaused = false;
    }
}