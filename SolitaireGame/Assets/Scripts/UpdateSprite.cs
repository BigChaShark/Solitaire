using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable Selectable;
    private Solitaire solitaire;
    private UserInput userInput;
    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        Selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Selectable.faceUp)
        {
            spriteRenderer.sprite = cardFace;
        }
        else 
        {
            spriteRenderer.sprite = cardBack;
        }

        if (userInput.slot1)
        {

            if (name == userInput.slot1.name)
            {
                spriteRenderer.color = Color.yellow;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
}
