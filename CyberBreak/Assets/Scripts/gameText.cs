using UnityEngine;
using System.Collections;
 
 public class gameText : MonoBehaviour
 {
     public float time = 5; //Seconds to read the text
 
     IEnumerator Start ()
     {
         yield return new WaitForSeconds(time);
         Destroy(gameObject);
     }
}
