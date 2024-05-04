using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerSet;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Call the UnityEvent
        onTriggerSet.Invoke();
    }
}
