using System.Collections;
using UnityEngine;

public class TalkingPeople : MonoBehaviour
{
    //animator til personerne
    private Animator animator;
    //hvor langt tid der skal gå fra idle til talking
    public float delayBetweenTalking = 30;

    //lyd sources
    public AudioSource whispers;
    public AudioSource shush;

    //bruges til personer der taller når de skal være idle
    private bool playerForcedIdle = false; 
    //lyd neivau
    private float volumeDecay = 0.002f;

    void Start()
    {
        //Animator
        animator = GetComponent<Animator>();
        
        // Start talking state
        animator.SetBool("Talking", true);
        
        //starter lyden
        whispers.Play();
        whispers.loop = true;
    }

    void Update()
    {
        //plan om at man skulle bruge camera fra spilleren til at se om man sagde shhh mens man så på dem
        if (GameManager.Instance.playerCameraGameobject != null)
        {
            float yRotation = GameManager.Instance.playerCameraGameobject.gameObject.transform.eulerAngles.y;
            if (yRotation > 180f)
            {
                yRotation -= 360f;
            }

            //bruges til at check om man har sagt shhh
            if (GameManager.Instance.soundLvlFromPlayer == 1) //&& yRotation <= -140f && yRotation >= -220f
            {
                //Ændre til SHHHHH fra player
                GoIdle();
                if (shush.isPlaying == false) shush.Play();
            }
            //gør fade på volume enten op eller ned
            if (playerForcedIdle == true && whispers.volume > 0.0f)
            {
                whispers.volume -= volumeDecay;
            }
            else if (playerForcedIdle == false && whispers.volume < 1.0f)
            {
                whispers.volume += volumeDecay;
            }
        }

        
    }

    //Når man siger shhh bliver de tvunget stille
    public void GoIdle()
    {
        if (!playerForcedIdle)
        {
            GameManager.Instance.talkingPeople = 0;
            playerForcedIdle = true; // Set the idle flag
            animator.SetBool("Talking", false); // Switch to idle animation

            // Start the coroutine to resume talking after a random delay
            StartCoroutine(ResumeTalkingAfterDelay());
        }
    }

    //Start talking efter lidt tid 30 sec ca
    private IEnumerator ResumeTalkingAfterDelay()
    {

        yield return new WaitForSeconds(delayBetweenTalking);

        //starter talking state igen
        GameManager.Instance.talkingPeople = 1;
        playerForcedIdle = false;
        animator.SetBool("Talking", true);
    }
}
