using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    //public AudioSource TypeSound;
    public Text nameText;
    public Text DialogueText;
    private Queue<string> Sentences;

    public Animator animator;

    //void Start()
    //{   
    //    TypeSound = GetComponent<AudioSource>();
    //}
    private void Awake()
    {
        Sentences = new Queue<string>();
    }
public void StartDialogue(Dialogue dialogue)
    {    
        nameText.text = dialogue.name;
        Sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            Sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (Sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = Sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
    //    TypeSound.Play();
 // PLAYS SOUND AT THE START
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }
     //   TypeSound.Stop();
 // STOPS THE SOUND AS SOON AS THE TEXT STOPS ANIMATING
        
    }

    void EndDialogue()
    {
       SceneManager.LoadScene("Courtyard");

       //Debug.Log("End");
    }
}