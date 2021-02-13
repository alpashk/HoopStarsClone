using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreHolderManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerScoreHolder;
    [SerializeField]
    private GameObject playerScore;


    [SerializeField]
    private GameObject enemyScoreHolder;

    [SerializeField]
    private GameObject enemyScore;

    [SerializeField]
    private GameObject circleModel;

    [SerializeField]
    private Color circleFinalColor = new Color(1, 1, 0);

    [SerializeField]
    private Color highlightColor = new Color(.5f, 0, 0);

    [SerializeField]
    private float highlightDuration = .5f;

    private TextMesh playerText;
    private TextMesh enemyText;

    private MeshRenderer playerMesh;
    private MeshRenderer enemyMesh;


    private List<SpriteRenderer> playerSprites;
    private List<SpriteRenderer> enemySprites;

    private readonly float circleLeftX = -0.5f;
    private readonly float circleStep = .25f;
    private readonly float circleStartY = -0.6f;
    private readonly int circlesInLine = 5;

    private void Awake()
    {
        playerText = playerScore.GetComponent<TextMesh>();
        playerMesh = playerScore.GetComponent<MeshRenderer>();
        enemyText = enemyScore.GetComponent<TextMesh>();
        enemyMesh = enemyScore.GetComponent<MeshRenderer>();

    }


    public void InitializeBoard(int maxWins)
    {
        playerText.text = "0";
        enemyText.text = "0";

        playerSprites = new List<SpriteRenderer>();
        enemySprites = new List<SpriteRenderer>();

        for(int i = 0; i<maxWins; i++)
        {
            Vector3 position = new Vector3(circleLeftX + circleStep * (int)(i % circlesInLine), circleStartY - circleStep * 
                (int)(i / circlesInLine), -0.051f);

            GameObject circle = InstantiateCircle(playerScoreHolder.transform, position);
            playerSprites.Add(circle.GetComponent<SpriteRenderer>());

            circle = InstantiateCircle(enemyScoreHolder.transform, position);
            enemySprites.Add(circle.GetComponent<SpriteRenderer>());
        }
    }

    private GameObject InstantiateCircle(Transform parent, Vector3 position)
    {
        GameObject circle = Instantiate(circleModel, parent);
        circle.transform.localPosition = position;
        return circle;
    }

    public void InitializeGemBoard()
    {
        playerText.text = "0";
        enemyScoreHolder.SetActive(false);
    }

    public void UpdatePlayerScore(int value)
    {
        playerText.text = value.ToString();
        playerText.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
        playerMesh.material.color = highlightColor;
        playerMesh.material.DOColor(Color.white, highlightDuration);
        if (playerSprites !=null && playerSprites.Count > value-1)
        {
            playerSprites[value - 1].color = circleFinalColor;
        }
    }

    public void UpdateEnemyScore(int value)
    {
        enemyText.text = value.ToString();
        enemyMesh.material.color = highlightColor;
        enemyMesh.material.DOColor(Color.white, highlightDuration);
        if (enemySprites.Count > value-1)
        {
            enemySprites[value - 1].color = circleFinalColor;
        }
    }




}
