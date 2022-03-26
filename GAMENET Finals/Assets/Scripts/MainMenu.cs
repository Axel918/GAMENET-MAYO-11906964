using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        ActivatePanel(mainMenuPanel);
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("LobbyScene");
    }

    public void OnTutorialButtonClicked()
    {
        Debug.Log("Tutorial");
        ActivatePanel(tutorialPanel);
    }

    public void OnTutorialReturnButtonClicked()
    {
        ActivatePanel(mainMenuPanel);
    }

    public void OnCreditsButtonClicked()
    {
        Debug.Log("Credits");
        ActivatePanel(creditsPanel);
    }

    public void OnCreditsReturnButtonClicked()
    {
        ActivatePanel(mainMenuPanel);
    }

    public void OnQuitGameButtonClicked()
    {
        Debug.Log("You Quit the Game!");
        Application.Quit();
    }

    public void ActivatePanel(GameObject chosenPanel)
    {
        mainMenuPanel.SetActive(chosenPanel.Equals(mainMenuPanel));
        tutorialPanel.SetActive(chosenPanel.Equals(tutorialPanel));
        creditsPanel.SetActive(chosenPanel.Equals(creditsPanel));
    }
}
