using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : MonoBehaviour
{
    Transform child;
    private void Awake()
    {
        child = transform.GetChild(0);
        child.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CardDisplay card in CardManager.GetInstance().cards)
        {
            if(Vector2.Distance(transform.position, card.transform.position) < 3)
            {
                child.gameObject.SetActive(true);
                return;
            }
        }
        child.gameObject.SetActive(false);
    }

}
