using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] GameObject creditsMenuUI;

    public void Back()
    {
        creditsMenuUI.SetActive(false);
    }
}
