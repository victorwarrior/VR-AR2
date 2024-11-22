using System.Collections;
using UnityEngine;

public class TalkingPeople : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;
    public float delayBetweenTalking = 30;

    private bool playerForcedIdle = false; // Tracks if the player has forced the character to idle

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Start the character in the talking state
        animator.SetBool("Talking", true);
    }

    void Update()
    {
        // Check for player input (example: Space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Ændre til SHHHHH fra player
            GoIdle();
        }
    }

    // Function to be called when the player forces the character to idle
    public void GoIdle()
    {
        if (!playerForcedIdle)
        {
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
        playerForcedIdle = false;
        animator.SetBool("Talking", true);
    }
}
