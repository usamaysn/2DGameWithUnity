using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelload : MonoBehaviour
{

    public int iLevelToLoad;
    public string sLevelToLoad;

    public bool useIntegerToLoadLevel = false;


    void Start()
    {

    }


    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;


        if (collisionGameObject.name == "Player")
        {
            Invoke("LoadScene", 0.1f);
        }

    }


    void LoadScene()
    {

        if (useIntegerToLoadLevel)
        {

            SceneManager.LoadScene(iLevelToLoad);

        }

        else
        {

            SceneManager.LoadScene(sLevelToLoad);


        }


    }




}

