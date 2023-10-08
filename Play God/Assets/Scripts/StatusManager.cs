using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StatusManager : MonoBehaviour
{

    private List<CardDisplay> cards = new List<CardDisplay>();

    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            SpriteRenderer childSprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            childSprite.sprite = null;
        }
    }

    public void AddCard(CardDisplay card, float duration)
    {
        cards.Add(card);
        UpdateIcons();
        StartCoroutine(RemoveCard(card, duration));
    }

    IEnumerator RemoveCard(CardDisplay card, float duration)
    {
        yield return new WaitForSeconds(duration);
        cards.Remove(card);
        UpdateIcons();
    }

    void UpdateIcons()
    {
        for(int i = 0; i < 8; i++)
        {
            SpriteRenderer childSprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            childSprite.sprite = null;
        }
        for(int i = 0; i < cards.Count; i++)
        {
            if (i >= 8) return;
            SpriteRenderer childSprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            childSprite.sprite = cards.ElementAt(i).card.icon;
        }
    }
}
