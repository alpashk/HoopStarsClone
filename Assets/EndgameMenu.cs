using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameMenu : MonoBehaviour
{
    [SerializeField] 
    private Text tournamentText;

    [SerializeField] 
    private Text resultText;

    [Tooltip("Add levels in order of progression")]
    [SerializeField] 
    private Image[] progressionSprites;

    [SerializeField] 
    private Button playButton;

    [SerializeField] 
    private Button menuButton;

    [SerializeField] 
    private Text playButtonText;

    [SerializeField] 
    private Color completedLevel = new Color(1, 1, 0);

    [SerializeField] 
    private Color currentLevel = new Color(0, 1, 0);

    [SerializeField] 
    private Color lockedLevel = new Color(0.3f, 0.3f, 0.3f);





    private int tournament;
    private int stage;


    private void Awake()
    {
        tournament = PlayerPrefs.GetInt("Tournament", 1);
        stage = PlayerPrefs.GetInt("Stage", 1);
        if (stage == 0)
        {
            playButtonText.text = "Try again";
            resultText.text = "You lost";
            stage = 1;
            PlayerPrefs.SetInt("Stage", stage);
        }

        tournamentText.text = "Tournament #" + tournament;

        for (int i = 0; i < progressionSprites.Length; i++)
        {
            if (i < stage - 1)
            {
                progressionSprites[i].color = completedLevel;
            }
            else if (i == stage - 1)
            {
                progressionSprites[i].color = currentLevel;
            }
            else
            {
                progressionSprites[i].color = lockedLevel;
            }
        }



        menuButton.onClick.AddListener(Menu);
        playButton.onClick.AddListener(Play);
    }

    private void Play()
    {
        SceneManager.LoadScene("Playfield");
    }
    private void Menu()
    {
        SceneManager.LoadScene("Start");
    }
}
