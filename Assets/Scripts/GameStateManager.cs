using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager _instance;
    public GameStates gamestate = GameStates.Menu;
    public int numberOfPlayers = 1;
    public GameObject pauseMenu, mainMenu, levelMenu, skinMenu, instrMenu;
    private bool paused = false;

    public GameObject canvas;
    public GameObject[] planes;
    public GameObject walls;

    public GameObject[] lives;

    public PlayerMovement player1, player2;

    public RuntimeAnimatorController[] skins;

    public GameObject[] bosses;

    public GameObject[] powers;

    private bool dropReady = true; //false?

    public Text textPlayerNumber;

    public GameObject player1SkinSelected, player2SkinSelected;

    private PlayerMovement currentPlayer;
    public Text textSkinPlayerNumber;
    public GameObject skinChangeButton;

    public GameObject[] playerSkinsButtons;

    public void Start()
    {
        _instance = this;

        player1.SetPlayerNumber(1);
        player2.SetPlayerNumber(2);

        currentPlayer = player1;

        BackToMenu();
    }

    public static GameStateManager Instance
    {

        get
        {
            if (_instance == null)
                _instance = new GameStateManager();

            return _instance;

        }

    }


    public void ChangeMode()
    {

        if (numberOfPlayers == 1)
        {
            numberOfPlayers = 2;
            textPlayerNumber.text = "2 P";


            if (player2SkinSelected.transform.parent.Equals(player1SkinSelected.transform.parent) &&
                numberOfPlayers == 2)
            {
                if (player1.animator.runtimeAnimatorController.Equals(skins[0]))
                {
                    player2.animator.runtimeAnimatorController = skins[1];
                    player2SkinSelected.transform.parent = playerSkinsButtons[1].transform;
                    player2SkinSelected.transform.position = playerSkinsButtons[1].transform.position;
                }
                else
                {
                    player2.animator.runtimeAnimatorController = skins[0];
                    player2SkinSelected.transform.parent = playerSkinsButtons[0].transform;
                    player2SkinSelected.transform.position = playerSkinsButtons[0].transform.position;
                }

            }


        }
        else
        {
            numberOfPlayers = 1;
            textPlayerNumber.text = "1 P";
        }

    }



    public void PauseGame()
    {
        paused = !paused;

        if (paused)
        {


            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            gamestate = GameStates.Paused;
        }
        else
        {

            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            gamestate = GameStates.Gameplay;
        }
    }

    public void LevelSelect()
    {
        Clear();
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
        skinMenu.SetActive(false);

    }

    public void ChangePlayerForSkinChange()
    {
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
            textSkinPlayerNumber.text = "PLAYER 2";
        }
        else
        {
            currentPlayer = player1;
            textSkinPlayerNumber.text = "PLAYER 1";
        }
    }

    public void SkinSelect()
    {
        currentPlayer = player1;
        textSkinPlayerNumber.text = "PLAYER 1";


        if (numberOfPlayers == 2)
        {
            player2SkinSelected.SetActive(true);
            skinChangeButton.SetActive(true);
        }
        else
        {
            player2SkinSelected.SetActive(false);
            skinChangeButton.SetActive(false);
        }

        skinMenu.SetActive(true);
        levelMenu.SetActive(false);



    }


    public void ApplySkin(int sk)
    {
        currentPlayer.animator.runtimeAnimatorController = skins[sk];

        if (currentPlayer == player1)
        {

            player1SkinSelected.transform.parent = playerSkinsButtons[sk].transform;
            player1SkinSelected.transform.position = playerSkinsButtons[sk].transform.position;


            if (player2SkinSelected.transform.parent.Equals(player1SkinSelected.transform.parent) &&
                numberOfPlayers == 2)
            {
                if (sk == 0)
                {
                    player2.animator.runtimeAnimatorController = skins[1];
                    player2SkinSelected.transform.parent = playerSkinsButtons[1].transform;
                    player2SkinSelected.transform.position = playerSkinsButtons[1].transform.position;
                }
                else
                {
                    player2.animator.runtimeAnimatorController = skins[0];
                    player2SkinSelected.transform.parent = playerSkinsButtons[0].transform;
                    player2SkinSelected.transform.position = playerSkinsButtons[0].transform.position;
                }

            }


        }
        else
        {
            player2SkinSelected.transform.parent = playerSkinsButtons[sk].transform;
            player2SkinSelected.transform.position = playerSkinsButtons[sk].transform.position;

            if (player2SkinSelected.transform.parent.Equals(player1SkinSelected.transform.parent))
            {
                if (sk == 0)
                {
                    player1.animator.runtimeAnimatorController = skins[1];
                    player1SkinSelected.transform.parent = playerSkinsButtons[1].transform;
                    player1SkinSelected.transform.position = playerSkinsButtons[1].transform.position;
                }
                else
                {
                    player1.animator.runtimeAnimatorController = skins[0];
                    player1SkinSelected.transform.parent = playerSkinsButtons[0].transform;
                    player1SkinSelected.transform.position = playerSkinsButtons[0].transform.position;
                }

            }

        }



    }

    private void Clear()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject g in go)
        {
            g.SetActive(false);
            Destroy(g);
        }

        go = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject g in go)
        {
            g.SetActive(false);
            Destroy(g);
        }

        go = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject g in go)
        {
            g.SetActive(false);
            Destroy(g);
        }

        go = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject g in go)
        {
            g.SetActive(false);
            Destroy(g);
        }

    }

    public void StartGame(int p)
    {
        Clear();

        player1.Reset();

        if (numberOfPlayers == 2)
            player2.Reset();

        lives[0].SetActive(true);
        lives[1].SetActive(true);
        lives[2].SetActive(true);


        if (numberOfPlayers == 2)
        {
            lives[3].SetActive(true);
            lives[4].SetActive(true);
            lives[5].SetActive(true);
        }



        paused = false;
        Time.timeScale = 1;

        mainMenu.SetActive(false);
        levelMenu.SetActive(false);
        planes[p].SetActive(true);

        player1.transform.gameObject.SetActive(true);
        player1.transform.position = new Vector2(0, -8f);


        player1.Lock();

        if (numberOfPlayers == 2)
        {
            player2.transform.gameObject.SetActive(true);
            player2.transform.position = new Vector2(4, -8f);


            player2.Lock();
        }

        walls.transform.gameObject.SetActive(false);
        StartCoroutine(Advance());

        if (p == 0)
        {
            //i=0-3
            GameObject fireGemBoss = Instantiate(bosses[0]);
            StartCoroutine(fireGemBoss.GetComponent<FireGemBoss>().BattleBegin());


            GameObject waterGemBoss = Instantiate(bosses[1]);
            StartCoroutine(waterGemBoss.GetComponent<WaterGemBoss>().BattleBegin());

            GameObject airGemBoss = Instantiate(bosses[2]);
            StartCoroutine(airGemBoss.GetComponent<AirGemBoss>().BattleBegin());

            GameObject earthGemBoss = Instantiate(bosses[3]);
            StartCoroutine(earthGemBoss.GetComponent<EarthGemBoss>().BattleBegin());

        }



        gamestate = GameStates.Gameplay;

    }

    IEnumerator Advance()
    {
        walls.transform.gameObject.SetActive(false);

        while (player1.transform.position.y <= -3.8)
        {
            player1.transform.position = new Vector2(player1.transform.position.x,
                  player1.transform.position.y + 0.1f);

            if (numberOfPlayers == 2)
                player2.transform.position = new Vector2(player2.transform.position.x,
                  player2.transform.position.y + 0.1f);

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        walls.transform.gameObject.SetActive(true);

        // yield return new WaitForSeconds(1f);

        player1.Unlock();
        if (numberOfPlayers == 2)
            player2.Unlock();
        //dropReady = true;
    }


    IEnumerator Advance2()
    {
        walls.transform.gameObject.SetActive(false);
        player1.Lock();
        player1.movement.x = 0;
        player1.movement.y = 0;

        if (numberOfPlayers == 2)
        {
            player2.Lock();
            player2.movement.x = 0;
            player2.movement.y = 0;
        }


        yield return new WaitForSeconds(2f);

        while (player1.transform.position.y <= 5.8)
        {
            player1.transform.position = new Vector2(player1.transform.position.x,
                  player1.transform.position.y + 0.1f);

            if (numberOfPlayers == 2)
                player2.transform.position = new Vector2(player2.transform.position.x,
                  player2.transform.position.y + 0.1f);

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);


        BackToMenu();


    }

    public void Revive()
    {
        currentPlayer = null;

        if (player1.transform.gameObject.active == false)
        {
            currentPlayer = player1;

            currentPlayer.transform.position = new Vector2(0, -8f);
        }

        if (player2.transform.gameObject.active == false)
        {
            currentPlayer = player2;

            currentPlayer.transform.position = new Vector2(4, -8f);
        }


        if (currentPlayer != null)
        {
            currentPlayer.HP = 1;

            currentPlayer.transform.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            currentPlayer.ReviveInvincibility1();

            currentPlayer.Lock();
            currentPlayer.transform.gameObject.SetActive(true);

            StartCoroutine(ReviveAdvance());
        }

    }


    private IEnumerator ReviveAdvance()
    {

        while (currentPlayer.transform.position.y <= -3.8)
        {
            currentPlayer.transform.position = new Vector2(currentPlayer.transform.position.x,
                  currentPlayer.transform.position.y + 0.1f);

            yield return new WaitForSeconds(0.05f);
        }

        currentPlayer.transform.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


        StartCoroutine(currentPlayer.ReviveInvincibility2());

        currentPlayer.Unlock();
    }

    IEnumerator PrepareDrop()
    {
        float time = Random.Range(200, 300) / 10f;

        //time = 2f;

        yield return new WaitForSeconds(time);

        dropReady = true;
    }

    public void BackToMenu()
    {
        StopAllCoroutines();
        dropReady = true;
        /*StopCoroutine(Advance());
        StopCoroutine(Advance2());
        StopCoroutine(ReviveAdvance());
        */

        //dropReady = false;
        lives[0].SetActive(false);
        lives[1].SetActive(false);
        lives[2].SetActive(false);

        lives[3].SetActive(false);
        lives[4].SetActive(false);
        lives[5].SetActive(false);

        Clear();
        player1.transform.gameObject.SetActive(false);

        if (numberOfPlayers == 2)
        {
            player2.transform.gameObject.SetActive(false);
        }
        walls.transform.gameObject.SetActive(false);

        gamestate = GameStates.Menu;
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        skinMenu.SetActive(false);
        instrMenu.SetActive(false);

        for (int i = 0; i < planes.Length; i++)
            planes[i].SetActive(false);

    }

    public void GameOptions()
    {
        //to add
        //Debug.Log("yup");
        mainMenu.SetActive(false);
        instrMenu.SetActive(true);


    }

    public void QuitGame()
    {
        Application.Quit();

    }




    private void Update()
    {
        if (gamestate.Equals(GameStates.Gameplay))
        {
            for (int i = 0; i < player1.GetHP(); i++)
                lives[i].SetActive(true);

            for (int i = 2; i >= player1.GetHP(); i--)
                lives[i].SetActive(false);

            if (numberOfPlayers == 2)
            {
                for (int i = 0; i < player2.GetHP(); i++)
                    lives[i + 3].SetActive(true);

                for (int i = 2; i >= player2.GetHP(); i--)
                    lives[i + 3].SetActive(false);

            }




            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                StartCoroutine(Advance2());
            else
            {
                if (dropReady)
                {
                    dropReady = false;

                    int pow = Random.Range(0, 4);

                    if (pow == 3 && numberOfPlayers == 2
                        && GameObject.FindGameObjectsWithTag("Player").Length == 1)
                        pow = 4;

                    GameObject power = Instantiate(powers[pow]);

                    float pos = Random.Range(-70, 70) / 10f;
                    power.transform.position = new Vector2(pos, 7f);
                    StartCoroutine(PrepareDrop());
                }

                if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
                    BackToMenu();
            }


        }


        if (Input.GetButtonDown("Pause") && !gamestate.Equals(GameStates.Menu))
        {
            PauseGame();

        }




    }
}
