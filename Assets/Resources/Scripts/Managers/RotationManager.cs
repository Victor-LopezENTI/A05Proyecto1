using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public static RotationManager Instance { get; private set; }

    [SerializeField] private CameraRotation cameraRotation;
    
    // Whether the chamber is upside down or not
    public bool chamberUpsideDown { get; private set; } = false;

    // Anti-spam buffer between chamber rotations
    [SerializeField] private float actionBuffer = 3f;
    public const float maxActionBuffer = 2.5f;

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance != null)
        {
            Debug.Log("There is already an instance of " + Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        #endregion
    }

    // Update WILL be called when the game is paused
    private void Update()
    {
        if (InputManager.Instance.interactInput == 1)
        {
            rotateLevel();
        }

        if (cameraRotation.onTransition)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;
    }

    // FixedUpdate will NOT be called when the game is paused
    private void FixedUpdate()
    {
        if (actionBuffer < maxActionBuffer)
            actionBuffer += Time.deltaTime;
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
        if (actionBuffer >= maxActionBuffer)
            return true;
        else
            return false;
    }

    public void rotateLevel()
    {
        if (isAbleToRotate())
        {
            chamberUpsideDown = !chamberUpsideDown;
            cameraRotation.transitionCamera();

            // Change the gravity
            changeGravity();

            // Rotate the player
            float rotationAngle;
            if (chamberUpsideDown)
                rotationAngle = 180f;
            else
                rotationAngle = 0f;

            PlayerMovement.Instance.rotatePlayer(rotationAngle, cameraRotation.maxTransitionBuffer);

            // Reset the action buffer
            actionBuffer = 0f;
        }
    }

    public bool GetChamberUpsideDown() { return chamberUpsideDown; }
}