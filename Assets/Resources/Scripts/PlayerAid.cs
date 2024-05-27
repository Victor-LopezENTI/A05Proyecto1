using System;
using DG.Tweening;
using UnityEngine;

public class PlayerAid : MonoBehaviour
{
	private GameObject _destination;
	private Collider2D _collider2D;
	private int _fallCount;

	private void Awake()
	{
		_destination = transform.Find("Destination").gameObject;
		_collider2D = GetComponent<Collider2D>();
		_fallCount = 0;
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_fallCount++;
			if (_fallCount >= 5)
			{
				PlayerStateMachine.instance.playerRb.excludeLayers = LayerMask.GetMask("Platforms");
				PlayerStateMachine.instance.playerRb.DOMove(_destination.transform.position, 1.5f).onComplete =
					ExcludePlayerFromPlatforms;
			}

			Debug.Log(_fallCount);
		}
	}

	private void ExcludePlayerFromPlatforms()
	{
		PlayerStateMachine.instance.playerRb.excludeLayers = 0;
	}
}