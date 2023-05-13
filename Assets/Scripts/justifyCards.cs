// class justifyCards{
//     public bool HasRocket(List<Card> handCards)
//     {
//         // 查找大王和小王的数量
//         int countBigJoker = handCards.Count(card => card.CardSuit == CardSuit.Joker && card.CardPoint == CardPoint.BigJoker);
//         int countSmallJoker = handCards.Count(card => card.CardSuit == CardSuit.Joker && card.CardPoint == CardPoint.SmallJoker);

//         // 如果同时存在大王和小王，则表示有火箭
//         return countBigJoker > 0 && countSmallJoker > 0;
//     }
//     public bool ContainsBomb(List<Card> handCards)
//     {
//         Dictionary<CardNumber, int> cardDict = new Dictionary<CardNumber, int>();
//         foreach (Card card in handCards)
//         {
//             if (cardDict.ContainsKey(card.Number))
//             {
//                 cardDict[card.Number] += 1;
//             }
//             else
//             {
//                 cardDict[card.Number] = 1;
//             }
//         }

//         foreach (KeyValuePair<CardNumber, int> pair in cardDict)
//         {
//             if (pair.Value == 4)
//             {
//                 return true;
//             }
//         }

//         return false;
//     }
// }