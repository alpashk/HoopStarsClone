using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour
{
    private List<GameObject> objectsToPool;
    private readonly Vector3 SpawnPos = new Vector3(0f, -10f, 0f);
    private int stage;

    [SerializeField]
    private GameObject ordinarySphere;

    [SerializeField]
    private GameObject gemSphere;

    [SerializeField]
    private int amountToCreate =2;

    private void Awake()
    {

        objectsToPool = new List<GameObject>();
        stage = 5;//PlayerPrefs.GetInt("Stage", 1);

        GameObject objectToCreate;

        if (stage == 5)
        {
            objectToCreate = gemSphere;
        }
        else
        {
            objectToCreate = ordinarySphere;
        }
        for (int i = 0; i < amountToCreate; i++)
        {
            GameObject addedObject = Instantiate(objectToCreate, SpawnPos, Quaternion.identity);
            addedObject.SetActive(false);
            objectsToPool.Add(addedObject);
        }
    }


    public GameObject PoolObject()
    {
        for (int i = 0; i < amountToCreate; i++)
        {
            if (!objectsToPool[i].activeInHierarchy)
            {
                return objectsToPool[i];
            }
        }


        //—юда не должно дойти

        GameObject objectToCreate;

        if (stage == 5)
        {
            objectToCreate = gemSphere;
        }
        else
        {
            objectToCreate = ordinarySphere;
        }
        GameObject addedObject = Instantiate(objectToCreate, SpawnPos, Quaternion.identity);
        addedObject.SetActive(false);
        objectsToPool.Add(addedObject);
        return addedObject;

    }
}
