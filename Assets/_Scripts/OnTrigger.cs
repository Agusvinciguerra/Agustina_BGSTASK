using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerSet;
    private Items items;
    private Item item;

    void Start()
    {
        // Find the Items and Item scripts
        items = FindObjectOfType<Items>();
        item = FindObjectOfType<Item>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Call the UnityEvent
        onTriggerSet.Invoke();

        // Get the item name and send it to the Items script
        Debug.Log(gameObject.GetComponent<Item>().itemType.ToString());
        items.GetName(gameObject.GetComponent<Item>().itemType.ToString());
    }
}
