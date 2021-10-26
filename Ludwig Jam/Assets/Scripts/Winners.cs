using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winners : MonoBehaviour
{
    public static bool won;
    [SerializeField] GameObject winText;
    // Start is called before the first frame update
    void Awake()
    {
        EventHandler.instance.e_Win += ListenWin;
        EventHandler.instance.e_Restart += ListenRestart;
    }

    public void ListenWin()
	{
        won = true;
        winText.SetActive(true);
	}

    public void ListenRestart()
	{
        won = false;
        winText.SetActive(false);
	}
}
