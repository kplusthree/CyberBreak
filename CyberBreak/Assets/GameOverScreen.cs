using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void Setup(bool endLevel)
    {
        if (endLevel == true);
        {
           gameObject.SetActive(true);
        }
        
    }
}
