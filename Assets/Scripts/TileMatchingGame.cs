using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 *  Responsible for the game initialisation and
 *  handling UI elements
 *  Handles game conditions, actively checks if game
 *  is finished and what is the result
 */
public sealed class TileMatchingGame : MonoBehaviour
{
    public static TileMatchingGame Instance { get; private set; }
    public int swipesLeft;
    
    /*
     * the bigger the value, the slower screens fade
     */
    public float fadingSpeed = 100;

    private Level _levelToPlay;

    public TMP_Text jewelsLeftText;
    public TMP_Text swipesLeftText;
    public GameObject endOfGamePanel;
    public TMP_Text endOfGameResultTest;
    public TMP_Text endOfGameDescriptionText;

    public GameObject gameMenuObject;

    public Button nextLevelButton;

    private int _jewelsCount;
    private CanvasGroup _endOfGameCanvasGroup;
    private CanvasGroup _gameMenuCanvasGroup;
    private float _timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateJewelsCount();
        EndOfGameCheck();
    }

    private void Start()
    {
        _levelToPlay = LevelDatabase.ChosenLevel;
        nextLevelButton.interactable = false;

        BoardController.Instance.BoardInitialization(_levelToPlay.LevelGridLayout);
        swipesLeft = _levelToPlay.NumOfSwaps;

        UpdateJewelsCount();
        UpdateSwipesLeftText();

        if (endOfGamePanel is not null)
        {
            _endOfGameCanvasGroup = endOfGamePanel.GetComponent<CanvasGroup>();
        }

        if (gameMenuObject is not null)
        {
            _gameMenuCanvasGroup = gameMenuObject.GetComponent<CanvasGroup>();
        }
    }

    public bool SwipeAllowed()
    {
        return swipesLeft > 0;
    }

    public void UseSwipe()
    {
        swipesLeft--;
        UpdateSwipesLeftText();
    }
    
    public void NextLevel()
    {
        _levelToPlay = LevelDatabase.GetNextLevel();
        RestartLevel();
    }

    public void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void DisplayGameMenu()
    {
        gameMenuObject.SetActive(true);
        StartCoroutine(FadeInScreen(_gameMenuCanvasGroup));
    }

    public void HideGameMenu()
    {
        StartCoroutine(HideScreen(_gameMenuCanvasGroup, gameMenuObject));
    }

    private void UpdateJewelsCount()
    {
        var nJewels = BoardController.Instance.GetNumOfJewelsLeft();
        if (_jewelsCount == nJewels) return;
        _jewelsCount = nJewels;
        jewelsLeftText.text = $"Jewels Left: {_jewelsCount}";

        if (_jewelsCount == 0) jewelsLeftText.color = Color.green;
    }

    private void UpdateSwipesLeftText()
    {
        swipesLeftText.text = $"Swipes Left: {swipesLeft}";
        if (swipesLeft < 3) swipesLeftText.color = Color.red;
    }

    private void EndOfGameCheck()
    {
        if (_jewelsCount <= 0)
        {
            nextLevelButton.interactable = true;
            GameWon();
        }
        else if (swipesLeft <= 0 
                 && !BoardController.Instance.AtLeastOneMatchingSet())
        {
            /*
             * Some Eliminations are animation restricted
             */
            _timer += Time.deltaTime;
            
            if (_timer < 1.5f) return;
            
            _timer = 0;
            GameFailed();
        }
        else
        {
            return;
        }
        
        endOfGamePanel.SetActive(true);
        StartCoroutine(FadeInScreen(_endOfGameCanvasGroup));
    }

    private void GameWon()
    {
        endOfGameResultTest.text = "Victory";
        endOfGameDescriptionText.text = "All jewels are eliminated!";
    }

    private void GameFailed()
    {
        endOfGameResultTest.text = "Game Over";
        endOfGameDescriptionText.text = "You run out of swaps!";
        endOfGameDescriptionText.color = Color.red;
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