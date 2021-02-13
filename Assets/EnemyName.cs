using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyName : MonoBehaviour
{
    [SerializeField] 
    private GameObject enemyText;
    [SerializeField] 
    private GameObject enemyFlag;

    private static string flagPath = @"flag";
    private static string namePath = @"PossibleNames";
    
    [Tooltip("The amount of possible flag. Every flag has t follow this naming style \"flag+number\" starting from 0")]
    [SerializeField]
    private int flagCount = 1;

    
    [Tooltip("How far the flag is from the text")]
    [SerializeField]
    private float flagOffset = 1;

    [SerializeField]
    private float yOffset = 1;



    private GameObject uiHolder;


    private void Awake()
    {
        GameObject textHolder;
        GameObject flagHolder;

        TextAsset nameFile = Resources.Load<TextAsset>(namePath);
        string[] names = nameFile.text.Split('\n');

        uiHolder = new GameObject("enemyUI");

        textHolder = Instantiate(enemyText, uiHolder.transform);
        textHolder.transform.position = new Vector3(0, yOffset, -1);
        TextMesh setupText = textHolder.GetComponent<TextMesh>();
        setupText.text = names[Random.Range(0, names.Length)];


        Vector3 meshBounds = textHolder.GetComponent<MeshRenderer>().bounds.extents;
        float totalFlagOffset = meshBounds.x + flagOffset;

        flagHolder = Instantiate(enemyFlag, uiHolder.transform);
        int flagNumber = Random.Range(0, flagCount);
        flagHolder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagPath + flagNumber);
        flagHolder.transform.position = new Vector3(totalFlagOffset, yOffset, -1);
    }


    private void FixedUpdate()
    {
        uiHolder.transform.position = transform.position;
    }
}
