using UnityEngine;
using DG.Tweening;

public class BottomHooksBehaviour : MonoBehaviour
{
    [SerializeField] GameObject highlight;
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
        else if (highlight.transform.localPosition == Vector3.zero)
            highlight.transform.localScale = Vector3.one * 0.5f;
    }

    public void chargeJumpAnimation(Vector2 position)
    {
        onTransition = true;
        highlight.transform.localPosition = -Vector2.ClampMagnitude(position, 10f);
        highlight.transform.localScale = new(Mathf.Lerp(1f, 2f, Map(position.magnitude, 10f, 37f, 0f, 1f))
                                            ,Mathf.Lerp(1f, 2f, Map(position.magnitude, 10f, 37f, 0f, 1f)));
    }

    private static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}