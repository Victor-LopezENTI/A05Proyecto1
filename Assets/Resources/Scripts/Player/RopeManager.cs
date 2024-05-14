using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RopeManager : MonoBehaviour
{
	public static RopeManager Instance;

	private const float RopeExpansionSpeed = 7f;
	private const float ClimbSpeed = 0.15f;

	public GameObject selectedHook;
	[SerializeField] private GameObject ropePrefab;
	private Rigidbody2D _playerRb;
	[SerializeField] private HingeJoint2D _hingeJoint;
	public LineRenderer ropeLineRenderer;
	private LayerMask _obstacleLayer;

	public float selectedHookAngle { get; private set; }
	public float selectedHookDistance { get; private set; }
	private float ropeSpeed;

	public bool hingeConnected = false;
	public bool enteringRope { get; private set; } = false;
	public bool leavingRope { get; private set; } = false;

	private Vector3 _savedPos;

	private void Awake()
	{
		#region Singleton Pattern

		if (Instance)
		{
			Debug.Log("There is already an instance of " + Instance);
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}

		#endregion

		_playerRb = GetComponent<Rigidbody2D>(); 
		_obstacleLayer= LayerMask.GetMask("Platforms");
	}

	private void Update()
	{
		/*
		if (ropeLineRenderer)
		{
			ropeLineRenderer.SetPosition(0, _playerRb.transform.position);
			selectedHookAngle = Vector2.SignedAngle(Vector2.right,
				selectedHook.transform.position - _playerRb.transform.position);
			selectedHookDistance = Vector2.Distance(_playerRb.transform.position, selectedHook.transform.position);
		}
		*/
	}

	private void FixedUpdate()
	{
		/*
		if (selectedHook != null && !PlayerStateMachine.instance.onGround && PlayerStateMachine.instance.clickInput &&
		    _ropeLineRenderer == null)
		{
			LaunchRope(selectedHook.transform);
			enteringRope = true;
		}
		else
			enteringRope = false;

		if (hingeConnected)
		{

			selectedHook.GetComponent<Rigidbody2D>().position = _savedPos;
			if (InputManager.instance.moveInput != 0 &&
			    ((InputManager.instance.moveInput < 0 && transform.position.x < selectedHook.transform.position.x) ||
			     (InputManager.instance.moveInput > 0 && transform.position.x < selectedHook.transform.position.x)))
			{
				playerRB.AddForce(new Vector2(InputManager.instance.moveInput * ropeSpeed, 0));
			}


			_playerRb.velocity = Vector2.ClampMagnitude(_playerRb.velocity, 100);
		}

		if (_ropeLineRenderer != null && (!InputManager.instance.clickInput || playerSM.onGround))
		{
			DestroyRope();
			leavingRope = true;
			selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0f;
			selectedHook.GetComponent<Rigidbody2D>().rotation = 0f;
		}
		else
			leavingRope = false;
			*/
	}

	public void LaunchRope(Transform hook)
	{
		if (RopeHasTrajectory(hook))
		{
			ropeLineRenderer = Instantiate(ropePrefab, _playerRb.transform).GetComponent<LineRenderer>();
			ropeLineRenderer.SetPosition(0, _playerRb.transform.position);
			ropeLineRenderer.SetPosition(1, _playerRb.transform.position);
			StartCoroutine(ExpandLine(hook));
		}
	}

	private bool RopeHasTrajectory(Transform hook)
	{
		return !Physics2D.Linecast(_playerRb.transform.position, hook.position, _obstacleLayer);
	}

	public void CompareHook(GameObject hook)
	{
		if (RopeHasTrajectory(hook.transform))
		{
			if (!selectedHook)
			{
				selectedHook = hook;
				hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
			}
			else if (ropeLineRenderer == null &&
			         Vector2.Distance(_playerRb.transform.position, hook.transform.position) <
			         Vector2.Distance(_playerRb.transform.position, selectedHook.transform.position))
			{
				selectedHook.GetComponent<TopHooksBehaviour>().SetHilight(false);
				selectedHook = null;
				selectedHook = hook;
				hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
			}
		}
		else
		{
			if (selectedHook == hook)
			{
				DeselectHook();
			}
		}
	}

	public void DeselectHook()
	{
		if (ropeLineRenderer)
		{
			DestroyRope();
		}

		selectedHook.GetComponent<TopHooksBehaviour>().SetHilight(false);
		Instance.selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0f;
		Instance.selectedHook.GetComponent<Rigidbody2D>().rotation = 0f;
		selectedHook = null;
	}

	private void AssignHinge()
	{
		hingeConnected = true;
		_hingeJoint.connectedBody = selectedHook.GetComponent<Rigidbody2D>();
		_hingeJoint.enabled = true;
	}

	public bool ClimbRope(float v)
	{
		Vector2 tempPos = Vector2.MoveTowards(_playerRb.position, selectedHook.transform.position, v * ClimbSpeed);
		float tempDist = Vector2.Distance(tempPos, selectedHook.transform.position);
		if (!((v > 0 && tempDist < 4) || (v < 0 && tempDist >
			    selectedHook.GetComponent<CircleCollider2D>().radius * selectedHook.transform.lossyScale.x)))
		{
			_hingeJoint.connectedBody = null;
			_playerRb.position = tempPos;
			_hingeJoint.connectedBody = selectedHook.GetComponent<Rigidbody2D>();
			return true;
		}

		return false;
	}

	private void DestroyRope()
	{
		selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0;
		_hingeJoint.enabled = false;
		_hingeJoint.connectedBody = null;
		Destroy(ropeLineRenderer.gameObject);
		hingeConnected = false;
	}

	IEnumerator ExpandLine(Transform hook)
	{
		for (float i = 0; i < 1; i += Time.deltaTime * RopeExpansionSpeed)
		{
			if (ropeLineRenderer)
			{
				ropeLineRenderer.SetPosition(0, _playerRb.transform.position);
				ropeLineRenderer.SetPosition(1, Vector3.Lerp(_playerRb.transform.position, hook.position, i));
				yield return null;
			}
			else
				yield break;
		}

		if (!ropeLineRenderer) yield break;

		ropeLineRenderer.SetPosition(1, hook.position);
		AssignHinge();
		yield break;
	}
}