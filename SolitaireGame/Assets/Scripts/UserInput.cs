using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
                    Top(hit.collider.gameObject);
                }
                if (hit.collider.CompareTag("Bottom"))
                {
                    Bottom(hit.collider.gameObject);
                    print("hit bottom");
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
        if (!selected.GetComponent<Selectable>().faceUp)
        {
            if (!Blocked(selected))
            {
                selected.GetComponent<Selectable>().faceUp = true;
                slot1 = this.gameObject;
            }

        }

        else if (selected.GetComponent<Selectable>().inDeckPile)
        {
            if (!Blocked(selected))
            {
                slot1 = selected;
            }
        }

        else
        {

            if (slot1 == gameObject)
            {
                slot1 = selected;
            }

            else if (slot1 != selected)
            {
                if (Stackable(selected))
                {
                    Stack(selected);
                }
                else
                {
                    slot1 = selected;
                }
            }
        }
    }
    void Top(GameObject selected)
    {
        if (slot1.CompareTag("Card"))
        {
            if(slot1.GetComponent<Selectable>().value == 1)
            {
                Stack(selected);
            }
        }
    }
    void Bottom(GameObject selected)
    {
        if(slot1.CompareTag("Card"))
        {
            if (slot1.GetComponent<Selectable>().value == 13)
            {
                Stack(selected);
            }
        }
    }

    bool Stackable(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        if (!s2.inDeckPile)
        {
            if (s2.top)
            {
                if (s1.suit == s2.suit || (s1.value == 1 && s2.suit == null))
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
                    if (s1.suit == "C" || s1.suit == "S")
                    {
                        card1Rad = false;
                    }
                    if (s2.suit == "C" || s2.suit == "S")
                    {
                        card2Rad = false;
                    }
                    if (card1Rad == card2Rad)
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
        }
        return false;
    }
    void Stack(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        float yoffset = 0.3f;
       
        if (s2.top || !s2.top && s1.value == 13)
        {
            yoffset = 0;

        }
        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y-yoffset, selected.transform.position.z - 0.01f);
        slot1.transform.parent = selected.transform;
        if (s1.inDeckPile)
        {
            solitaire.tripOnDisplay.Remove(slot1.name);
        }
        else if (s1.top && s2.top && s1.value == 1)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = 0;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = null;
        }
        else if (s1.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value-1;
        }
        else
        {
            solitaire.bottoms[s1.row].Remove(slot1.name);

        }
        s1.inDeckPile = false;
        s1.row = s2.row;

        if (s2.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        }
        else
        {
            s1.top = false;
        }
        slot1 = gameObject;
    }
    bool Blocked (GameObject selected)
    {
        Selectable s2 = selected.GetComponent<Selectable>();
        if (s2.inDeckPile == true)
        {
            if (s2.name == solitaire.tripOnDisplay.Last())
            {
                return false;
            }
            else
            {
                print(s2.name + "blolck by " + solitaire.tripOnDisplay.Last());
                return true;
            }
        }
        else
        {
            if (s2.name == solitaire.bottoms[s2.row].Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}