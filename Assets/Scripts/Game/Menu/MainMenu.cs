using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    enum MenuState
    {
        MAIN,
        PLAY,
        CONNECTION,
        OPTIONS
    };

    public RectTransform mainPanel;
    public RectTransform playPanel;
    public RectTransform connectionPanel;
    public RectTransform optionsPanel;

    // Main state
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;

    // Play state
    public Button connectButton;
    public Button tryHostButton;

    // Connection state
    public InputField ipAddressField;
    public Button tryConnectionButton;

    private MenuState currentMenuState;
    private Text exitButtonText;

    private void Start()
    {
        Destroy(GameObject.Find("network_manager"));

        exitButtonText = exitButton.GetComponentInChildren<Text>();

        playButton.onClick.AddListener(OpenPlayMenu);
        optionsButton.onClick.AddListener(OpenOptionsMenu);

        connectButton.onClick.AddListener(OpenConnectionMenu);
        tryHostButton.onClick.AddListener(TryHost);

        tryConnectionButton.onClick.AddListener(TryConnection);

        exitButton.onClick.AddListener(Exit);

        currentMenuState = MenuState.MAIN;
        UpdateMenuState();
    }

    private void OpenPlayMenu()
    {
        currentMenuState = MenuState.PLAY;
        UpdateMenuState();
    }

    private void OpenOptionsMenu()
    {

    }

    private void OpenConnectionMenu()
    {
        currentMenuState = MenuState.CONNECTION;
        UpdateMenuState();
    }

    private void TryHost()
    {
        GlobalData.networkType = NetworkType.HOST;
        SceneManager.LoadScene("main_scene");
    }

    private void TryConnection()
    {
        if (ipAddressField != null)
        {
            GlobalData.networkType = NetworkType.CLIENT;
            GlobalData.connectionAddress = ipAddressField.text;
            SceneManager.LoadScene("main_scene");
        }
    }

    private void Exit()
    {
        switch (currentMenuState)
        {
            case MenuState.MAIN:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                return;


            case MenuState.PLAY:
                currentMenuState = MenuState.MAIN;
                break;


            case MenuState.CONNECTION:
                currentMenuState = MenuState.PLAY;
                break;


            case MenuState.OPTIONS:
                currentMenuState = MenuState.MAIN;
                break;
        }

        UpdateMenuState();
    }

    private void UpdateMenuState()
    {
        if (mainPanel != null) mainPanel.gameObject.SetActive(false);
        if (playPanel != null) playPanel.gameObject.SetActive(false);
        if (connectionPanel != null) connectionPanel.gameObject.SetActive(false);
        if (optionsPanel != null) optionsPanel.gameObject.SetActive(false);

        switch (currentMenuState)
        {
            case MenuState.MAIN:
                if (mainPanel != null) mainPanel.gameObject.SetActive(true);
                if (exitButtonText != null) exitButtonText.text = "Exit";
                break;

            case MenuState.PLAY:
                if (playPanel != null) playPanel.gameObject.SetActive(true);
                if (exitButtonText != null) exitButtonText.text = "Back";
                break;

            case MenuState.CONNECTION:
                if (connectionPanel != null) connectionPanel.gameObject.SetActive(true);
                if (exitButtonText != null) exitButtonText.text = "Back";
                break;

            case MenuState.OPTIONS:
                if (optionsPanel != null) optionsPanel.gameObject.SetActive(true);
                if (exitButtonText != null) exitButtonText.text = "Back";
                break;
        }
    }
}
