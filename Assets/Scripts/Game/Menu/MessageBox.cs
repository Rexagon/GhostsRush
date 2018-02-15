using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text messageText;
    public Button closeButton;

    void Start ()
    {
        if (PlayerPrefs.HasKey("last_message") && messageText != null && closeButton != null)
        {
            gameObject.SetActive(true);

            messageText.text = PlayerPrefs.GetString("last_message");
            closeButton.onClick.AddListener(() => { SetMessageVisible(false); });

            PlayerPrefs.DeleteKey("last_message");
        }
        else
        {
            SetMessageVisible(false);
        }
	}
	
    private void SetMessageVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
