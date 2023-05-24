using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public partial class AIplayer:Player
{
     public AIplayer(): base()
    {
        type = playerType.dealer;
        
    }
    // private void Awake() {    
    //     //  if(PlayerList==null)
    //     //     PlayerList = new List<Player>();      
    // }
    protected override void Awake()
    {
        base.Awake();
        // 在子类的 Awake() 方法中调用父类的 Awake() 方法，此时父类的列表已经初始化
        // 可以在这里进行进一步的处理
        AddToList();  
    }
    private void Start() {
        Subject.Instance.Attach(Subject.EventName.DealCardsFirstOver,CallLandlord);
    }
    private void RemoveRangeCards(int startIndex, int delCount, List<GameObject> playerObjs)
    {
        for (int i = startIndex; i < startIndex + delCount; i++)
        {
            Destroy(playerObjs[i]);
        }
        playerObjs.RemoveRange(startIndex, delCount);
    }
    public override void CallGame()
    {
        base.CallGame();
        Timer.instance.StartTimer(3,()=>{
            if(shouldCallLandlord)
            {
                Call();
            }
            else
            {
                NoCall();
            }
            showUI.CloseAllUIObjects();
        });

    }
    public override void GrabGame()
    {
        base.GrabGame();
        Timer.instance.StartTimer(3,()=>{
            if(CanGrab())
            {   
                Grab();
            }
            else
            {
                NoGrab();
            }
            showUI.CloseAllUIObjects();
        });
    }
    public override void Handle()
    {
        Timer.instance.StopAllCoroutines();
        if (playerCards.Count > 0)
        {
            if(passNum == 2)
            {
                passNum = 0;
                Debug.Log("清除");
                lastPlayed.Clear();
            }
            // Debug.Log("Player " + (player == playerCards ? "1" : player == dealerCards1 ? "2" : "3") + "'s turn:");
            AllCandidatesNum = 0;
            Dictionary<CardType, List<List<Card>>> candidates = GetAllCandidates(playerCards, lastPlayed, out AllCandidatesNum);
            setShowTip();
            if (AllCandidatesNum == 0)
            {
                // Debug.Log("Pass.");  
                Timer.instance.StartTimer(2,() =>{
                    Debug.Log("玩家"+playerIndex+" Pass");
                    // Debug.Log("玩家"+playerIndex+" 转台");
                    //lastPlayed.Clear();
                    PassChangeIndex();
                    showUI.CloseTimerAndOpenTip();
                    showUI.GetTip().text = "Pass";
                    playTypeSound("pass-1");
                });              
            }
            else
            {
                Timer.instance.StartTimer(5,() =>{
                    // AI玩家
                    CardType handType;
                    List<Card> played = ChooseBestCandidate(candidates,out handType);
                    Debug.Log("AI玩家牌型: "+handType);
                    if(played.Count==0|| handType == CardType.None)
                    {
                        Debug.Log("Pass");
                        PassChangeIndex();
                        showUI.CloseTimerAndOpenTip();
                        showUI.GetTip().text = "Pass";
                        playTypeSound("pass-1");
                        return;
                    }
                    playTypeSound(handType,played);
                    // Debug.Log("AI玩家"+playerIndex+ string.Join(" ", played));
                    int index = playerCards.FindIndex(x => x == played[0]);
                    // Debug.Log(string.Format("第{0}位，出牌数{1}",playerIndex,played.Count));
                    // Debug.Log("index:"+index);
                    playerCards.RemoveRange(index, played.Count);
                    RemoveRangeCards(index, played.Count, playerObjs);
                    lastPlayed = played;
                    ShowOneceCards(played);
                    if (playerCards.Count == 0)
                    {
                        Env.Instance.isGameOver = true;
                        Timer.instance.StopAllCoroutines();
                        stopBGM();
                        playSound("失败");
                        Subject.Instance.Notify(Subject.EventName.GameOver);
                        // Debug.Log("Game over.");
                        return;
                    }
                    showUI.CloseAllUIObjects();
                    // Debug.Log("AI玩家"+playerIndex+"转台");
                    ChangeIndex();
                });  
            
            }
            // player = player == playerCards ? dealerCards1 : player == dealerCards1 ? dealerCards2 : playerCards;      
        }
    }
}