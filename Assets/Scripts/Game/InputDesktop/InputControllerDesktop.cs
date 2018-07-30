using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputControllerDesktop : InputController
{
    [Header("Lobby")]
    public Camera lobbyCamera;

    [Header("Root panels")]
    public RectTransform lobbyPanel;
    public RectTransform gamePanel;

    [Header("Additional panels")]
    public RectTransform pawnsPanel;

    [Header("Counters labels")]
    public Text mealCounterText;
    public Text manaCounterText;

    [Header("Controlls")]
    public UnitSelectionButton[] unitSelectionButtons;

    private bool lobbyEnabled = false;


    void Awake()
    {
        foreach (UnitSelectionButton selectionButton in unitSelectionButtons)
        {
            if (selectionButton != null)
            {
                selectionButton.inputController = this;
            }
        }

        SetLobbyEnabled(true);
    }

    private void Update()
    {
        if (lobbyEnabled && ExitButtonPressed())
        {
            PlayerPrefs.SetString("last_message", "You cancelled session creation");
            SceneManager.LoadScene("main_menu");
        }
    }


    // Lobby
    public override void SetLobbyEnabled(bool lobbyEnabled)
    {
        this.lobbyEnabled = lobbyEnabled;

        if (lobbyCamera != null)
        {
            lobbyCamera.enabled = lobbyEnabled;
            AudioListener audioListener = lobbyCamera.GetComponent<AudioListener>();
            if (audioListener)
            {
                audioListener.enabled = lobbyEnabled;
            }
        }

        // Configure/hide lobby panel
        if (lobbyPanel)
        {
            if (lobbyEnabled)
            {
                Text connectionText = lobbyPanel.GetComponentInChildren<Text>();
                if (connectionText != null)
                {
                    if (PlayerPrefs.HasKey("is_server") && PlayerPrefs.GetInt("is_server") == 1)
                    {
                        connectionText.text = "Waiting for connections...";
                    }
                    else
                    {
                        connectionText.text = "Connecting to server...";
                    }
                }
            }

            lobbyPanel.gameObject.SetActive(lobbyEnabled);
        }

        // Hide/show game panel
        if (gamePanel != null)
        {
            gamePanel.gameObject.SetActive(!lobbyEnabled);
        }
    }

    // General input
    public override FieldCell GetHoveredCell()
    {
        if (mainCamera == null ||
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f, 1 << 9))
        {
            FieldCell cell = hit.transform.gameObject.GetComponent<FieldCell>();
            if (cell)
            {
                return cell;
            }
        }

        return null;
    }

    public override GameUnit GetHoveredUnit()
    {
        if (mainCamera == null ||
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f, 1 << 8))
        {
            GameUnit unit = hit.transform.gameObject.GetComponent<GameUnit>();
            if (unit)
            {
                return unit;
            }
        }

        return null;
    }

    public override bool AcceptButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override bool RejectButtonPressed()
    {
        return Input.GetMouseButtonDown(1);
    }

    public override bool SelectButtonPressed()
    {
        return Input.GetMouseButtonDown(1);
    }

    public override bool ExitButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }


    // UI input

    public override void SetMealAmount(int amount)
    {
        mealCounterText.text = amount.ToString();
    }

    public override void SetManaAmount(int amount)
    {
        manaCounterText.text = amount.ToString();
    }

    public override void SelectPlaceableUnit(GameUnit unit)
    {
        if (unit == null)
        {
            foreach (UnitSelectionButton selectionButton in unitSelectionButtons)
            {
                if (selectionButton != null)
                {
                    selectionButton.Deselect();
                }
            }
        }

        base.SelectPlaceableUnit(unit);
    }
}
