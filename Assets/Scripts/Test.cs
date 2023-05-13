using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    // public List<int> handCards = new List<int>{2, 3, 3, 3, 4, 4, 5, 6, 7};  // 手牌列表，存储手牌的牌面值
    public List<Card> handCards;
    List<Card> lastCards;
    void Start()
    {
        List<int> handintCards = new List<int> { 2, 3, 3, 3, 4, 4, 5, 6, 7, 8, 9,10};
        List<int> lastintCards = new List<int> {3, 4, 5, 6, 7,8 };
        handCards = new List<Card>();
        lastCards = new List<Card>();
        foreach (var i in handintCards)
        {            
            handCards.Add(new Card((Card.Suit)1,(Card.Rank)i));
        } 
         foreach (var i in lastintCards)
        {            
            lastCards.Add(new Card((Card.Suit)1,(Card.Rank)i));
        } 
        List<List<Card>> straights = GetCanStraights(handCards,lastCards);
        Debug.Log("Straights Count: " + straights.Count);

        foreach (List<Card> straight in straights)
        {
            Debug.Log("Straight: " + string.Join(", ", straight));
        }
    }
    List<Card> GetMaxStraights(List<Card> cards,List<Card> lastCards)
    {
        List<Card> maxstraight = new List<Card>();
        int currentCount = 1;
        Card currentCard = cards[0];
        int lastCount = lastCards.Count;
        List<Card> straight = new List<Card> { currentCard };

        for (int i = 1; i < cards.Count; i++)
        {   
            if(currentCard.rank!=Card.Rank.Two&&currentCard.rank!=Card.Rank.Joker1&&currentCard.rank!=Card.Rank.Joker2)
            {
                if (cards[i].GetCardRank() == currentCard.GetCardRank() + 1)
                {
                    currentCount++;
                    currentCard = cards[i];
                    straight.Add(currentCard);
                }
                else if (cards[i].GetCardRank() != currentCard.GetCardRank())
                {
                    currentCount = 1;
                    currentCard = cards[i];
                    straight = new List<Card> { currentCard };
                }

                if (currentCount >= lastCount)
                {
                    lastCount = straight.Count;
                    maxstraight= straight;
                }
            }
            else
            {
                currentCard = cards[i];
                straight = new List<Card> { currentCard };
            }
        }
        return maxstraight;
    }
    private int getMax(List<Card> cards)
    {
        int max = -1;
        foreach (var card in cards)
        {
            max = card.GetCardRank()>max?card.GetCardRank():max; 
        }
        return max;
    }
    List<List<Card>> GetCanStraights(List<Card> cards, List<Card> lastCards)
    {
        List<Card> straight = new List<Card>();
        List<List<Card>> canStraights = new List<List<Card>>();
        int lastMaxValue = getMax(lastCards);
        int lastCount = lastCards.Count;
        // 获取符合长度的顺子
        straight = GetMaxStraights(cards, lastCards);
       
        if (getMax(straight) > lastMaxValue)
        {
            if (straight.Count == lastCount)
            {
                canStraights.Add(straight);
            }
            else if (straight.Count > lastCount)
            {
                int index = straight.FindIndex(a=>lastMaxValue<a.GetCardRank());
                int startIndex = index-lastCount+1<0?0:index-lastCount+1;
                for (int i = startIndex; i <= straight.Count - lastCount; i++)
                {
                    List<Card> s = straight.GetRange(i, lastCount);
                    canStraights.Add(s);
                }
            }
        }

        return canStraights;
    }

    // List<List<int>> GetCanStraights(List<int> cards,List<int> lastCards)
    // {
    //     List<List<int>> straights = new List<List<int>>();
    //     List<List<int>> canstraights = new List<List<int>>();
    //     int lastMaxValue =  lastCards.Max();
    //     int lastCount = lastCards.Count;
    //     // 获取符合长度的顺子
    //     straights = GetStraights(cards,lastCards);
    //     foreach (var straight in straights)
    //     {
    //         if(straight.Max()>lastMaxValue)
    //         {
    //             if(straight.Count==lastCount)
    //                 canstraights.Add(straight);
    //             else
    //             {
    //                 for (int i = straight.Count-1; i >=0; i--)
    //                 {
    //                     int index = i-lastCount+1;
    //                     if(index>=0)
    //                     {
    //                         List<int> s = straight.GetRange(index,i);
    //                         canstraights.Add(s);
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //     return canstraights;
    // }
    // List<List<int>> GetStraights(List<int> cards,List<int> lastCards)
    // {
    //     List<List<int>> straights = new List<List<int>>();
    //     int lastMaxValue =  lastCards.Max();
    //     int lastCount = lastCards.Count;
    //     Debug.Log(lastCount);
    //     int currentCount = 1;
    //     int currentCard = cards[0];
    //     int index;
    //     List<int> straight = new List<int>();
    //     // 2, 3, 3, 3, 4, 4, 5, 6, 7
    //     int nowCount = -1;
    //     while(straights.Count>nowCount)
    //     {
    //         if(straights.Count>0)
    //         {
    //         //    straights[-1][0]
    //             Debug.Log(straights[straights.Count-1][1]);
    //             index = cards.FindIndex(a=> a==straights[straights.Count-1][1]);
    //             Debug.Log(index);
    //         }
    //         else
    //         {
    //             index = 0;
    //         }
    //         nowCount = straights.Count;
    //         for (int i = index; i < cards.Count-1; i++)
    //         {
    //             if(cards[i]!=2&&cards[i]!=13&&cards[i]!=14)
    //             {
    //                 if (cards[i] + 1 == cards[i+1] )
    //                 {
    //                     currentCount++;
    //                     currentCard = cards[i];
    //                     straight.Add(currentCard);
    //                 }
    //                 else if (cards[i] != cards[i+1])
    //                 {
    //                     currentCount = 1;
    //                     currentCard = cards[i];
    //                     straight = new List<int> { currentCard };
    //                 }
    //                 if (currentCount == lastCount)
    //                 {
    //                     straights.Add(straight);
    //                 }
    //             }

    //         }
    //     }

    //     return straights;
    // }
}