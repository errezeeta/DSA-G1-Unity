using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public Sprite dmgSprite;                    //Alternate sprite to display after Wall has been attacked by player.
	public int hp = 3;
	private SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Awake()
	{
		//Get a component reference to the SpriteRenderer.
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void DamageWall(int loss)
	{

		//Set spriteRenderer to the damaged wall sprite.
		spriteRenderer.sprite = dmgSprite;

		//Subtract loss from hit point total.
		hp -= loss;

		//If hit points are less than or equal to zero:
		if (hp <= 0)
			//Disable the gameObject.
			gameObject.SetActive(false);
	}
}
