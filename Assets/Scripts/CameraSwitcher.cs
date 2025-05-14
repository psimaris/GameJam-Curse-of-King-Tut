using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour {
    public GameObject mainCamera;
    public GameObject doorCamera;
    public float duration = 1.5f;

    public void ShowDoorView() {
        StartCoroutine(SwitchToDoorCamera());
    }

    IEnumerator SwitchToDoorCamera() {
        mainCamera.SetActive(false);
        doorCamera.SetActive(true);

        yield return new WaitForSeconds(duration);

        doorCamera.SetActive(false);
        mainCamera.SetActive(true);
    }
}