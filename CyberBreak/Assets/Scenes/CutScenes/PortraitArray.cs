using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitArray : MonoBehaviour
{
    public GameObject[] characterImages;
    int index;
    void Start()
    {
        index = 0;
    }

    void Update()
    {
        if(index >= 5)
        index = 5;
        if(index < 0)
        index = 0;

        if(index == 0)
        {
            characterImages[0].gameObject.SetActive(true);
        }
    }

    public void Next()
    {
        index += 1;

        for(int i = 0; i < characterImages.Length; i++)
        {
            characterImages[i].gameObject.SetActive(false);

            if (index < characterImages.Length)
            {
                characterImages[index].gameObject.SetActive(true);
            }
        }
    }
}
