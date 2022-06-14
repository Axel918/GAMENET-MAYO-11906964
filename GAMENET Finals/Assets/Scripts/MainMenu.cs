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

    [Header("Tutorial Panels")]
    public GameObject[] tutorialPages;

    [Header("Credits Panels")]
    public GameObject[] creditsPages;

    private int tutorialPageNumber;
    private int creditsPageNumber;

    private void Start()
    {
        ActivatePanel(mainMenuPanel);
        tutorialPageNumber = 0;
        creditsPageNumber = 0;
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
        TutorialPanel(tutorialPages[0]);
    }

    public void OnCreditsButtonClicked()
    {
        Debug.Log("Credits");
        ActivatePanel(creditsPanel);
    }

    public void OnCreditsReturnButtonClicked()
    {
        ActivatePanel(mainMenuPanel);
        CreditsPanel(creditsPages[0]);
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

    public void TutorialPanel(GameObject chosenPanel)
    {
        tutorialPages[0].SetActive(chosenPanel.Equals(tutorialPages[0]));
        tutorialPages[1].SetActive(chosenPanel.Equals(tutorialPages[1]));
    }

    public void CreditsPanel(GameObject chosenPanel)
    {
        creditsPages[0].SetActive(chosenPanel.Equals(creditsPages[0]));
        creditsPages[1].SetActive(chosenPanel.Equals(creditsPages[1]));
    }

    // Tutorial
    public void TutorialPrevious()
    {
        tutorialPageNumber--;

        if (tutorialPageNumber < 0)
        {
            tutorialPageNumber = 0;
        }

        TutorialPanel(tutorialPages[tutorialPageNumber]);
    }

    public void TutorialNext()
    {
        tutorialPageNumber++;

        if (tutorialPageNumber >= tutorialPages.Length)
        {
            tutorialPageNumber = tutorialPages.Length - 1;
        }

        TutorialPanel(tutorialPages[tutorialPageNumber]);
    }

    // Credits
    public void CreditsPrevious()
    {
        creditsPageNumber--;

        if (creditsPageNumber < 0)
        {
            creditsPageNumber = 0;
        }

        CreditsPanel(creditsPages[creditsPageNumber]);
    }

    public void CreditsNext()
    {
        creditsPageNumber++;

        if (creditsPageNumber >= creditsPages.Length)
        {
            creditsPageNumber = creditsPages.Length - 1;
        }

        CreditsPanel(creditsPages[creditsPageNumber]);
    }
}
