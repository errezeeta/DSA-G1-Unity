using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bp : MonoBehaviour, IPointerClickHandler
{
    private Inventory player;
    private Text test;
    public int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        player = GameObject.Find("Player").GetComponent<Inventory>();
        player.Open();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
