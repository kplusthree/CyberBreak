using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    private float currentWaitTime = -1.0f;

    void Update()
    {
        if (currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime;
            return;
        }

        for (int i = 0; i < popUps.Length; i++)
        {
            if(i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }

        if (popUpIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                currentWaitTime = 2.0f;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentWaitTime = 2.0f;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentWaitTime = 2.0f;
                popUpIndex++;
            }
        }
    }
}
