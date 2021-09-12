using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PickUp, Examine, GrabDrop}

    public enum ItemType { Static, Consumables}
    public InteractionType interactType;
    public ItemType type;
    [Header("Exmaine")]
    public string descriptionText;
    [Header("Custom Event")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset()
    {
        gameObject.layer = 10;
        GetComponent<Collider2D>().isTrigger = true;
        
    }

    public void Interact()
    {
        switch(interactType)
        {
            case InteractionType.PickUp:
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                gameObject.SetActive(false);
                    break;
            case InteractionType.Examine:
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                break;

            case InteractionType.GrabDrop:
                FindObjectOfType<InteractionSystem>().GrabDrop();

                break;
            default:
                Debug.Log("Null item");
                break;

        }
        customEvent.Invoke();
    }

}
