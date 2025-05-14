using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float openDistance = 2f;
    public float openSpeed = 2f;

    public AudioSource doorAudioSource;  
    public AudioClip doorOpenSound;      

    private bool shouldOpen = false;
    private Vector3 leftStartPos, rightStartPos;

    void Start() {
        leftStartPos = leftDoor.localPosition;
        rightStartPos = rightDoor.localPosition;

        // Check if AudioSource and AudioClip are assigned
        if (doorAudioSource == null) {
            doorAudioSource = GetComponent<AudioSource>(); // Automatically get the AudioSource if not set
        }

        if (doorOpenSound == null) {
            Debug.LogWarning("Door Open Sound not assigned!");
        }
    }

    void Update() {
        if (shouldOpen) {
            // Slide doors open
            leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition,
                leftStartPos + Vector3.left * openDistance, openSpeed * Time.deltaTime);

            rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition,
                rightStartPos + Vector3.right * openDistance, openSpeed * Time.deltaTime);
        }
    }

    public void OpenDoor() {
        shouldOpen = true;

        if (doorAudioSource != null && doorOpenSound != null) {
            doorAudioSource.PlayOneShot(doorOpenSound);
        }
    }
}