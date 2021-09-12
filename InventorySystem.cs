using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    public List<GameObject> items;

    public bool isOpen;
    [Header("UI Item Section")]

    public GameObject UI_Windown;

    public Image[] Items_image;


    [Header("UI Item Description")]

    public GameObject ui_Description_Windown;

    public Image Description_Image; 

    public Text Description_Title;

    public Text Description_Text;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
    private void ToggleInventory()
    {
        isOpen = !isOpen;
        UI_Windown.SetActive(isOpen);
        Update_UI();
    }

    public void PickUp(GameObject item)
    {
        items.Add(item);
        Update_UI();
    }
    private void Update_UI()
    {
        HideAll();
        for(int i = 0; i < items.Count; i ++)
        {
            Items_image[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            Items_image[i].gameObject.SetActive(true);
        }
    }
    private void HideAll()
    {
        foreach(var i in Items_image) { i.gameObject.SetActive(false); }
        HideDescription();
    }    
    public void ShowDescription(int id)
    {
        //Set the Image
        Description_Image.sprite = Items_image[id].sprite;
        //set the Title
        Description_Title.text = items[id].name;
        //Show the description
        Description_Text.text = items[id].GetComponent<Item>().descriptionText;
        //Show the Elements
        Description_Image.gameObject.SetActive(true);
        Description_Title.gameObject.SetActive(true);
        Description_Text.gameObject.SetActive(true);

    }
    public void HideDescription()
    {
        Description_Image.gameObject.SetActive(false);
        Description_Title.gameObject.SetActive(false);
        Description_Text.gameObject.SetActive(false);
    }

    public void Consume(int id)
    {
        if (items[id].GetComponent<Item>().type == Item.ItemType.Consumables)
        {
            Debug.Log($"Consumed{ items[id].name}");
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            //destroy the item from list
            Destroy(items[id], 0.1f);
            //remove item from list
            items.RemoveAt(id);
            //Update UI
            Update_UI();
            
        }
    }
}
