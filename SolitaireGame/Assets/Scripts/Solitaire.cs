using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public GameObject deckButton;
    public static string[] suits = new string[] { "C","D","H","S"};
    public static string[] values = new string[] { "A","2", "3", "4", "5","6","7","8","9","10","J","Q","K"};
    public List<string>[] bottoms;
    public List<string>[] tops;
    

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    public List<string> deck = new List<string>();
    public List<string> discardPie = new List<string>();
    public List<string> tripOnDisplay = new List<string>();
    private List<List<string>> deckTrips = new List<List<string>>();
    private int deckLocation;
    private int trip;
    private int tripReminder;
    int iOfTrip = 0;
    void Start()
    {
        bottoms = new List<string>[] { bottom0,bottom1,bottom2,bottom3,bottom4,bottom5,bottom6};
        //MakeDeck();
        PlayCard();
        //makeTT();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayCard()
    {
        foreach(List<string>list in bottoms)
        {
            list.Clear();
        }
        deck = GenerateDeck();
        Shuffle(deck);
        CardSort();
        StartCoroutine(MakeCard());
        SortDeckIntoTrips();
    }
    //Make IEnumerator vvv
    IEnumerator MakeCard()
    {
        for (int i = 0; i < 7; i++)
        {
            float yOffset = 0;
            float zOffset = 0.1f;
            foreach (string card in bottoms[i])
            {
                yield return new WaitForSeconds(0.01f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity,bottomPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<Selectable>().row = i;
                if (card == bottoms[i][bottoms[i].Count - 1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                
                yOffset += 0.5f;
                zOffset += 0.3f;
                discardPie.Add(card);
            }
        }
        foreach(string card in discardPie)
        {
            if (deck.Contains(card))
            {
                deck.Remove(card);
            }
        }
        discardPie.Clear();
    }
    void MakeDeck()
    {
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                deck.Add(s+v);
            }
        }
    }
    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }
    void Shuffle<T>(List<T> list) 
    {
        System.Random ran = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = ran.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
    public void SortDeckIntoTrips()
    {
        trip = deck.Count / 3;
        tripReminder = deck.Count % 3;
        deckTrips.Clear();
        int Mod = 0;
        for (int i = 0; i < trip; i++)
        {
            List<string> myTrip = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                myTrip.Add(deck[j+Mod]);
            }
            deckTrips.Add(myTrip);
            //Debug.Log("//////////////////////////////////");
            Mod += 3;
        }
        if (tripReminder != 0)
        {
            List<string> myRemineders = new List<string>();
            Mod = 0;
            for (int k = 0; k<tripReminder;k++)
            {
                myRemineders.Add(deck[deck.Count - tripReminder + Mod]);
                Mod++;
            }
            deckTrips.Add(myRemineders);
            trip++;
        }
        deckLocation = 0;
    }
    public void DealFromDeck()
    {
        foreach (Transform child in deckButton.transform)
        {
            if (child.CompareTag("Card"))
            {
                deck.Remove(child.name);
                discardPie.Add(child.name);
                Destroy(child.gameObject);
            }
        }
        if (deckLocation < trip)
        {
            tripOnDisplay.Clear();
            float xoffSet = 2.5f;
            float zOffset = -0.2f;
            foreach (string card in deckTrips[deckLocation])
            {
                GameObject newTopCard = Instantiate(cardPrefab, new Vector3(deckButton.transform.position.x + xoffSet, deckButton.transform.position.y, deckButton.transform.position.z + zOffset),Quaternion.identity,deckButton.transform);
                xoffSet += 0.5f;
                zOffset -= 0.2f;
                newTopCard.name = card;
                tripOnDisplay.Add(card);
                newTopCard.GetComponent<Selectable>().faceUp = true;
                newTopCard.GetComponent<Selectable>().inDeckPile = true;
            }
            deckLocation++;
        }
        else 
        {
            RestackToTopDeck();
        }
    }
    void RestackToTopDeck()
    {
        deck.Clear();
        foreach(string card in discardPie)
        {
            deck.Add(card);
        }
        discardPie.Clear();
        SortDeckIntoTrips();
    }
    void CardSort()
    {
        for (int i = 0; i<7;i++)
        {
            for (int j = i; j < 7; j++) 
            {
                bottoms[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }
}
