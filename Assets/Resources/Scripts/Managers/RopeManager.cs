using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RopeManager : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] PlayerStateMachine playerSM;
    [SerializeField] LayerMask obstacleLayers;
    [SerializeField] GameObject ropePrefab;
    LineRenderer existingRope;
    GameObject selectedHook;
    bool hingeConnected = false;
    [SerializeField] float ropeExpansionSpeed;
    private void Update()
    {
        if (selectedHook != null && Input.GetKeyDown("e") && existingRope == null)
        {
            launchRope(selectedHook.transform);
        }
        if (existingRope != null)
        {
            existingRope.SetPosition(0, playerRB.transform.position);
        }
    }
    void launchRope(Transform hook)
    {
        if (ropeHasTrajectory(hook))
        {
            existingRope = Instantiate(ropePrefab, playerRB.transform).GetComponent<LineRenderer>();
            existingRope.SetPosition(0, playerRB.transform.position);
            existingRope.SetPosition(1, playerRB.transform.position);
            StartCoroutine(ExpandLine(hook));
        }
    }
    bool ropeHasTrajectory(Transform hook)
    {
        return !Physics2D.Linecast(playerRB.transform.position, hook.position, obstacleLayers);
    }
    public void compareHook(GameObject hook)
    {
        if (ropeHasTrajectory(hook.transform))
        {
            if (selectedHook == null)
            {
                selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().setHilight(true);
            }
            else if(existingRope == null && Vector2.Distance(playerRB.transform.position, hook.transform.position) < Vector2.Distance(playerRB.transform.position, selectedHook.transform.position))
            {
                selectedHook.GetComponent<TopHooksBehaviour>().setHilight(false);
                selectedHook = null;
                selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().setHilight(true);
            }
        }
        else
        {
            if (selectedHook == hook)
            {
                deselectHook();
            }
        }
    }
    public void checkExittingHook(GameObject hook)
    {
        if(selectedHook == hook)
        {
            deselectHook();
        }
    }
    void deselectHook()
    {
        if (existingRope != null)
        {
            Destroy(existingRope.gameObject);
            hingeConnected = false;
        }
        selectedHook.GetComponent<TopHooksBehaviour>().setHilight(false);
        selectedHook = null;
    }
    IEnumerator ExpandLine(Transform hook)
    {
        for (float i = 0; i < 1; i += Time.deltaTime * ropeExpansionSpeed)
        {
            existingRope.SetPosition(0, playerRB.transform.position);
            existingRope.SetPosition(1, Vector3.Lerp(playerRB.transform.position, hook.position, i));
            yield return null;
        }
        existingRope.SetPosition(1, hook.position);
        hingeConnected = true;
        yield break;
    }
}
