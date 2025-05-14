using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenu;    
    public GameObject instructionsText; 
    public Button startButton;     
    public Button exitButton;       

    
    void Start() {
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        instructionsText.SetActive(true); // Show the instructions text
    }

    private void OnStartButtonClicked() {
        startMenu.SetActive(false);   // Hide the menu
        SceneManager.LoadScene("Mt_Egypt");  // Load the main game scene 
    }

    private void OnExitButtonClicked() {
        Application.Quit(); 
    }
}