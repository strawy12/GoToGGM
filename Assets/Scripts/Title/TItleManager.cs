using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TItleManager : MonoBehaviour
{
    [SerializeField] GameObject msgBox = null;
    public void QuitButtonOnClick()
    {
        msgBox.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void CancleButtonOnClick()
    {
        msgBox.SetActive(false);
    }

    public void StartAreaOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}
