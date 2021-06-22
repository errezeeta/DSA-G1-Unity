using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject
{
    private Transform target;                           //Transform to attempt to move toward each turn.
    private bool skipMove;
    public int playerDamage;
    public Text dmgText;
    // Start is called before the first frame update
    void Start()
    {
        //Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
        //This allows the GameManager to issue movement commands.
        GameManager.instance.AddEnemyToList(this);

        //Find the Player GameObject using it's tag and store a reference to its transform component.
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Call the start function of our base class MovingObject.
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Check if skipMove is true, if so set it to false and skip this turn.
        if (skipMove)
        {
            skipMove = false;
            return;

        }

        //Call the AttemptMove function from MovingObject.
        base.AttemptMove<T>(xDir, yDir);

        //Now that Enemy has moved, set skipMove to true to skip next move.
        skipMove = true;
    }

    public void MoveEnemy()
    {
        //Declare variables for X and Y axis move directions, these range from -1 to 1.
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        int xDir = 0;
        int yDir = 0;

        //If the difference in positions is approximately zero (Epsilon) do the following:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
            yDir = target.position.y > transform.position.y ? 1 : -1;

        //If the difference in positions is not approximately zero (Epsilon) do the following:
        else
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = target.position.x > transform.position.x ? 1 : -1;

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        //Declare hitPlayer and set it to equal the encountered component.
        Player hitPlayer = component as Player;
        dmgText.text = "test";
        //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
        hitPlayer.LoseFood(playerDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
