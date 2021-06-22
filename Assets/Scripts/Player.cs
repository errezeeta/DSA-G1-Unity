using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int pointsPerFood = 10;
    public int pointsPerDmg = 10;
    public int pointsPerDef = 10;
    public int wallDamage = 1;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public Text defText;
    public Text dmgText;
    private Animator animator;
    public int food;
    public int dmg;
    public int def;
    private Vector2 touchOrigin = -Vector2.one;
    public int able;
    // Start is called before the first frame update
    protected override void Start()
    {
        //Get the current food point total stored in GameManager.instance between levels.
        food = GameManager.instance.playerFoodPoints;
        dmg = GameManager.instance.playerDmgPoints;
        def = GameManager.instance.playerDefPoints;
        foodText.text = "NOTA: " + food;
        dmgText.text = "Attk: " + dmg;
        defText.text = "Def: " + def;
        able = 1;
        base.Start();
    }

    private void OnDisable()
    {
        //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
        GameManager.instance.playerFoodPoints = food;
        GameManager.instance.playerDmgPoints = dmg;
        GameManager.instance.playerDefPoints = def;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (able=true)
        {
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));
            if (horizontal != 0)
            {
                vertical = 0;
            }
        }
#else
        if (able == 1)
        {
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];
                if (myTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = myTouch.position;
                }
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;
                else
                    vertical = y > 0 ? 1 : -1;
                }
            }
        }
#endif
        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }
    }
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Every time player moves, subtract from food points total.
        food--;
        foodText.text = "NOTA: " + food;
        defText.text = "Def: " + def;
        dmgText.text = "Dmg: " + dmg;
        //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);

        //Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;

        //Since the player has moved and lost food points, check if the game has ended.
        CheckIfGameOver();

        //Set the playersTurn boolean of GameManager to false now that players turn is over.
        GameManager.instance.playersTurn = false;
    }

    private void CheckIfGameOver()
    {
        //Check if food point total is less than or equal to zero.
        if (food <= 0)
        {
            //Call the GameOver function of GameManager.
            GameManager.instance.GameOver();
        }
    }
    protected override void OnCantMove<T>(T component)
    {
        //Set hitWall to equal the component passed in as a parameter.
        Wall hitWall = component as Wall;

        //Call the DamageWall function of the Wall we are hitting.
        hitWall.DamageWall(wallDamage);

    }

    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
        //and not load all the scene object in the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);

            //Disable the player object since level is over.
            enabled = false;
        }

        //Check if the tag of the trigger collided with is Food.
        else if (other.tag == "Food")
        {
            //Add pointsPerFood to the players current food total.
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " NOTA: " + food;
            //Disable the food object the player collided with.
            other.gameObject.SetActive(false);
        }

        //Check if the tag of the trigger collided with is Soda.
        else if (other.tag == "Soda")
        {
            //Add pointsPerSoda to players food points total
            dmg+= pointsPerDmg;

            //Disable the soda object the player collided with.
            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Def")
        {
            //Add pointsPerSoda to players food points total
            def += pointsPerDef;

            //Disable the soda object the player collided with.
            other.gameObject.SetActive(false);
        }

    }

    public void LoseFood(int loss)
    {
        //Subtract lost food points from the players total.
        food -= loss;
        //Check to see if game has ended.
        foodText.text = "-" + loss + " NOTA: " + food;
        CheckIfGameOver();
    }

    
}
