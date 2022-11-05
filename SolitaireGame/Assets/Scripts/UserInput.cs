using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    private Solitaire solitaire;
    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }
    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    Deck();
                }
                if (hit.collider.CompareTag("Card"))
                {

                    Card(hit.collider.gameObject);
                }
                if (hit.collider.CompareTag("Top"))
                {
                    Top();
                }
                if (hit.collider.CompareTag("Bottom"))
                {
                    Bottom();
                }
            }
        }
    }
    void Deck()
    {
        solitaire.DealFromDeck();
    }
    void Card(GameObject selected)
    {
        if (slot1 == gameObject)
        {
            slot1 = selected;
        }
        
        else if (slot1 != selected)
        {
            if (Stackable(selected))
            {

            }
            else
            {
                slot1 = selected;
            }
        }
    }
    void Top()
    {

    }
    void Bottom()
    {

    }

    bool Stackable(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
       if (s2.top)
        {
            if(s1.suit == s2.suit || (s1.value == 1 && s2.suit == null))
            { 
                if (s1.value == s2.value + 1) 
                {
                    return true;
                } 
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (s1.value == s2.value - 1)
            {
                bool card1Rad = true;
                bool card2Rad = true;
                if  (s1.suit == "C" || s1.suit == "S")
                {
                    card1Rad = false; 
                }
                if  (s2.suit == "C" || s2.suit == "S")
                {
                    card2Rad = false;
                }
                if(card1Rad == card2Rad)
                {
                    print(" Not Stackable!!! ");
                        return false;
                }
                else
                {
                    print("Stackable");
                    return true;
                }
            }
        }
        return false;
    }
}
