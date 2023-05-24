using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class HumanPlayer : Player
{
    public HumanPlayer(): base()
    {
        type = playerType.player; 

    }
    
    public Button NoCardsButton;
    public Button PlayingCardsButton;
    public Button NoCallButton;
    public Button CallButton;
    public Button NoGrabButton;
    public Button GrabButton;
    private TextMeshProUGUI NoCardsButtonText;
    private TextMeshProUGUI PlayingCardsButtonText;
    // static
    public static List<GameObject> playerChooseObj;
    // private void Awake() {  
    //     // if(PlayerList==null)
    //     // PlayerList = new List<Player>();      
    // }
    protected override void Awake()
    {
        base.Awake();
        // 在子类的 Awake() 方法中调用父类的 Awake() 方法，此时父类的列表已经初始化
        // 可以在这里进行进一步的处理
        AddToList();        
    }
    private void Start() {
        playerChooseObj = new List<GameObject>();
        //button link
        NoCardsButton.onClick.AddListener(NoCardsOnClick);
        PlayingCardsButton.onClick.AddListener(PlayingCardsOnClick);
        NoCallButton.onClick.AddListener(()=>{
            NoCall();
            showUI.CloseAllUIObjects();            
            Timer.instance.StopTimer();
        });
        CallButton.onClick.AddListener(()=>{
            Call();
            showUI.CloseAllUIObjects();
            Timer.instance.StopTimer();
        });
        NoGrabButton.onClick.AddListener(()=>{
            NoGrab();
            showUI.CloseAllUIObjects();            
            Timer.instance.StopTimer();
        });
        GrabButton.onClick.AddListener(()=>{
            if(CanGrab())
            {
                Grab();
            }
            showUI.CloseAllUIObjects();
            Timer.instance.StopTimer();
        });
    }
    public List<Card> ObjToChooseCard(List<GameObject> chooseObjs)
    {
        List<Card> played = new List<Card>();
        foreach (var chooseObj in chooseObjs)
        {
            int index = playerObjs.IndexOf(chooseObj);
            played.Add(playerCards[index]);
        }
        return played;
    }
    private void NoCardsOnClick()
    {
        // Debug.Log("游戏玩家不发牌转台");
        //lastPlayed.Clear();
        // changge
        // playerIndex++;
        // playerIndex = playerIndex % 3;
        // passNum++;
        PassChangeIndex();
        Timer.instance.StopTimer();
        UItimer.instance.StopAllCoroutines();
        showUI.GetTip().text = "Pass";
        showUI.OpenTip();
        playTypeSound("pass");
    }
    private void RemoveCards(List<Card> played, List<GameObject> chooseObjs)
    {
        for (int i = 0; i < played.Count; i++)
        {
            Card card = played[i];
            GameObject chooseObj = chooseObjs[i];
            //删除player中的Card
            playerCards.Remove(card);
            // 删除显示的Card
            Destroy(chooseObj);
            playerObjs.Remove(chooseObj);
        }
    }
   
    private void PlayingCardsOnClick()
    {
        //删除player中的Card
        // 删除显示的Card
        List<Card> playerplayed = ObjToChooseCard(playerChooseObj);
        CardType handType;
        if (CanPlay(playerplayed, lastPlayed,out handType))
        {
            Debug.Log(handType);
            // Debug.Log("游戏玩家发牌转台");
            RemoveCards(playerplayed, playerChooseObj);
            lastPlayed = playerplayed;
            ShowOneceCards(playerplayed);
            if (playerCards.Count == 0)
            {
                // isGameOver = true;
                Timer.instance.StopAllCoroutines();
                // Debug.Log("Game over.");
                stopBGM();
                playSound("胜利");
                Subject.Instance.Notify(Subject.EventName.GameOver);
                return;
            }
            ChangeIndex();
            playerChooseObj.Clear();
            Timer.instance.StopTimer();
            UItimer.instance.StopAllCoroutines();
            playSound("出牌");
            playTypeSound(handType,playerplayed);
            showUI.CloseAllUIObjects();
        }
        else
        {
            // GameObject single = showTip.transform.GetChild(0).gameObject;
            // GameObject text = showTip.transform.GetChild(1).gameObject;
            // single.SetActive(false);
            // text.SetActive(true);
            // showTip.gameObject.SetActive(true);
            TextMeshProUGUI tipText = showUI.GetTip();
            tipText.text = "不合法";
            showUI.OpenNormalTip();
            UItimer.instance.StartTime(2,()=>{
                // single.SetActive(true);
                // text.SetActive(false);
                showUI.CloseNormalTip();
            });
        }
    }
    //  private void ShowOneceCards(List<Card> playerCards)
    // {
    //     foreach (var obj in ShowOneceCardObjs)
    //     {
    //         Destroy(obj);
    //     }
    //     ShowOneceCardObjs.Clear();
    //     for (int i = 0; i < playerCards.Count; i++)
    //     {
    //         Card card = playerCards[i];
    //         GameObject obj = Load.instance.LoadObj("othersCard");
    //         int index = card.indexOfSprite();
    //         Sprite sp = Sprites[index];
    //         obj.GetComponent<Image>().sprite = sp;
    //         obj.transform.parent = leftOverparent;
    //         obj.transform.localRotation = Quaternion.identity;
    //         ShowOneceCardObjs.Add(obj);
    //     }
    // }
    // 获取所有符合条件的牌
    public static Dictionary<CardType, List<List<Card>>> GetAllCandidates(List<Card> hand, List<Card> lastPlayed, out int AllCandidatesNum)
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
    public override void CallGame()
    {
        showUI.OpenCallButton();
        base.CallGame();
        Timer.instance.StartTimer(10,()=>{
            if(shouldCallLandlord)
                Call();
            else
                NoCall();
            showUI.CloseAllUIObjects();
        });
    }
    public override void GrabGame()
    {
        if(CanGrab())
        {
            showUI.OpenGrabButton();
            base.GrabGame();
            Timer.instance.StartTimer(10,()=>{
            if(shouldGrabLandlord)
                    Grab();
                else
                    NoGrab();
                showUI.CloseAllUIObjects();
            });
        }
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
            showUI.OpenButton();
            setShowTip();
            if (AllCandidatesNum == 0)
            {
                showUI.setButtonDisable();
                Debug.Log("人类玩家"+playerIndex+"Pass.");  
                Timer.instance.StartTimer(2,() =>{
                    UItimer.instance.StopAllCoroutines();
                    Debug.Log("玩家"+playerIndex+" Pass");
                    // Debug.Log("玩家"+playerIndex+" 转台");
                    //lastPlayed.Clear();
                    PassChangeIndex();
                    showUI.OpenTip();
                    showUI.GetTip().text = "Pass";
                    playTypeSound("pass-1");
                });              
            }
            else
            {
                // 游戏玩家
                // Debug.Log("游戏玩家");
                Timer.instance.StartTimer(30,() =>{
                    // Debug.Log("游戏玩家转台");
                    //lastPlayed.Clear();
                    UItimer.instance.StopAllCoroutines();
                    Debug.Log("Pass");
                    PassChangeIndex();
                    showUI.GetTip().text = "Pass";
                    showUI.OpenTip();
                    playTypeSound("pass");
                });
            }
        }
    }
}