using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hbdialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public float lineSpeed;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text= string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){

        foreach (char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);

            if(textComponent.text == lines[index]){
                StartCoroutine(DelayNextLine());
            }
        }

    }

    IEnumerator DelayNextLine(){
        yield return new WaitForSeconds(lineSpeed);
        NextLine();
    }

    void NextLine(){

        if(index < lines.Length - 1){

            index++;
            textComponent.text= string.Empty;
            StartCoroutine(TypeLine());

        }else{
            gameObject.SetActive(false);
        }

    }
}
