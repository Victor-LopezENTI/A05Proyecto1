using DG.Tweening;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public static RotationManager Instance { get; private set; }

    #region Variables

    public Vector2 globalDirection { get; private set; } = Vector2.one;

    // Whether the chamber is upside down or not
    public bool chamberUpsideDown { get; private set; } = false;

    // The animated target containing the camera states
    private Animator cameraAnimator;

    // Anti-spam buffer between chamber rotations
    [SerializeField] private float actionBuffer = 0f;
    private const float maxActionBuffer = 2.5f;

    // Transition buffer for the camera rotation
    [SerializeField] private float transitionBuffer = 0f;
    private const float maxTransitionBuffer = 2f;

    // Whether the camera is transitioning or not
    private bool onTransition = false;

    #endregion

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one instance of RotationManager. Deleting the newest instance: " + gameObject.name);
            Destroy(gameObject);
        }

        #endregion

        cameraAnimator = GetComponent<Animator>();
    }

    // Update WILL be called when the game is paused
    private void Update()
    {
        if (InputManager.Instance.interactInput)
            rotateLevel();

        if (onTransition)
        {
            Time.timeScale = 0f;
            transitionBuffer += Time.unscaledDeltaTime;
            if (transitionBuffer >= maxTransitionBuffer)
            {
                onTransition = false;
                transitionBuffer = 0f;
            }
        }
        else
            Time.timeScale = 1f;
    }

    // FixedUpdate will NOT be called when the game is paused
    private void FixedUpdate()
    {
        if (actionBuffer < maxActionBuffer)
            actionBuffer += Time.deltaTime;
        else
            actionBuffer = maxActionBuffer;
    }

    public void rotateLevel()
    {

        if (isAbleToRotate())
        {
            // Rotate the hooks
            for (int i = 0; i < GameManager.Instance.hooks.Count; i++)
            {
                GameManager.Instance.hooks[i].changeState();
            }

            globalDirection = -globalDirection;
            chamberUpsideDown = !chamberUpsideDown;

            transitionCamera();

            // Change the gravity
            changeGravity();

            // Rotate the player
            float rotationAngle;
            if (chamberUpsideDown)
                rotationAngle = 180f;
            else
                rotationAngle = 0f;

            PlayerMovement.Instance.transform.DORotate(new(0, 0, rotationAngle), maxTransitionBuffer).SetUpdate(true).SetEase(Ease.InOutSine);

            // Reset the action buffer
            actionBuffer = 0f;
        }
    }

    private void changeGravity()
    {
        if (chamberUpsideDown)
            Physics2D.gravity = Vector2.up * Physics2D.gravity.magnitude;
        else
            Physics2D.gravity = Vector2.down * Physics2D.gravity.magnitude;
    }

    private bool isAbleToRotate()
    {
        if (actionBuffer == maxActionBuffer)
            return true;
        else
            return false;
    }

    private void transitionCamera()
    {
        if (chamberUpsideDown)
            cameraAnimator.Play("Upside Down");
        else
            cameraAnimator.Play("Upside Up");

        // Start the transition buffer
        transitionBuffer = 0f;
        onTransition = true;
    }

}