using UnityEngine;
using DG.Tweening;

public class BottomHooksBehaviour : MonoBehaviour
{
    public GameObject highlight;
    public bool onTransition = false;

    private void Start()
    {
        if (highlight == null)
            highlight = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (highlight.transform.localPosition != Vector3.zero && !onTransition)
        {
            highlight.transform.DOMove(transform.position, 0.07f);
        }
        else if (highlight.transform.localPosition.magnitude <= 6f)
        {
            highlight.transform.localScale = Vector3.one * 0.5f;
            highlight.transform.localPosition = Vector3.zero;
        }
    }

    public void ChargeJumpAnimation(Vector2 position)
    {
        position /= 35f;
        onTransition = true;
        highlight.transform.localPosition = -Vector2.ClampMagnitude(position / 1.5f, 8f);
        highlight.transform.localScale = new Vector3(Mathf.Lerp(1f, 2f, Map(position.magnitude, 15f, 18f, 0f, 1f))
            , Mathf.Lerp(1f, 2f, Map(position.magnitude, 15f, 18f, 0f, 1f)));
    }

    private static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}