using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    public List<Sprite> icons;
    public List<Sprite> iconsCopy;
    public List<int> indexes;
    
    public GameObject itemPrafab;

    public float xMin, xMax;

    public float yMin, yMax;

    public Color[] colors;

    List<Color> colorsList;

    Color tempColor;

    public List<Item> levelItems { get; private set; } = new List<Item>();

    public event System.Action OnAllItemsDeleted;


    // Start is called before the first frame update
    void Awake()
    {
        colorsList = new List<Color>(colors);
        
        
    }

   public void Generate(int itemsCount)
    {
        colorsList = new List<Color>(colors);
        iconsCopy = new List<Sprite>(icons);
        indexes = new List<int>();
        GameObject firstItem = null;
        for (int i = 0; i < itemsCount; i++)
        {

            indexes.Add(i);
           GameObject item = Instantiate(itemPrafab, GetRandomPosition(), Quaternion.identity);
            item.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            item.GetComponent<SpriteRenderer>().sprite = GetRandomSprite();
            item.AddComponent<BoxCollider2D>().isTrigger = true;
            item.GetComponent<Item>().index = i;
            item.name = i.ToString();
            item.GetComponent<Item>().particlesColor = tempColor;
            if(i == 0)
            {
                firstItem = item;
            }
            levelItems.Add(item.GetComponent<Item>());
        }

        indexes.Add(0);
        GameObject lastitem = Instantiate(firstItem, GetRandomPosition(), Quaternion.identity);
        levelItems.Add(lastitem.GetComponent<Item>());
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);
        return new Vector3(randomX, randomY, 0);
    }

    Sprite GetRandomSprite()
    {
        int randomIndex = Random.Range(0, iconsCopy.Count);
        Sprite sprite = iconsCopy[randomIndex];
        tempColor = colorsList[randomIndex];
        iconsCopy.Remove(sprite);
        colorsList.Remove(tempColor);

        return sprite;
    }
    public void SpawnNextItem(int deletedItemIndex, Vector3 position)
    {
        var items = FindObjectsOfType<Item>();
        foreach (Item item in items)
        {
            if(item.index == deletedItemIndex + 1)
                  
            {
                GameObject nextitem = Instantiate(item.gameObject, position, Quaternion.identity);
                nextitem.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            }
        }
    }
    public void DeleteItem(Item item)
    {
        levelItems.Remove(item);
        if(levelItems.Count == 0)
        {
            OnAllItemsDeleted?.Invoke();
        }
    }
}
