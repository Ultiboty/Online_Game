using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    public void HostGame() 
    {
        SceneManager.LoadScene("Host_scene");
    }
    public void JoinGame()
    {

        SceneManager.LoadScene("Client_scene");
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
