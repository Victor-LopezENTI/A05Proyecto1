using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] public GameObject visualCue;
    [SerializeField] public TextAsset inkJson;
    private bool _isPlayerInRange;
    private Animator _thanatosAnimator;

    private void Awake()
    {
        visualCue.SetActive(false);
        _isPlayerInRange = false;
        _thanatosAnimator = GetComponentInParent<Animator>();
        _thanatosAnimator.Play("blank");
        _thanatosAnimator.SetBool("interacted", false);
    }

    private void Update()
    {
        if (_isPlayerInRange && !DialogueManager.GetInstance().dialoguePlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.I))
            {
                _thanatosAnimator.SetBool("interacted", true);
                DialogueManager.GetInstance().EnterDialogue(inkJson);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
        }
    }
}