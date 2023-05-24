using System.Collections;
using System.Collections.Generic;
using System;
public class Deck
{
    private List<Card> cards;
    private static Deck instance = null;
    private static readonly object padlock = new object();

    public static Deck Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Deck();
                }
                return instance;
            }
        }
    }
    public Deck()
    {
        CreateCards();
    }
    public void CreateCards()
    {
        cards = new List<Card>();
        foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
        {
            if(suit == Card.Suit.Jokers)
            {
                cards.Add(new Card(Card.Suit.Jokers,Card.Rank.Joker1));
                cards.Add(new Card(Card.Suit.Jokers,Card.Rank.Joker2));
            }
            else
            {
                foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank)))
                {
                    if(rank!=Card.Rank.Joker1&&rank!=Card.Rank.Joker2)
                        cards.Add(new Card(suit, rank));
                }
            }
        }
    }
    public void Shuffle()
    {
        System.Random random = new System.Random();
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
    public List<Card> GetLeftCards()
    {
        return cards;
    }
    public int getCardsCount()
    {
        return cards.Count;
    }
    public Card Deal()
    {
        if (cards.Count > 0)
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        else
        {
            return null;
        }
    }
}