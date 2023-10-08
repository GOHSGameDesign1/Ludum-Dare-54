using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CardDisplay : MonoBehaviour
{
    private bool dragging;
    private Camera cam;
    private Transform globe;
    public Card card;

    private GameObject statusManager;
    public Vector2 originalPos;
    private Vector2 velocity = new Vector2(0, 0);

    // Reference Children Components
    private TextMeshPro titleText;
    private TextMeshPro descriptionText;
    private SpriteRenderer iconRenderer;

    private void Awake()
    {
        cam = Camera.main;

        globe = GameObject.Find("Globe").transform;
        statusManager = GameObject.Find("Status Manager");
        titleText = transform.GetChild(1).GetComponent<TextMeshPro>();
        descriptionText = transform.GetChild(2).GetComponent<TextMeshPro>();
        iconRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();

        titleText.text = card.title;
        descriptionText.text = card.description;
        iconRenderer.sprite = card.icon;
    }

    private void Start()
    {
        CardManager.GetInstance().AddCard(this);
    }

    private void Update()
    {

        if (!dragging)
        {
            transform.position = Vector2.SmoothDamp(transform.position, originalPos, ref velocity, 0.1f, Mathf.Infinity, Time.unscaledDeltaTime);
            return;
        }



            Vector2 mousePosition = cam.ScreenToWorldPoint( Input.mousePosition );
        transform.position = Vector2.SmoothDamp(transform.position, mousePosition, ref velocity, 0.1f, Mathf.Infinity, 2 * Time.unscaledDeltaTime);
    }    
    
    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;

        if(Vector2.Distance(transform.position, globe.position) < 3 && !GameManager.GetInstance().isPaused)
        {
            transform.position = globe.position;
            UseEffect();
            return;
        }

        // yea but i learned everything thyey teach p much
        // yt mostly and just by doing u get experince share room u right no, its for a competition, I wont, its for fun
        StopAllCoroutines();
        MoveTowards(originalPos);
    }

    public void MoveTowards(Vector2 location)
    {
        //StartCoroutine(MoveLerp(location));
    }

    public IEnumerator MoveLerp(Vector2 location)
    {
        transform.position = location;
        float time = 0;
        float duration = 100f;
        Vector2 startPos = transform.position;
        float t = time / duration;

        while (time < duration)
        {
            t = time / duration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector2.Lerp(startPos, location, time/duration);
            time ++;
            yield return null;
        }

        //transform.position = location;
    }

    void UseEffect()
    {
        Debug.Log("Card Used: " + card.title + "\nEffects used: " + card.effects);

        float effectDuration = Random.Range(card.lowerDurationLimit, card.upperDurationLimit);
        statusManager.GetComponent<StatusManager>().AddCard(this, effectDuration);
        foreach (Effect effect in card.effects)
        {
            GameManager.GetInstance().AddEffect(effect, effectDuration);
        }
        CardManager.GetInstance().RemoveCard(this);
    }
}
