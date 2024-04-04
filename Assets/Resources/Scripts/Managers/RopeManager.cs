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
    bool hookAvailable;
    private void Update()
    {
        if(hookAvailable && ropeHasTrajectory())
        {

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
        return Physics2D.Linecast(playerRB.transform.position, hook.position, obstacleLayers);
    }
    IEnumerator ExpandLine(Transform hook)
    {
        for(float i = 0; i < 1; i += 0.05f * Time.deltaTime)
        {
            existingRope.SetPosition(1, new Vector3(Mathf.Lerp(playerRB.transform.position.x, hook.position.x, i), Mathf.Lerp(playerRB.transform.position.y, hook.position.y, i), 0));
        }
        existingRope.SetPosition(1, hook.position);
        yield break;
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("UpHook"))
        {
            if(ropeHasTrajectory(col.transform) && (selectedHook == null) || (selectedHook != null && Vector2.Distance(playerRB.transform.position, col.transform.position) < Vector2.Distance(playerRB.transform.position, selectedHook.transform.position))
            {
                selectedHook = col.gameObject;
            }
            else
        }
    }
}
