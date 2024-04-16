using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button levelButton;
    public float fadingSpeed = 0.7f;
    public GameObject informationGameObject;
    
    private GameObject _scrollPanel;
    private CanvasGroup _informationCanvasGroup;

    private void Awake()
    {
        _scrollPanel = GameObject.FindGameObjectWithTag("ScrollPanel");
        InitialiseLevelSelection();
    }

    private void Start()
    {
        if (informationGameObject is not null)
        {
            _informationCanvasGroup = informationGameObject.GetComponent<CanvasGroup>();
        }
    }

    private static void StartLevel(Level level)
    {
        LevelDatabase.ChosenLevel = level;
        SceneManager.LoadScene("GameScene");
    }

    private void InitialiseLevelSelection()
    {

        foreach (var level in LevelDatabase.Levels)
        {
            var newLevelBtn = Instantiate(levelButton, _scrollPanel.transform, true);
            newLevelBtn.GetComponentInChildren<TextMeshProUGUI>().text = level.LevelName;
            newLevelBtn.onClick.AddListener(() => StartLevel(level));
        }
    }
    
    public void DisplayInstructions()
    {
        informationGameObject.SetActive(true);
        StartCoroutine(FadeInScreen(_informationCanvasGroup));
    }

    public void HideInstructions()
    {
        StartCoroutine(HideScreen(_informationCanvasGroup, informationGameObject));
    }

    private IEnumerator FadeInScreen(CanvasGroup screenCanvasGroup)
    {
        while (screenCanvasGroup.alpha < 1)
        {
            screenCanvasGroup.alpha += Time.deltaTime / fadingSpeed;
            yield return null;
        }
    }

    private IEnumerator HideScreen(CanvasGroup screenCanvasGroup, GameObject screenGameObject)
    {
        while (screenCanvasGroup.alpha > 0)
        {
            screenCanvasGroup.alpha -= Time.deltaTime / fadingSpeed;
            yield return null;
        }
        screenGameObject.SetActive(false);
    }
}