using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.U2D.IK;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // Mutate
    [Header("Mutate Stats")]
    public float upperLimit;
    public float lowerLimit;

    // year stuff
    public int year {  get; private set; }
    [Header("Year Stats")]
    public float yearIncreaseTime;
    private float yearCounter;

    // population stuff
    public int population { get; private set; }
    [Header("Population Stats")]
    public float popIncreaseTime;
    public float deathRate;
    public float birthRate;
    [SerializeField]
    float currentBirthRate;
    [SerializeField]
    float currentDeathRate;

    private float popCounter;

    // Resources
    [Header("Resources")]
    public float resourceUpdateTime;
    private float resourceCounter;

    [Header("Plant Stats")]
    public int plants;
    public float plantMax;
    public float plantMultiplier;
    public float plantIncreaseRate;
    [SerializeField] private float currentPlantIncreaseRate;

    [Header("Mineral Stats")]
    public int minerals;
    public float mineralMax;
    public float mineralMultiplier;
    public float mineralIncreaseRate;
    [SerializeField] private float currentMineralIncreaseRate;

    [Header("Animal Stats")]
    public int animals;
    public float animalMax;
    public float animalMultiplier;
    public float animalIncreaseRate;
    [SerializeField] private float currentAnimalIncreaseRate;
    public float animalConsumption;

    // Effect List
    [SerializeField]
    private List<Effect> effects = new List<Effect>();

    // Event stuff
    public bool isPaused { get; private set; }
    [Header("Events")]
    public GameObject eventPanel;
    public TextMeshProUGUI eventText;
    public List<Event> events;
    public Event deathEvent;
    private bool dead;
    private int level;
    public List<Event> levelEvents;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one GameManager in scene! Instance not set");
            return;
        }

        instance = this;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        year = 0;
        population = 1000;
        yearCounter = yearIncreaseTime;
        popCounter = popIncreaseTime;
        dead = false;
        UnpauseGame();
        isPaused = false;
        eventPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (yearCounter > 0)
        {
            yearCounter -= Time.deltaTime;
        }
        else
        {
            TimeElapse();
            yearCounter = yearIncreaseTime;
        }

        if(popCounter > 0)
        {
            popCounter -= Time.deltaTime;
        } else
        {
            PopUpdate();
            popCounter = popIncreaseTime;
        }

        if (resourceCounter > 0)
        {
            resourceCounter -= Time.deltaTime;
        }
        else
        {
            ResourceUpdate();
            resourceCounter = resourceUpdateTime;
        }

        if(population == 0 || plants == 0 || animals == 0 || minerals == 0)
        {
            dead = true;
            PauseGame(deathEvent);
        }
    }

    void TimeElapse()
    {
        year++;
        if(year == 30)
        {
            level++;
            PauseGame(levelEvents.ElementAt(0));
            TriggerScriptedEvent(level);
            return;
        } else if(year == 100)
        {
            level++;
            PauseGame(levelEvents.ElementAt(1));
            TriggerScriptedEvent(level);
            return;
        } else if(year == 200)
        {
            level++;
            PauseGame(levelEvents.ElementAt(2));
            TriggerScriptedEvent(level);
            return;
        } else if(year == 300)
        {
            level++;
            PauseGame(levelEvents.ElementAt(3));
            TriggerScriptedEvent(level);
            return;
        } else if(year == 400)
        {
            level++;
            PauseGame(levelEvents.ElementAt(4));
            TriggerScriptedEvent(level);
            return;
        }

        chooseRandomEvent();
    }

    void PopUpdate()
    {

        // List<Effect> birthEffects = new List<Effect>();
        currentBirthRate = birthRate;
        currentDeathRate = deathRate;
        foreach(Effect e in effects)
        {
            /*switch (e.target)
            {
                case "birth":
                    currentBirthRate = ApplyEffect(currentBirthRate, e);
                    break;
                case "death":
                    currentDeathRate = ApplyEffect(currentDeathRate, e);
                    break;
                default:
                    //Debug.LogWarning("Could not register effect: " + e.name + "! Invlaid target!");
                    break;
            }*/

        }

        float birthChange = Mutate(currentBirthRate);
        float deathChange = Mutate(currentDeathRate);
        int netChange = Mathf.RoundToInt(birthChange - deathChange);
        population += netChange;
        population = Mathf.Clamp(population, 0, 9999999);
    }

    void ResourceUpdate()
    {

        currentPlantIncreaseRate = plantIncreaseRate;
        currentAnimalIncreaseRate = animalIncreaseRate;
        currentMineralIncreaseRate = mineralIncreaseRate;

        animalConsumption = animalMultiplier * population;

        foreach(Effect e in effects)
        {
            switch (e.target)
            {
               /* case "plant":
                    currentPlantIncreaseRate = ApplyEffect(currentPlantIncreaseRate, e);
                    break;
                case "animal":
                    currentAnimalIncreaseRate = ApplyEffect(currentAnimalIncreaseRate, e);
                    break;
                case "mineral":
                    currentMineralIncreaseRate = ApplyEffect(currentMineralIncreaseRate, e);
                    break;
                default:
                    //Debug.LogWarning("Could not register effect: " + e.name + "! Invlaid target!");
                    break;*/
            }
        }


        float plantChange = Mutate(currentPlantIncreaseRate) - Mutate(plantMultiplier * population);
        float animalChange = Mutate(currentAnimalIncreaseRate) - Mutate(animalMultiplier * population);
        float mineralChange = Mutate(currentMineralIncreaseRate) - Mutate(mineralMultiplier * population);

        if (plants <= plantMax)
        {
            plants += Mathf.RoundToInt(plantChange);
            plants = (int)Mathf.Clamp(plants, 0, plantMax);
        } else
        {
            //plants -= Mathf.RoundToInt(Mutate(plantMultiplier * population));
        }

        if(animals <= animalMax)
        {
            animals += Mathf.RoundToInt(animalChange);
            animals = (int)Mathf.Clamp(animals, 0, animalMax);
        } else
        {
            //animals -= Mathf.RoundToInt(Mutate(animalMultiplier * population));
        }

        if(mineralChange <= mineralMax)
        {
            minerals += Mathf.RoundToInt(mineralChange);
            minerals = (int)Mathf.Clamp(minerals, 0, mineralMax);
        } else
        {
            //minerals -= Mathf.RoundToInt(Mutate(mineralMultiplier * population));
        }


    }

    float Mutate(float n)
    {
        return n * Random.Range(lowerLimit, upperLimit);
    }

    public void AddEffect(Effect effect, float effectDuration)
    {
        if (effect.isInstant)
        {
            switch (effect.target)
            {
               /* case "birth":
                    population = Mathf.RoundToInt(ApplyEffect(population, effect));
                    return;
                case "death":
                    population = Mathf.RoundToInt(ApplyEffect(population, effect));
                    return;
                case "plant":
                    plants = Mathf.RoundToInt(ApplyEffect(plants, effect));
                    return;
                case "animal":
                    animals = Mathf.RoundToInt(ApplyEffect(animals, effect));
                    return;
                case "mineral":
                    minerals = Mathf.RoundToInt(ApplyEffect(minerals, effect));
                    return;
                default:
                    Debug.LogWarning("Could not register effect: " + effect.name + "! Invlaid target!");
                    return;*/

            }
        }

        Debug.Log("Added effect: " + effect.name);
        StartCoroutine(EffectCountdown(effect, effectDuration));
        effects.Add(effect);
    }

    IEnumerator EffectCountdown(Effect effect, float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Removed effect: " + effect.name);
        effects.Remove(effect);
    }

    // Applies duration effect to a float and returns it
    float ApplyEffect(float n, Effect effect)
    {
        switch (effect.addOrTimes)
        {
            // Addition
            case 0:
                return n + effect.value * (0.5f * (level + 1));
            // Multiplication
            case 1:
                return n * effect.value;
            default:
                Debug.LogWarning("Unable to apply effect: " + effect.name + "! Invalid operation!");
                return n;
        }
    }

    // To be used by effects to change the population
    public void ChangePopulation(int amount)
    {
        population += amount;
    }

    // Used by effects: changes plants
    public void ChangePlants(int amount)
    {
        plants += amount;
    }

    // Used by effects: changes plants
    public void ChangeAnimals(int amount)
    {
        animals += amount;
    }

    // Used by effects: changes plants
    public void ChangeMinerals(int amount)
    {
        minerals += amount;
    }

    void chooseRandomEvent()
    {

       /* foreach(Event e in events)
        {
            int value = Random.Range(0, 100);
            if(value <= e.chanceOfHappening && !effects.Any(x => e.effects.Contains(x)))
            {
                foreach(Effect effect in e.effects)
                {
                    float effectDuration = Random.Range(e.lowerDuration, e.upperDuration);
                    AddEffect(effect, effectDuration);
                }
                PauseGame(e);
                return; // Break the foreach so as to not have multiple events happen on the same day
            }
        }*/
    }

    void TriggerScriptedEvent(int level)
    {
        switch (level)
        {
            case 1:
                birthRate = 3;
                plantMultiplier *= 1.2f;
                break;
            case 2:
                birthRate = 5;
                plantMultiplier *= 1.2f;
                animalMultiplier *= 1.2f;
                break;
            case 3:
                deathRate += 1;
                mineralMultiplier = 0.0005f;
                animalMultiplier *= 1.2f;
                break;
            case 4:
                mineralMultiplier = 0.001f;
                break;
            case 5:
                birthRate = 10;
                plantMultiplier *= 1.2f;
                animalMultiplier *= 1.3f;
                mineralMultiplier = 0.002f;
                break;


        }
    }

    public void PauseGame(Event _event)
    {
        isPaused = true;
        eventText.text = _event.description;
        eventPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        isPaused = false;
        eventPanel.SetActive(false);
        Time.timeScale = 1;
        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(1);
        }
    }
}
