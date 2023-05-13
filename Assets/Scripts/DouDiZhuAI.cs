using System.Collections.Generic;
using System;
public partial class AIplayer
{
    // 获取所有符合条件的牌
    public  Dictionary<CardType, List<List<Card>>> GetAllCandidates(List<Card> hand, List<Card> lastPlayed, out int AllCandidatesNum)
    {
        AllCandidatesNum = 0;
        Dictionary<CardType, List<List<Card>>> candidates = new Dictionary<CardType, List<List<Card>>>();

        if (lastPlayed == null || lastPlayed.Count == 0)
        {
            // List<List<Card>> canPlayCards = GetAllCardsByType(hand, CardType.Bomb, lastPlayed);
            // candidates[CardType.Bomb] = canPlayCards;
            // AllCandidatesNum += canPlayCards.Count;
            // canPlayCards = GetAllCardsByType(hand, CardType.JokerBomb, lastPlayed);
            // candidates[CardType.JokerBomb] = canPlayCards;
            // AllCandidatesNum += canPlayCards.Count;
            // // 首轮出牌，随便出
            // canPlayCards =(new List<List<Card>>{new List<Card>() { hand[0] }});
            // candidates[CardType.Single] = canPlayCards;
            // AllCandidatesNum += canPlayCards.Count;
            playCardType = PlayCardType.min;
            foreach (CardType cardtype in Enum.GetValues(typeof(CardType)))
            {
                //  if(cardtype == CardType.Straight || cardtype == CardType.StraightPair||cardtype == CardType.StraightThree)
                //      continue;
                List<List<Card>> canPlayCards = GetAllCardsByType(hand, cardtype, lastPlayed);
                candidates[cardtype] = canPlayCards;
                AllCandidatesNum += canPlayCards.Count;
            }
        }
        else
        {
            int RankNum = 0;
            // 根据上一轮出牌找到所有符合条件的牌
            CardType type = GetCardType(lastPlayed,out RankNum);
            List<List<Card>> canPlayCards = GetAllCardsByType(hand, type, lastPlayed);
            LogOut.Instance.Print("上家牌的类型："+type.ToString());
            LogOut.Instance.Print("获取可以打的牌型的个数："+canPlayCards.Count.ToString());
            candidates[type]=canPlayCards;
            AllCandidatesNum += canPlayCards.Count;
            if (type != CardType.JokerBomb && type != CardType.Bomb)
            {
                canPlayCards = GetAllCardsByType(hand, CardType.Bomb, lastPlayed);
                candidates[CardType.Bomb]=(canPlayCards);
                AllCandidatesNum += canPlayCards.Count;
                canPlayCards.Clear();
                canPlayCards = GetAllCardsByType(hand, CardType.JokerBomb, lastPlayed);
                candidates[CardType.JokerBomb]=(canPlayCards);
                AllCandidatesNum += canPlayCards.Count;
            }
            // foreach (CardType cardtype in Enum.GetValues(typeof(CardType)))
            // {
            //     List<List<Card>> canPlayCards = GetAllCardsByType(hand, cardtype, lastPlayed);
            //     candidates[cardtype] = canPlayCards;
            //     AllCandidatesNum += canPlayCards.Count;
            // }
        }
        return candidates;
    }
    private  List<Card> getCards(Dictionary<CardType, List<List<Card>>> candidates, CardType type)
    {
        if(candidates[type].Count>0)
        {
            if(playCardType == PlayCardType.min)
            {
                candidates[type].Sort((a, b) => GetScore(a) - GetScore(b));
            }
            else
            {
                candidates[type].Sort((a, b) => GetScore(b) - GetScore(a));
            }
            if(candidates[type][0].Count>0)
                return candidates[type][0];
            else
                return null;
        }
        else
        {
            return null;
        }
    }
    // 选择最佳出牌
    public  List<Card> ChooseBestCandidate(Dictionary<CardType, List<List<Card>>> candidates, out CardType cardType)
    {
        // 如果有炸弹，先出炸弹；
        // 如果手上有单张牌，优先出单张牌；
        // 如果手上有对子，优先出对子；
        // 如果手上有三带一、三带二，优先出三带一、三带二；
        // 如果手上有顺子、连对、飞机等组合牌型，优先出组合牌型；
        // 如果手上有大于当前出牌的同类型牌，出最小的那张；
        // 如果手上没有符合条件的牌，则不出牌。

        // 火箭（双王）：最大的牌，可以随时出
        // 炸弹：四张同数值牌，比其他牌型大
        // 单牌、对牌、三张牌、四张牌：依次按牌值从大到小出牌
        // 顺子、连对、飞机：依次按牌数和最大牌值从大到小出牌
        if(candidates.ContainsKey(CardType.JokerBomb))
        if (candidates[CardType.JokerBomb].Count > 0)
        {
            cardType = CardType.JokerBomb;
            return getCards(candidates, CardType.JokerBomb);
        }
        if(candidates.ContainsKey(CardType.Bomb))
        if (candidates[CardType.Bomb].Count > 0)
        {
            cardType = CardType.Bomb;
            return getCards(candidates, CardType.Bomb);
        }

        List<List<Card>> rankCards = new List<List<Card>>();
        if(candidates.ContainsKey(CardType.Straight))
        if (candidates[CardType.Straight].Count > 0)
        {
            cardType = CardType.Straight;
            List<Card> cards = getCards(candidates, CardType.Straight);
            if(cards!=null)
                rankCards.Add(cards);
        }
        if(candidates.ContainsKey(CardType.StraightPair))
        if (candidates[CardType.StraightPair].Count > 0)
        {
            cardType = CardType.StraightPair;
            List<Card> cards = getCards(candidates, CardType.StraightPair);
            if(cards!=null)
                rankCards.Add(cards);
        }
        if(candidates.ContainsKey(CardType.StraightThree))
        if (candidates[CardType.StraightThree].Count > 0)
        {
            cardType = CardType.StraightThree;
            List<Card> cards = getCards(candidates, CardType.StraightThree);
            if(cards!=null)
                rankCards.Add(cards);
        }
        if (rankCards.Count > 0)
        {
            if(playCardType == PlayCardType.max)
            {
                rankCards.Sort((a, b) => GetScore(b) - GetScore(a));
            }
            else
            {
                rankCards.Sort((a, b) => GetScore(a) - GetScore(b));
            }
            int rank = 0;
            foreach (var item in rankCards)
            {                
                LogOut.print("Straight: " + string.Join(", ", item)+item.Count);
            }        
            cardType = GetCardType(rankCards[0], out rank);
            return rankCards[0];
        }
          // 
        if(candidates.ContainsKey(CardType.ThreeWithOne))
        if (candidates[CardType.ThreeWithOne].Count > 0)
        {
            cardType = CardType.ThreeWithOne;
            List<Card> cards = getCards(candidates, CardType.ThreeWithOne);
            if(cards!=null)
                rankCards.Add(cards);
        }
        if(candidates.ContainsKey(CardType.ThreeWithPair))
        if (candidates[CardType.ThreeWithPair].Count > 0)
        {
            cardType = CardType.ThreeWithPair;
            List<Card> cards = getCards(candidates, CardType.ThreeWithPair);
            if(cards!=null)
                rankCards.Add(cards);
        }
        if(candidates.ContainsKey(CardType.Three))
        if (candidates[CardType.Three].Count > 0)
        {
            cardType = CardType.Three;
            List<Card> cards = getCards(candidates, CardType.Three);
            if(cards!=null)
                rankCards.Add(cards);
        }

         if (rankCards.Count > 0)
        {
            if(playCardType == PlayCardType.max)
            {
                rankCards.Sort((a, b) => GetScore(b) - GetScore(a));
            }
            else
            {
                rankCards.Sort((a, b) => GetScore(a) - GetScore(b));
            }
            int rank = 0;
            cardType = GetCardType(rankCards[0], out rank);
            return rankCards[0];
        }
        //
        if(candidates.ContainsKey(CardType.Pair))
        if (candidates[CardType.Pair].Count > 0)
        {
            cardType = CardType.Pair;
            List<Card> cards = getCards(candidates, CardType.Pair);
            if(cards!=null)
                return cards;
        }
        if(candidates.ContainsKey(CardType.Single))
        if (candidates[CardType.Single].Count > 0)
        {
            cardType = CardType.Single;
            List<Card> cards = getCards(candidates, CardType.Single);
            if(cards!=null)
                return cards;
        }
       

        cardType = CardType.None;
        // 简单地选择第一个出牌
        return new List<Card>();
    }
    // private bool CanBid() {
    //     // 判断是否要叫分
    //     // ...
    // }

    // private List<Card> GetPlayableCards(List<Card> myCards, List<Card> lastPlayedCards) {
    //     // 获取可出的牌
    //     // ...
    // }

    // private List<Card> ChooseCardsToPlay(List<Card> playableCards) {
    //     // 选择要出的牌
    //     // ...
    // }
}