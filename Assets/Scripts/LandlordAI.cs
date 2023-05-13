using System.Collections.Generic;
using System.Collections;
public partial class AIplayer:Player
{
    public void CallLandlord()
    {
        // 获取手牌中每种牌面的数量
        int[] cardCounts = new int[15]; // 数组下标0表示没有牌，1-13表示对应牌面的数量，14表示王的数量

        foreach (Card card in playerCards)
        {
            if (card.GetCardRank() >= 1 && card.GetCardRank() <= 13)
            {
                cardCounts[card.GetCardRank()]++;
            }
            else if (card.GetCardRank() == 14 || card.GetCardRank() == 15)
            {
                cardCounts[14]++;
            }
        }

        // 判断是否叫地主
        shouldCallLandlord = false;

        // 根据牌面的数量进行判断
        // 这里的逻辑可以根据具体需求进行修改
        if (cardCounts[13] >= 2) // 如果手中有两个以上的K，则叫地主
        {
            shouldCallLandlord = true;
        }
        else if (cardCounts[13] == 1 && cardCounts[12] >= 2) // 如果有一个K和两个以上的Q，则叫地主
        {
            shouldCallLandlord = true;
        }
        else if (cardCounts[13] == 1 && cardCounts[11] >= 2 && cardCounts[12] >= 2) // 如果有一个K，两个以上的Q和两个以上的J，则叫地主
        {
            shouldCallLandlord = true;
        }
    }
}