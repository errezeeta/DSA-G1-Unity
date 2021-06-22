using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public BoardManager boardScript;
    public int level = 1;
    public static GameManager instance = null;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;
    public int playerDmgPoints = 100;
    public int playerDefPoints = 100;
    private Text DMGtext;
    private Text DEFtext;
    private Text Htext;
    private GameObject levelImage;
    private Text levelText;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup = true;
    [HideInInspector] public bool playersTurn = true;
    private Player player;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }

    //private void OnLevelWasLoaded (int index)
    //////  InitGame();

    //}

    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        levelText.text = "Després de " + level + " setmanes, has suspés!";

        //Enable black background image gameObject.
        levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }

    void InitGame()
    {
        doingSetup = true;

        //Get a reference to our image LevelImage by finding it by name.
        levelImage = GameObject.Find("LevelImage");

        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "SEMANA " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();
        boardScript.SetupScene(level);
    }
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);

        //Set doingSetup to false allowing player to move again.
        doingSetup = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if (playersTurn || enemiesMoving || doingSetup)

            //If any of these are true, return and do not start MoveEnemies.
            return;

        //Start moving enemies.
        StartCoroutine(MoveEnemies());

    }

    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies.
        enemies.Add(script);
    }
    public void Change(string typo, int a, int b)
    {
        if (typo == "DMG")
            ChangeDMG(a);
        else if (typo == "DEF")
            ChangeDEF(a);
        else if (typo == "H")
            ChangeH(a);
        else if (typo== "DMGDEF")
        {
            ChangeDMG(a);
            ChangeDEF(b);
        }
        else if (typo== "DMGH")
        {
            ChangeDMG(a);
            ChangeH(b);
        }
        else
        {
            ChangeDEF(a);
            ChangeH(b);

        }
    }
    public void ChangeDMG(int buf)
    {
        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
        DMGtext = GameObject.Find("DmgText").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        string texto = DMGtext.text;
        //Set the text of levelText to the string "Day" and append the current level number.
        DMGtext.text = "Attk: " + player.dmg + " + " + buf;
        player.dmg += buf;
        playerDmgPoints = player.dmg;
    }

    public void ChangeDEF(int buf)
    {
        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
        DEFtext = GameObject.Find("DefText").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        string texto = DEFtext.text;
        //Set the text of levelText to the string "Day" and append the current level number.
        DEFtext.text = "Def: " + player.def + " + " + buf;
        
        player.def += buf;
        playerDefPoints = player.def;
    }

    public void ChangeH(int buf)
    {
        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
        Htext = GameObject.Find("FoodText").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        //Set the text of levelText to the string "Day" and append the current level number.
        Htext.text = "NOTA: " + player.food + " + " + buf;
        player.food += buf;
        playerFoodPoints = player.food;

    }
    IEnumerator MoveEnemies()
    {
        //While enemiesMoving is true player is unable to move.
        enemiesMoving = true;

        //Wait for turnDelay seconds, defaults to .1 (100 ms).
        yield return new WaitForSeconds(turnDelay);

        //If there are no enemies spawned (IE in first level):
        if (enemies.Count == 0)
        {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
            yield return new WaitForSeconds(turnDelay);
        }

        //Loop through List of Enemy objects.
        for (int i = 0; i < enemies.Count; i++)
        {
            //Call the MoveEnemy function of Enemy at index i in the enemies List.
            enemies[i].MoveEnemy();

            //Wait for Enemy's moveTime before moving next Enemy, 
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        //Once Enemies are done moving, set playersTurn to true so player can move.
        playersTurn = true;

        //Enemies are done moving, set enemiesMoving to false.
        enemiesMoving = false;
    }
}
