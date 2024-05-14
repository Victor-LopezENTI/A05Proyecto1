using System;
using UnityEngine;

public class TopHooksBehaviour : MonoBehaviour
{
	private GameObject _highlight;

	private void OnEnable()
	{
		if (_highlight == null)
			_highlight = transform.GetChild(0).gameObject;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			RopeManager.Instance.CompareHook(gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (gameObject == RopeManager.Instance.selectedHook)
			{
				SetHilight(false);
				RopeManager.Instance.selectedHook = null;
			}
		}
	}

	public void SetHilight(bool state)
	{
		_highlight.SetActive(state);
	}
}