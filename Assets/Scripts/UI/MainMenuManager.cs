using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Difficulty")]
    public Button[] difficultyButtons; // Easy / Normal / Hard
    public Color    selectedDiffColor = new Color(0.8f, 0.6f, 0.1f);
    public Color    normalDiffColor   = new Color(0.3f, 0.3f, 0.3f);

    [Header("Audio")]
    public AudioSource bgMusic;
    public AudioClip   menuTheme;

    [Header("Visuals")]
    public CanvasGroup logoGroup;

    void Start()
    {
        // Intro fade
        if (logoGroup)
        {
            logoGroup.alpha = 0;
            logoGroup.DOFade(1f, 1.5f).SetEase(Ease.InOutQuad);
        }

        playButton.onClick.AddListener(OnPlay);
        settingsButton?.onClick.AddListener(() => ShowPanel(settingsPanel));
        creditsButton?.onClick.AddListener(() => ShowPanel(creditsPanel));
        quitButton?.onClick.AddListener(OnQuit);

        // Difficulty buttons
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            int idx = i;
            difficultyButtons[i].onClick.AddListener(() => SelectDifficulty(idx));
        }

        // Default difficulty: Normal
        SelectDifficulty(1);

        // Music
        if (bgMusic && menuTheme)
        {
            bgMusic.clip = menuTheme;
            bgMusic.loop = true;
            bgMusic.Play();
        }

        ShowPanel(mainPanel);
    }

    void OnPlay()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("Game");
    }

    void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ShowPanel(GameObject panel)
    {
        mainPanel?.SetActive(panel == mainPanel);
        settingsPanel?.SetActive(panel == settingsPanel);
        creditsPanel?.SetActive(panel == creditsPanel);
    }

    void SelectDifficulty(int idx)
    {
        PlayerPrefs.SetInt("Difficulty", idx);
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            var img = difficultyButtons[i].GetComponent<Image>();
            if (img) img.color = i == idx ? selectedDiffColor : normalDiffColor;
        }
    }
}
