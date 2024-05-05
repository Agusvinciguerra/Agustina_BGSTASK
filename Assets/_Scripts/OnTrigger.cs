using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerSet;
    [SerializeField] private UnityEvent onTriggerExit;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Call the UnityEvent
        onTriggerSet.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Call the UnityEvent
        onTriggerExit.Invoke();
    }
}
