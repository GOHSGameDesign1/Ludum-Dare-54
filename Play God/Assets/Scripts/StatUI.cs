using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    public TextMeshProUGUI year;
    public TextMeshProUGUI population;
    public TextMeshProUGUI plant;
    public TextMeshProUGUI mineral;
    public TextMeshProUGUI animal;

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        year.text = "Year: " + GameManager.GetInstance().year;
        population.text = "Population: " + GameManager.GetInstance().population;
        plant.text = ": " + GameManager.GetInstance().plants;
        mineral.text = ": " + GameManager.GetInstance().minerals;
        animal.text = ": " + GameManager.GetInstance().animals;
    }
}
