using System.Collections.Generic;
using UnityEngine;

public class DoudiZhuRule : MonoBehaviour {
    // 判断手牌是否可以出牌的方法
    public static bool CanPlay(List<Card> hand, List<Card> lastPlayed,out CardType handType)
    {
        int handRank;
        int lastPlayedRank;
        int lastCount = lastPlayed.Count;   // 上家出牌的数量
        CardType lastType = GetCardType(lastPlayed,out lastPlayedRank);  // 上家出牌的牌型

        int handCount = hand.Count; // 手牌的数量
        handType = GetCardType(hand,out handRank);  // 手牌的牌型
        if (handType == CardType.None)
        {
            // 手牌不合法，不能出牌
            return false;
        }
        if (lastPlayed == null || lastPlayed.Count == 0)
        {
            // 如果上家没有出牌，则任何手牌都可以出
            return true;
        }

        if (handType == CardType.JokerBomb)
        {
            // 火箭可以打任何牌型
            return true;
        }

        if (handType == CardType.Bomb)
        {
            // 炸弹只能打比它大的炸弹或者火箭
            if (lastType == CardType.Bomb || lastType == CardType.JokerBomb)
            {
                if (handRank > lastPlayedRank)
                {
                    return true;
                }
            }
            return false;
        }

        if (handType != lastType || handCount != lastCount)
        {
            // 手牌牌型和数量不符合要求，不能出牌
            return false;
        }

        if (handType == CardType.StraightThree || handType == CardType.StraightPair || handType == CardType.Straight)
        {
            // 顺子、连对、飞机必须比上家大
            if (handRank > lastPlayedRank)
            {
                return true;
            }
            return false;
        }

        // 其它牌型只要点数相同就可以打
        if (handRank > lastPlayedRank)
        {
            return true;
        }
        return false;
    }


    // 牌型枚举
    public enum CardType {
        None = 0, // 无牌型
        Single = 1, // 单张
        Pair = 2, // 对子
        Three = 3, // 三张
        ThreeWithOne = 4, // 三带一
        ThreeWithPair = 5, // 三带二
        Straight = 6, // 单顺
        StraightPair = 7, // 双顺
        StraightThree = 8, // 三顺
        Bomb = 9, // 炸弹
        JokerBomb = 10, // 火箭
    }

    // 计算牌型并返回得分
    public static int GetScore(List<Card> cards) {
       

        int score = 0;
        CardType type = GetCardType(cards, out int rank);

        switch (type) {
            case CardType.Single:
                score = (rank > 10 ? rank - 10 : 0);
                break;
            case CardType.Pair:
                score = (rank > 10 ? rank - 10 : 0) * 2;
                break;
            case CardType.Three:
                score = (rank > 10 ? rank - 10 : 0) * 5;
                break;
            case CardType.ThreeWithOne:
                score = (rank > 10 ? rank - 10 : 0) * 5 + 10;
                break;
            case CardType.ThreeWithPair:
                score = (rank > 10 ? rank - 10 : 0) * 5 + 20;
                break;
            case CardType.Straight:
                score = (rank > 10 ? rank - 10 : 0) * 10;
                break;
            case CardType.StraightPair:
                score = (rank > 10 ? rank - 10 : 0) * 20;
                break;
            case CardType.StraightThree:
                score = (rank > 10 ? rank - 10 : 0) * 30;
                break;
            case CardType.Bomb:
                score = (rank > 10 ? rank - 10 : 0) * 100;
                break;
            case CardType.JokerBomb:
                score = 200;
                break;
        }

        return score;
    }
    
    // 判断牌型并返回点数和牌型
    public static CardType GetCardType(List<Card> cards, out int rank) {
         // 先将牌按点数从小到大排序
        cards.Sort((a, b) => a.GetCardRank() - b.GetCardRank());
        rank = 0;

        if (cards.Count == 1) {
            rank = cards[0].GetCardRank();
            return CardType.Single;
        }

        if (cards.Count == 2 && cards[0].IsJoker() && cards[1].IsJoker()) {
            return CardType.JokerBomb;
        }

        if (cards.Count == 4 && cards[0].GetCardRank() == cards[3].GetCardRank()) {
            rank = cards[0].GetCardRank();
            return CardType.Bomb;
        }
        if (cards.Count == 2 && cards[0].GetCardRank() == cards[1].GetCardRank()) {
            rank = cards[0].GetCardRank();
            return CardType.Pair;
        }
        if (cards.Count == 3 && cards[0].GetCardRank() == cards[2].GetCardRank()) {
            rank = cards[0].GetCardRank();
            return CardType.Three;
        }

        if (cards.Count == 4 && (cards[0].GetCardRank() == cards[2].GetCardRank() || cards[1].GetCardRank() == cards[3].GetCardRank())) {
            rank = (cards[0].GetCardRank() == cards[2].GetCardRank() ? cards[0].GetCardRank() : cards[1].GetCardRank());
            return CardType.ThreeWithOne;
        }

        if (cards.Count == 5 && ((cards[0].GetCardRank() == cards[2].GetCardRank() && cards[3].GetCardRank() == cards[4].GetCardRank()) || (cards[0].GetCardRank() == cards[1].GetCardRank() && cards[2].GetCardRank() == cards[4].GetCardRank()))) {
            rank = (cards[0].GetCardRank() == cards[2].GetCardRank() ? cards[0].GetCardRank() : cards[2].GetCardRank());
            return CardType.ThreeWithPair;
        }
        // // 将牌从小到大排序
        cards.Sort((a, b) => a.GetCardRank() - b.GetCardRank());
        if(IsSingleStraight(cards, out rank))
        {
            return CardType.Straight;
        }
         if(IsDoubleStraight(cards, out rank))
        {
            return CardType.StraightPair;
        }
         if(IsTripleStraight(cards, out rank))
        {
            return CardType.StraightThree;
        }
        // if (IsSingleStraight(cards, out rank)) {
        //     int length = cards.Count;
        //     if (length >= 5 && length == rank - cards[0].GetCardRank() + 1) {
        //         if (cards[0].GetCardRank() > 10) {
        //             return CardType.Straight;
        //         } else if (cards[0].GetCardRank() == 10) {
        //             return CardType.StraightPair;
        //         } else {
        //             return CardType.StraightThree;
        //         }
        //     }
        // }

        return CardType.None;
    }
    // 判断是否是单顺，返回顺子的分值
    public static bool IsSingleStraight(List<Card> cards,out int endValue)
    {
        endValue = 0;
        int count = cards.Count;
        if (count < 5 || count > 12) return false;

        // // 将牌从小到大排序
        // cards.Sort();

        // 顺子的牌值范围
        int startValue = cards[0].GetCardRank();
        endValue = cards[count - 1].GetCardRank();
        if (startValue < 3 || endValue > 14 || startValue == 14) 
        {
            endValue = 0;
            return false;
        }

        // 判断是否连续
        for (int i = 0; i < count - 1; i++)
        {
            if (cards[i + 1].GetCardRank() - cards[i].GetCardRank() != 1) 
            {
                endValue = 0;
                return false;
            }
        }

        // 是单顺
        return true;
    }

    // 判断是否是双顺，返回顺子的分值
    public static bool IsDoubleStraight(List<Card> cards, out int endValue)
    {
        endValue = 0;
        int count = cards.Count;
        if (count < 6 || count % 2 != 0)  {
            endValue = 0;
            return false;
        }

        // // 将牌从小到大排序
        // cards.Sort();

        // 顺子的牌值范围
        int startValue = cards[0].GetCardRank();
        endValue = cards[count - 1].GetCardRank();
        if (startValue < 3 || endValue > 14 || startValue == 14) {
            endValue = 0;
            return false;
        }

        // 判断是否连续
        for (int i = 0; i < count - 2; i += 2)
        {
            // 334455
            if (cards[i + 1].GetCardRank() != cards[i].GetCardRank() || cards[i + 2].GetCardRank() - cards[i].GetCardRank() != 1) 
            {
                endValue = 0;
                return false;
            }
        }

        // 是双顺
        return true;
    }

    // 判断是否是三顺，返回顺子的分值
    public static bool IsTripleStraight(List<Card> cards,out int endValue)
    {
        int count = cards.Count;
        if (count < 9 || count % 3 != 0) {
            endValue = 0;
            return false;
        }

        // // 将牌从小到大排序
        // cards.Sort();

        // 顺子的牌值范围
        int startValue = cards[0].GetCardRank();
        endValue = cards[count - 1].GetCardRank();
        if (startValue < 3 || endValue > 14 || startValue == 14) {
            endValue = 0;
            return false;
        }

        // 判断是否连续
        for (int i = 0; i < count - 3; i += 3)
        {
            if (cards[i + 1].GetCardRank() != cards[i].GetCardRank() || cards[i + 2].GetCardRank() != cards[i].GetCardRank()
                || cards[i + 3].GetCardRank() - cards[i].GetCardRank() != 1) 
            {
                endValue = 0;
                return false;
            }
        }

        // 是三顺
        return true;
    }

    // 判断是否为顺子并返回顺子长度和最大点数
    public static bool IsStraight(List<Card> cards, out int rank) {
        rank = 0;

        int length = cards.Count;
        if (length < 5 || cards[length - 1].GetCardRank() > 14) {
            return false;
        }

        for (int i = 0; i < length - 1; i++) {
            if (cards[i + 1].GetCardRank() - cards[i].GetCardRank() != 1 || cards[i].GetCardRank() > 14) {
                return false;
            }
        }

        rank = cards[length - 1].GetCardRank();
        return true;
    }

    // 获取所有符合牌型的牌的方法
    public static List<List<Card>> GetAllCardsByType(List<Card> hand, CardType type, List<Card> lastPlayed)
    {
        List<List<Card>> result = new List<List<Card>>();
        int count = hand.Count;
        int lastCount = lastPlayed?.Count ?? 0;
        int lastValue = lastCount > 0 ? lastPlayed[0].GetCardRank() : -1;
        
        switch (type)
        {
            case CardType.Single:
                for (int i = 0; i < count; i++)
                {
                    if (hand[i].GetCardRank() > lastValue)
                    {
                        result.Add(new List<Card>() { hand[i] });
                    }
                }
                break;

            case CardType.Pair:
                for (int i = 0; i < count - 1; i++)
                {
                    if (hand[i].GetCardRank() == hand[i + 1].GetCardRank() && hand[i].GetCardRank() > lastValue)
                    {
                        result.Add(new List<Card>() { hand[i], hand[i + 1] });
                    }
                }
                break;

            case CardType.Three:
                for (int i = 0; i < count - 2; i++)
                {
                    if (hand[i].GetCardRank() == hand[i + 1].GetCardRank() && hand[i].GetCardRank() == hand[i + 2].GetCardRank() && hand[i].GetCardRank() > lastValue)
                    {
                        result.Add(new List<Card>() { hand[i], hand[i + 1], hand[i + 2] });
                    }
                }
                break;

            case CardType.Straight:
                List<List<Card>> CanStraightCards = GetCanStraights(hand,lastPlayed);
                foreach (var item in CanStraightCards)
                {
                    result.Add(item);
                }
                break;

            case CardType.StraightPair:
                if(lastCount!=0)
                {
                    for (int i = 0; i < count - lastCount + 1; i += 2)
                    {
                        if (hand[i].GetCardRank() > lastValue && GetDoubleStraightLength(hand, i) == lastCount / 2)
                        {
                            result.Add(GetDoubleStraight(hand, i));
                        }
                    }
                }
                break;

            case CardType.StraightThree:
                if(lastCount!=0)
                {
                    for (int i = 0; i < count - lastCount + 1; i += 3)
                    {
                        if (hand[i].GetCardRank() > lastValue && GetTripleStraightLength(hand, i) == lastCount / 3)
                        {
                            result.Add(GetTripleStraight(hand, i));
                        }
                    }
                }
                break;

            case CardType.Bomb:
                for (int i = 0; i < count - 3; i++)
                {
                    if (hand[i].GetCardRank() == hand[i + 1].GetCardRank() && hand[i].GetCardRank() == hand[i + 2].GetCardRank() && hand[i].GetCardRank() == hand[i + 3].GetCardRank() && hand[i].GetCardRank() > lastValue)
                    {
                        result.Add(new List<Card>() { hand[i], hand[i + 1], hand[i + 2], hand[i + 3] });
                    }
                }
                break;

            case CardType.JokerBomb:
                Card Joker1Card = new Card(Card.Suit.Jokers,Card.Rank.Joker1);
                Card Joker2Card = new Card(Card.Suit.Jokers,Card.Rank.Joker2);
                if (hand.Contains(Joker1Card) && hand.Contains(Joker2Card))
                {
                    result.Add(new List<Card>() { Joker1Card, Joker2Card });
                }
                break;
        }

        return result;
    }
    static List<Card> GetMaxStraights(List<Card> cards,List<Card> lastCards)
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
    static private int getMax(List<Card> cards)
    {
        int max = -1;
        foreach (var card in cards)
        {
            max = card.GetCardRank()>max?card.GetCardRank():max; 
        }
        return max;
    }
    static List<List<Card>> GetCanStraights(List<Card> cards, List<Card> lastCards)
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
    // 获取顺子的长度
    private static int GetStraightLength(List<Card> hand, int startIndex)
    {
        int count = 1;
        for (int i = startIndex; i < hand.Count - 1; i++)
        {
            if (hand[i].GetCardRank() - hand[i + 1].GetCardRank() != 1&&hand[i].GetCardRank() != hand[i + 1].GetCardRank())
            {
                break;
            }
            if(hand[i].GetCardRank() != hand[i + 1].GetCardRank())
           {
                 count++;
           }
        }
        return count;
    }

    // 获取顺子
    private static List<Card> GetStraight(List<Card> hand, int startIndex)
    {
        List<Card> result = new List<Card>();
        int length = GetStraightLength(hand, startIndex);
         for (int i = startIndex; i < hand.Count - 1; i++)
        {
            if (hand[i].GetCardRank() - hand[i + 1].GetCardRank() != 1&&hand[i].GetCardRank() != hand[i + 1].GetCardRank())
            {
                break;
            }
            if(hand[i].GetCardRank() != hand[i + 1].GetCardRank())
           {
                result.Add(hand[i]);
           }
        }
        return result;
    }

    // 获取连对的长度
    private static int GetDoubleStraightLength(List<Card> hand, int startIndex)
    {
        int count = 1;
        for (int i = startIndex; i < hand.Count - 2; i += 2)
        {
            if (hand[i].GetCardRank() != hand[i + 1].GetCardRank() || hand[i].GetCardRank() - hand[i + 2].GetCardRank() != 1)
            {
                break;
            }
            count++;
        }
        return count;
    }

    // 获取连对
    private static List<Card> GetDoubleStraight(List<Card> hand, int startIndex)
    {
        List<Card> result = new List<Card>();
        int length = GetDoubleStraightLength(hand, startIndex);
        for (int i = startIndex; i < startIndex + length * 2; i++)
        {
            result.Add(hand[i]);
        }
        return result;
    }

    // 获取飞机的长度
    private static int GetTripleStraightLength(List<Card> hand, int startIndex)
    {
        int count = 1;
        for (int i = startIndex; i < hand.Count - 3; i += 3)
        {
            if (hand[i].GetCardRank() != hand[i + 1].GetCardRank() || hand[i].GetCardRank() != hand[i + 2].GetCardRank() || hand[i].GetCardRank() - hand[i + 3].GetCardRank() != 1)
            {
                break;
            }
            count++;
        }
        return count;
    }

    // 获取飞机
    private static List<Card> GetTripleStraight(List<Card> hand, int startIndex)
    {
        List<Card> result = new List<Card>();
        int length = GetTripleStraightLength(hand, startIndex);
        for (int i = startIndex; i < startIndex + length * 3; i++)
        {
            result.Add(hand[i]);
        }
        return result;
    }
}