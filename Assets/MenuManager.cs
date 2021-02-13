using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Text tournamentText;

    [SerializeField]
    private Text gemText;

    
    [Tooltip("Add levels in order of progression")]
    [SerializeField]
    private Image[] progressionSprites;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Color completedLevel = new Color(1, 1, 0);

    [SerializeField]
    private Color currentLevel = new Color(0,1,0);

    [SerializeField]
    private Color lockedLevel =  new Color(0.3f, 0.3f, 0.3f);





    private int tournament;
    private int gems;
    private int stage;

    private AsyncOperation sceneLoad;

    // Start is called before the first frame update
    private void Awake()
    {
        tournament = PlayerPrefs.GetInt("Tournament", 1);
        stage = PlayerPrefs.GetInt("Stage", 1);
        gems = PlayerPrefs.GetInt("Gems", 0);

        tournamentText.text = "Tournament #" + tournament;
        gemText.text = gems.ToString();

        for(int i = 0; i<progressionSprites.Length; i++)
        {
            if(i<stage-1)
            {
                progressionSprites[i].color = completedLevel;
            }
            else if (i == stage-1)
            {
                progressionSprites[i].color = currentLevel;
            }
            else
            {
                progressionSprites[i].color = lockedLevel;
            }
        }

        exitButton.onClick.AddListener(Exit);
        playButton.onClick.AddListener(Play);
        
    }

    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        sceneLoad = SceneManager.LoadSceneAsync("Playfield", LoadSceneMode.Single);
        sceneLoad.allowSceneActivation = false;
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
    }


    private void Play()
    {
        sceneLoad.allowSceneActivation = true;
    }
    private void Exit()
    {
        Application.Quit();
    }
}
