using System.Collections;
using UnityEngine;

public class TalkingPeople : MonoBehaviour
{
    private Animator animator;
    public float delayBetweenTalking = 30;
    public AudioSource whispers;
    public AudioSource shush;

    private bool playerForcedIdle = false; // Tracks if the player has forced the character to idle
    private float volumeDecay = 0.002f;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        
        // Start the character in the talking state
        animator.SetBool("Talking", true);
        
        whispers.Play();
        whispers.loop = true;
    }

    void Update()
    {
        if (GameManager.Instance.playerCameraGameobject != null)
        {
            float yRotation = GameManager.Instance.playerCameraGameobject.gameObject.transform.eulerAngles.y;
            if (yRotation > 180f)
            {
                yRotation -= 360f;
            }

            if (GameManager.Instance.soundLvlFromPlayer == 1) //&& yRotation <= -140f && yRotation >= -220f
            {
                //Ændre til SHHHHH fra player
                GoIdle();
                if (shush.isPlaying == false) shush.Play();
            }

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

    // Function to be called when the player forces the character to idle
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

    // Coroutine to resume talking after a random delay
    private IEnumerator ResumeTalkingAfterDelay()
    {
        // Wait for a random amount of time (20-30 seconds)
        yield return new WaitForSeconds(delayBetweenTalking);

        // Resume talking state
        GameManager.Instance.talkingPeople = 1;
        playerForcedIdle = false;
        animator.SetBool("Talking", true);
    }
}
