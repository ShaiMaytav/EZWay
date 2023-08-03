using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private LevelSelectionUI levelSelectionUI;
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private GameplayUI gameplayUI;

    private GameManager _gameManager;
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("A UIManager component was removed from " + gameObject.name);
            Destroy(this);
        }

        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        levelSelectionUI.CreateLevelsButtons();
    }

    public void Play()
    {
        mainMenuUI.gameObject.SetActive(false);
        levelSelectionUI.gameObject.SetActive(true);
        levelSelectionUI.UpdateButtonsInfo();
    }

    public void StartLevel()
    {
        levelSelectionUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
    }
}
