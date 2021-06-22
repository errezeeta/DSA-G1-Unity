using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public string type;
    public int descripcion;
    public Sprite icon;
    [HideInInspector]
    public bool equipped;
    [HideInInspector]
    public bool pickedUp;
    [HideInInspector]
    public GameObject weaponManager;
    public Player player;
    public Text foodText;

    public bool playersWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(equipped)
        {

        }
    }
    
    public void ItemUsage()
    {
        //int perida = 3;
        //player.LoseFood(perida);
        foodText.text = "testing";
}
}
