using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    
    [SerializeField] LevelGenerator levelGenerator;
    int level = 1;
    [SerializeField] GameObject timer;
    [SerializeField] Text levelCompleteText;
    [SerializeField] Text gameOverText;
    [SerializeField] Button newGameButton;
    [SerializeField] Button nextLevelButton;
    

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator.OnAllItemsDeleted += LevelComplete;
        /*StartNewGame();*/
        newGameButton.onClick.AddListener(StartNewGame);
        nextLevelButton.onClick.AddListener(NextLevel);
        timer.GetComponent<TimeManager>().OnTimesEnd += GameOver;

    }
    void StartNewGame()
    {
        int itemsCount = 5 + level - 1;
        float startTime = 5 + level - 0.5f;

        levelGenerator.Generate(itemsCount);
        timer.GetComponent<TimeManager>().StartTime(startTime, 0.5f);

        levelCompleteText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        timer.SetActive(true);
    }
    void LevelComplete()
    {
        levelCompleteText.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);

        timer.SetActive(false);
    }
    void GameOver()
    {
        StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            gameOverText.gameObject.SetActive(true);
            level = 1;
            timer.SetActive(false);
            HideAllItems();

            yield return new WaitForSeconds(0.5f);
            DestroyAll();
            newGameButton.gameObject.SetActive(true);

        }

    }
    void NextLevel()
    {
        level++;
        StartNewGame();
    }
    void HideAllItems()
    {
        foreach (Item item in levelGenerator.levelItems)
        {
            item.GetComponent<Animator>().SetTrigger("Hide");

        }
    }
    void DestroyAll()
    {
        var items = FindObjectsOfType<Item>();
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i].gameObject);
        }
    }
}
