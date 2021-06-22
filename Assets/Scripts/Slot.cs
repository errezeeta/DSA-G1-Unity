using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public GameObject nullItem;
    public int SlotID;
    public int ID;
    public string type;
    public int description;
    public Text text;
    public Inventory inventory;
    public bool empty;
    public Sprite icon;

    public Transform slotIconGameObject;
    // Start is called before the first frame update
    void Start()
    {
        slotIconGameObject = transform.GetChild(0);
    }

    // Update is called once per frame
    public void UpdateSlot()
    {
        slotIconGameObject.GetComponent<Image>().sprite = icon;

    }

    public void UseItem()
    {
        int value;
        value = item.GetComponent<Item>().descripcion;
        int value2;
        value2 = item.GetComponent<Item>().ID;
        string typo;
        typo = item.GetComponent<Item>().type;
        Item ni = nullItem.GetComponent<Item>();
        item = nullItem;
        ID = 0;
        type = ni.type;
        description = ni.descripcion;
        icon = ni.icon;
        GameManager.instance.Change(typo,value,value2);
        inventory.Close(SlotID);
    }

    public void OnPointerClick (PointerEventData pointerEventData)
    {
        UseItem();
        
    }

}
