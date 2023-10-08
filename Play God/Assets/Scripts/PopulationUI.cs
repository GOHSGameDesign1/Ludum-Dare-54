using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationUI : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = "Population: " + GameManager.GetInstance().population;
    }
}
