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
    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
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
    }
}
