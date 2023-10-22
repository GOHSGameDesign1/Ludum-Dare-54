using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    public float lowerSpawnTime;
    public float upperSpawnTime;
    public GameObject explosionFX;
    private float counter;

    public List<GameObject> cardPrefabs = new List<GameObject>();

    public List<CardDisplay> cards = new List<CardDisplay>();
    Vector2[] cardPositions = { new Vector2(-4.5f, -3f), new Vector2(-1.5f, -3f), new Vector2(1.5f, -3f), new Vector2(4.5f, -3f) };

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple card managers in the same scene");
            return;
        }

        instance = this;
    }

    public static CardManager GetInstance()
    {
        return instance;
    }

    public void AddCard(CardDisplay card)
    {
        cards.Add(card);
        UpdateIndexPositions();
    }

    public void RemoveCard(CardDisplay card)
    {
        cards.Remove(card);
        Destroy(card.gameObject);
        Instantiate(explosionFX, Vector3.zero, Quaternion.identity);
        UpdateIndexPositions();
    }

    void UpdateIndexPositions()
    {
        int counter = 0;
        foreach(CardDisplay card in cards)
        {
            card.originalPos = cardPositions[counter];
            counter++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        counter = Random.Range(lowerSpawnTime, upperSpawnTime);
        SpawnCard();
        SpawnCard();
    }

    // Update is called once per frame
    void Update()
    {
        if (cards.Count >= 4) return;

        if (counter > 0)
        {
            counter -= Time.deltaTime;
        } else
        {
            SpawnCard();
            counter = Random.Range(lowerSpawnTime, upperSpawnTime);
            
        }
    }

    void SpawnCard()
    {
        if (cards.Count >= 4) return;
        Instantiate(cardPrefabs[Random.Range(0, cardPrefabs.Count)]);
    }
}
