using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public List<GameObject> Screens = new List<GameObject>();
    public GameObject PreviousButton;
    public GameObject NextButton;

    [HideInInspector] public int ScreenIndex = 0;

    private void OnEnable()
    {
        Screens[ScreenIndex].SetActive(false);
        ScreenIndex = 0;
        Screens[ScreenIndex].SetActive(true);

        PreviousButton.SetActive(false);
    }
}
