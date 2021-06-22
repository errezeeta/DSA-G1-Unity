using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;
    public GameObject inventory;
    private int allSlots;
    private int enabledSlots;
    private GameObject[] slot;
    public GameObject slotHolder;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        allSlots = slotHolder.transform.childCount;
        slot = new GameObject[allSlots];
        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item==null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    public void Close(int i)
    {
        inventoryEnabled = !inventoryEnabled;

        slot[i].GetComponent<Slot>().UpdateSlot();
        slot[i].GetComponent<Slot>().empty = true;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
        }

        if (inventoryEnabled == true)
        {
            inventory.SetActive(true);
            button= GameObject.Find("InventoryBtn").GetComponent<Button>();
            button.enabled = false;
        }
        else
        {
            inventory.SetActive(false);
        }
    }
    //Ante la collision con el objeto en el suelo, nos preparamos para pasarlo al inventario
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Item")
        {
            GameObject itemPickedUp = other.gameObject;

            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.ID, item.type, item.descripcion, item.icon);
        }
    }
    //Añadimos el objeto recogido (que llega como parametro) a nuestro inventario
    public void AddItem(GameObject itemObject, int itemID, string itemType, int itemDescription, Sprite itemIcon)
    {
        for (int i=0; i<allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().icon = itemIcon;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
                return;
            }
            
        }
    }

    public void Open()
    {
        inventoryEnabled = !inventoryEnabled;
        if (inventoryEnabled == true)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }
}
