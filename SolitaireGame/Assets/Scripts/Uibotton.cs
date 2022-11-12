using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uibotton : MonoBehaviour
{
    public GameObject highScorePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Playagain()
    {
       ResetScene();
       highScorePanel.SetActive(false);
    }
    public void ResetScene()
    {
        UpdateSprite[] cards = FindObjectsOfType<UpdateSprite>();
        foreach(UpdateSprite card in cards)
        {
            Destroy(card.gameObject);
        }
        ClearTopValues();
        FindObjectOfType<Solitaire>().PlayCard();
    }
    void ClearTopValues()
    {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        foreach(Selectable selectable in selectables)
        {
            if (selectable.CompareTag("Top"))
            {
                selectable.suit = null;
                selectable.value = 0;
            }
        }
    }
}
