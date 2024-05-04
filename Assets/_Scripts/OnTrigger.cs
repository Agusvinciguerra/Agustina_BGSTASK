using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerSet;
    private Items items;
    private Item item;
    //[SerializeField] private GameObject collidedItem; 

    void Start()
    {
        items = FindObjectOfType<Items>();
        item = FindObjectOfType<Item>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerSet.Invoke();

        Debug.Log(gameObject.GetComponent<Item>().itemType.ToString());

        items.SetUI(gameObject.GetComponent<Item>().itemType.ToString());
    }
}
