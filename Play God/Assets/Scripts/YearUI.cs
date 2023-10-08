using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YearUI : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = "Year: " + GameManager.GetInstance().year.ToString();
    }
}
