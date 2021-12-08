using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWarning : MonoBehaviour
{
    int waitTime;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = 5;
        StartCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
