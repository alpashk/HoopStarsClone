using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMaster : MonoBehaviour
{

    /*
        stage 1-3 = ordinary
        stage 4 = finals
        stage 5 = bonus gem level
    */

    [Tooltip("How many times do you need to score at first tournament")]
    [SerializeField] 
    private int minWins = 3;

    [Tooltip("Every N tournaments the requiered score is increased by 1")]
    [SerializeField] 
    private int minTournament = 3;

    [Tooltip("How much each picked up gem gives you")]
    [SerializeField] 
    private int gemValue = 3;

    [Tooltip("Timelimit for a level (in seconds)")]
    [SerializeField] 
    private int levelTimer = 60;

    [Tooltip("Timelimit for overtime (in seconds)")]
    [SerializeField]
    private int overtimeTimer = 15;

    [Tooltip("Time untill round end when a time limit alarm turns on")]
    [SerializeField] 
    private int lowTimeEffect = 5;

    [SerializeField] 
    private float minimalSlowMotionTime = 1.5f;
    [SerializeField] 
    private float slowMotionStrength = .4f;


    [SerializeField] 
    private GameObject playerModel;

    [SerializeField] 
    private GameObject enemyModel;

    [SerializeField] 
    private GameObject sphereModel;

    [SerializeField] 
    private GameObject crownModel;

    [SerializeField] 
    private GameObject almostWinParticleModel;

    [SerializeField] 
    private GameObject winParticleModel;


    [Tooltip("GameObject with a textMesh component that displays time")]
    [SerializeField] 
    private GameObject timer;    

    [SerializeField] 
    private GameObject scoreHolder;

    [SerializeField] 
    private GameObject inputField;

    [SerializeField] 
    private GameObject endgameUI;

    [SerializeField] 
    private GameObject lowTimeLight;

    [SerializeField]
    private Text endgameOutput;



    [SerializeField]
    private Vector3 playerStartPos;
    [SerializeField]
    private Vector3 enemyStartPos;
    [SerializeField]
    private Vector3 sphereStartPos;


    [Tooltip("Left top corner of an area where ball is allowed to spawn")]
    [SerializeField]
    private Vector2 leftTopSpawnCorner;
    [Tooltip("Right bottom corner of an area where ball is allowed to spawn")]
    [SerializeField] 
    private Vector2 rightBottomSpawnCorner;
    [Tooltip("Distance from player/enemy where balls are not allowed to spawn")]
    [SerializeField] 
    private float noSpawnRange = 2.0f;



    private int currentTime;

    private int tournament;
    private int stage;

    private int playerScore = 0;
    private int enemyScore = 0;
    private int maxScore;
    private bool alreadyLoading = false;

    private GameObject player;
    private GameObject enemy;
    private GameObject sphere;
    private GameObject crown;
    private SimpleObjectPool simplePool;
    private CrownFollow crownFollow;
    private TextMesh timerText;
    private ScoreHolderManager scoreHolderManager;
    private SphereBounce sphereBounce;
    private GameObject almostWinParticles;

    //sceneLoad
    private AsyncOperation sceneLoad;

    #region LevelCreation
    private void Awake()
    {
        tournament = PlayerPrefs.GetInt("Tournament", 1);
        stage = PlayerPrefs.GetInt("Stage", 1);
        Time.timeScale = 1;

        maxScore = minWins + tournament / minTournament;
        currentTime = levelTimer;
        simplePool = GetComponent<SimpleObjectPool>();
        timerText = timer.GetComponent<TextMesh>();
        scoreHolderManager = scoreHolder.GetComponent<ScoreHolderManager>();


        ScoreManager.OnScore += GameManager;
        TapToStart.OnStart += RoundStart;
    }

    private void Start()
    {
        if (stage != 5)
        {
            player = Instantiate(playerModel, playerStartPos, Quaternion.identity);
            enemy = Instantiate(enemyModel, enemyStartPos, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().enabled = false;

            crown = Instantiate(crownModel);
            crownFollow = crown.GetComponent<CrownFollow>();
            crown.SetActive(false);

            sphere = simplePool.PoolObject();
            sphere.transform.position = sphereStartPos;
            sphereBounce = sphere.GetComponent<SphereBounce>();
            sphere.SetActive(true);

            scoreHolderManager.InitializeBoard(maxScore);
        }
        else
        {
            player = Instantiate(playerModel, playerStartPos, Quaternion.identity);
            enemy = null;
            sphere = simplePool.PoolObject();
            sphere.transform.position = sphereStartPos;
            sphereBounce = sphere.GetComponent<SphereBounce>();
            sphere.SetActive(true);
            scoreHolderManager.InitializeGemBoard();
        }
        PlayerFollow.player = player.transform;
    }

    private void RoundStart()
    {
        inputField.SetActive(true);
        StartCoroutine(RoundTimer());
        if (enemy != null)
        {
            enemy.GetComponent<EnemyAI>().enabled = true;
        }
        TapToStart.OnStart -= RoundStart;
    }
    #endregion

    #region Level Control
    private IEnumerator RoundTimer()
    {
        while (currentTime > 0)
        {
            UiTimeUpdate();
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            if (currentTime == lowTimeEffect)
            {
                lowTimeLight.SetActive(true);
            }

        }
        UiTimeUpdate();
        EndOfTime();
    }

    private void UiTimeUpdate()
    {
        System.TimeSpan timeConversion = System.TimeSpan.FromSeconds(currentTime);
        string outputTime = timeConversion.ToString(@"mm\:ss");
        timerText.text = outputTime;

    }

    private void GameManager(bool player)
    {

        if (stage == 4)
        {
            RespawnBall();
        }
        if (stage != 5)
        {
            if (player)
            {
                playerScore++;
                scoreHolderManager.UpdatePlayerScore(playerScore);
            }
            else
            {
                enemyScore++;
                scoreHolderManager.UpdateEnemyScore(enemyScore);
            }
            WinCheck();
            EffectCheck();
        }
        else
        {
            playerScore += gemValue;
            scoreHolderManager.UpdatePlayerScore(playerScore);
            RespawnBall();

        }

    }

    private void RespawnBall()
    {
        GameObject nextSphere = simplePool.PoolObject();
        sphereBounce.KillSequence();
        sphere.SetActive(false);
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = Vector3.zero;
        if (enemy != null)
        {
            enemyPos = enemy.transform.position;
        }

        while (true)
        {
            Vector3 ballPosition = new Vector3(Random.Range(leftTopSpawnCorner.x, rightBottomSpawnCorner.x),
                Random.Range(rightBottomSpawnCorner.y, leftTopSpawnCorner.y));


            //Проверка на нахождение случайной позиции в квадрате с стороной в 2*noSpawnrange и центром в позиции игрока
            if (!(playerPos.x > ballPosition.x - noSpawnRange && playerPos.x < ballPosition.x + noSpawnRange
                && playerPos.y > ballPosition.y - noSpawnRange && playerPos.y < ballPosition.y + noSpawnRange))
                //Аналогичная проверка с центром квадрата в позиции противника
                if (!(enemy != null && enemyPos.x > ballPosition.x - noSpawnRange && enemyPos.x < ballPosition.x + noSpawnRange
                && enemyPos.y > ballPosition.y - noSpawnRange && enemyPos.y < ballPosition.y + noSpawnRange))
                {
                    sphere = nextSphere;
                    sphere.transform.position = ballPosition;
                    sphereBounce = sphere.GetComponent<SphereBounce>();

                    sphere.SetActive(true);

                    break;
                }
        }


    }

    private void LoadScene()
    {
        sceneLoad.allowSceneActivation = true;
    }
    #endregion


    #region Win and Effect Conditions
    private void WinCheck()
    {
        if (playerScore == maxScore)
        {
            Win();
        }
        else if (enemyScore == maxScore)
        {
            Lose();
        }
    }

    private void EffectCheck()
    {
        if (!crown.activeInHierarchy)
        {
            crown.SetActive(true);
        }
        if (playerScore > enemyScore)
        {
            crownFollow.target = player.transform;
        }
        else if (playerScore < enemyScore)
        {
            crownFollow.target = enemy.transform;
        }
        else
        {
            crown.SetActive(false);
        }

        if ((playerScore == maxScore - 1 || enemyScore == maxScore - 1) && almostWinParticles == null)
        {
            almostWinParticles = Instantiate(almostWinParticleModel, new Vector3(0, 10, 0), Quaternion.Euler(90, 0, 0));
        }
    }

    private void EndOfTime()
    {
        if (playerScore > enemyScore || stage == 5)
        {
            Win();
        }
        else if (enemyScore < playerScore)
        {
            Lose();
        }
        else
        {
            lowTimeLight.SetActive(false);
            currentTime = overtimeTimer;
            StartCoroutine(RoundTimer());
            //disable endoftime effect
        }
    }

    private void Win()
    {
        if (stage == 5)
        {
            SaveGems();
            tournament += 1;
            stage = 1;
        }
        else
        {
            stage++;
        }
        if (!alreadyLoading)
        {
            StartCoroutine(EndgameRoutine(true));
        }
    }

    private void Lose()
    {
        stage = 0;
        if (!alreadyLoading)
        {
            StartCoroutine(EndgameRoutine(false));
        }
    }


    #endregion

    #region Round end routines
    private IEnumerator EndgameRoutine(bool playerWin)
    {
        SaveGame();
        Destroy(almostWinParticles);
        Instantiate(winParticleModel, new Vector3(0, -3, 0), Quaternion.Euler(-90, 0, 0));
        lowTimeLight.SetActive(false);
        alreadyLoading = true;
        sphere.SetActive(false);
        endgameUI.SetActive(true);
        TapToStart.OnStart += LoadScene;
        inputField.SetActive(false);
        if (enemy != null)
        {
            Destroy(enemy.GetComponent<EnemyAI>());
        }
        Time.timeScale = slowMotionStrength;
        float slowedDownTime = minimalSlowMotionTime;
        sceneLoad = SceneManager.LoadSceneAsync("Endgame", LoadSceneMode.Single);
        sceneLoad.allowSceneActivation = false;
        if (playerWin)
        {
            endgameOutput.text = "You won";
        }
        else
        {
            endgameOutput.text = "You lost";
        }
        while (slowedDownTime > 0 || sceneLoad.progress < 0.9f)
        {
            yield return new WaitForSecondsRealtime(.1f);
            slowedDownTime -= .1f;
        }
        endgameOutput.text = "Tap to continue";
        while (!sceneLoad.isDone)
        {
            yield return null;
        }

    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("Tournament", tournament);
        PlayerPrefs.SetInt("Stage", stage);
    }

    private void SaveGems()
    {
        int gems = PlayerPrefs.GetInt("Gems", 0);
        gems += playerScore;
    }
    #endregion

    private void OnDestroy()
    {
        SaveGame();
        ScoreManager.OnScore -= GameManager;
        TapToStart.OnStart -= LoadScene;
    }

}
