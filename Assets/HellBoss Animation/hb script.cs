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
    public float spawnRate;
    public int projectileCount;
    private int index;
    private float projectileTimeCtr;

    public GameObject projectile;
    public Animator animator;
    public GameObject cameraShakeObject; 
    private CameraShakeTrigger cameraShake;


    // Start is called before the first frame update
    void Start()
    {
        cameraShake = cameraShakeObject.GetComponent<CameraShakeTrigger>();

        textComponent.text= string.Empty;
        StartDialogue();
        InvokeRepeating("SpawnProjectilesBunch", 2.0f, spawnRate);
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

    void StartProjectileCtr(){
        projectileTimeCtr += Time.deltaTime;
    }

    void SpawnProjectilesBunch(){
        cameraShake.ShakeCamera();
        animator.SetTrigger("isShouting");

        for(int i = 0; i<projectileCount; i++){

            Invoke("SpawnProjectile",Random.Range(0.1f, 2f));

        }

        projectileTimeCtr = 0;
        
    }

    void SpawnProjectile(){

        Instantiate(projectile, new Vector2(Random.Range(-15.0f, 15.0f),50f), Quaternion.identity);
        
    }
}
