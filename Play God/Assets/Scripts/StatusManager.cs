using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StatusManager : MonoBehaviour
{

    public static StatusManager Instance;

    private List<CardDisplay> cards = new List<CardDisplay>();

    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < 8; i++)
        {
            SpriteRenderer childSprite = transform.GetChild(i).GetComponent<SpriteRenderer>();
            childSprite.sprite = null;
        }
    }

    public static StatusManager GetInstance()
    {
        return Instance;
    }

    public void AddCard(CardDisplay cardDisplay, float duration)
    {
        cards.Add(cardDisplay);
        UpdateIcons();

        foreach(StatusEffect effect in cardDisplay.card.statusEffects)
        {
            StartCoroutine(UseStatusEffects(effect, duration));
        }

        StartCoroutine(RemoveCard(cardDisplay, duration));
    }

    IEnumerator RemoveCard(CardDisplay cardDisplay, float duration)
    {
        yield return new WaitForSeconds(duration);
        cards.Remove(cardDisplay);
        UpdateIcons();
    }

    IEnumerator UseStatusEffects(StatusEffect effect, float duration)
    {
        float timeCounter = 0;
        float effectCounter = 0;

        while (timeCounter < duration)
        {

            if(effectCounter >= effect.changeInterval)
            {
                effectCounter = 0;
                ApplyEffect(effect);
                Debug.Log("Used Effect");
            }

            timeCounter += Time.deltaTime;
            effectCounter += Time.deltaTime;
            yield return null;
        }
    }

    void ApplyEffect(StatusEffect effect)
    {
        switch(effect.target)
        {
            case Target.Population:
                GameManager.GetInstance().ChangePopulation(effect.value);
                break;
            case Target.Animal:
                GameManager.GetInstance().ChangeAnimals(effect.value);
                break;
        }
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
