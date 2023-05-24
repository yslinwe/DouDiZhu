using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player : DoudiZhuRule
{
    public static Sprite [] Sprites;
    public static List<Card> lastPlayed;
    public static List<GameObject> ShowOneceCardObjs;
    public static List<GameObject> leftCardObjs;
    public static List<Player> PlayerList;
    private static List<Player> CallPlayerList;
    public List<Card> playerCards;
    [HideInInspector]
    public List<GameObject> playerObjs;
    public static int playerIndex = 1;
    private static int CallplayerIndex = 1;
    public static int passNum = 0;
    public Transform leftOverparent;
    public GameObject showTip;
    public GameObject showInfo;
    public Transform playerparent;
    // opencards
    public RectTransform[] imageRectTransforms;
    public float animationDuration = 1f;
    public float imageSpaceDistance = 50f;
    //
    protected ShowTip showUI;
    protected ShowInfo showInfoUI;
    public Sex sex;
    public int landlordIndex = -1; // 地主索引，0：地主上家，1：地主，2：地主下家
    public bool shouldCallLandlord = false;
    public bool shouldGrabLandlord = false;
    public bool isGrab = false;
    public Player lastPlayer;
    private ImageLayout imageLayout;
    public enum Sex{
        M,
        F
    }
    public enum PlayCardType{
        min,
        max
    }
    public enum CharcaterType
    {
        landlord,
        farmer
    }
    public PlayCardType playCardType; 
    [HideInInspector]
    public int AllCandidatesNum;
    public Player() {
        if(PlayerList==null)
        {    
            PlayerList = new List<Player>();        
        }
    }
    public void AddToList()
    {
        if(!PlayerList.Contains(this))
            PlayerList.Add(this);
    }
    protected virtual void  Awake() {
        Debug.Log("初始化");
        leftCardObjs = new List<GameObject>();
        playerCards = new List<Card>();
        CallPlayerList = new List<Player>();
        lastPlayed = new List<Card>();
        ShowOneceCardObjs = new List<GameObject>();
        playerObjs = new List<GameObject>();
        playerparent = transform.GetChild(0);
        showUI = showTip.GetComponent<ShowTip>();
        showInfoUI = showInfo.GetComponent<ShowInfo>();
        imageLayout= playerparent.GetComponent<ImageLayout>();
    }

    public enum playerType
    {
        player,
        dealer
    }
    public  playerType type;
    // public Player()
    // {
    //     if(PlayerList.Count<3)
    //         PlayerList.Add(this);
    // }
    public virtual void CallGame()
    {
        Timer.instance.StopAllCoroutines();
        setShowTip();
    }
    public virtual void GrabGame()
    {
        Timer.instance.StopAllCoroutines();
        setShowTip();
    }
    public void Call() 
    {
        if(PlayerList.Count<3)
        {
            throw new SystemException("玩家必须是三个人");
        }
        landlordIndex = 1;
        int index = PlayerList.FindIndex(x=>x==this);
        // 2 0 1
        // 0 1 2
        // 1 2 0
        // 上家  
        PlayerList[(index+2)%3].Change(0);
        // 下家
        PlayerList[(index+1)%3].Change(2);
        playTypeSound("call");
        CallPlayerList.Add(this);
    }
    private void Change(int index)
    {
        landlordIndex = index;
    }
    public void NoCall()
    {
        playTypeSound("nocall");
    }
    public bool CanGrab()
    {
        int callindex = CallPlayerList.FindIndex(x=>x==this);
        Debug.Log("callindex:"+callindex);
        switch (callindex)
        {
            case 0:
                return true;
            case 1:
                //  上家有没有抢
                return !CallPlayerList[0].isGrab;
            case 2:
                //  上上家有没有抢
                return !CallPlayerList[0].isGrab&&!CallPlayerList[1].isGrab;
            default:
                return true;
        }
    }
    public void Grab()
    {
        isGrab = true;
        landlordIndex = 1;
        int index = PlayerList.FindIndex(x=>x==this);
        // 2 0 1
        // 0 1 2
        // 1 2 0
        // 上家  
        PlayerList[(index+2)%3].Change(0);
        // 下家
        PlayerList[(index+1)%3].Change(2);
        playTypeSound("grab");
    }
    public void NoGrab()
    {
        playTypeSound("nograb");
    }
     public static void DealCardsOnbyOne()
    {
        Sprites = Load.instance.LoadSprite("poker");
        int CardLengths = 17;
        if(Deck.Instance.getCardsCount()<17)
        {
            CardLengths = Deck.Instance.getCardsCount();

        }
        for (int i = 0; i < CardLengths; i++)
        {
            foreach (var player in PlayerList)
            {    
                Card card = Deck.Instance.Deal();
                player.playerCards.Add(card);
                string objName = player.type == playerType.player ? "playerCard" : "othersCard";
                GameObject obj = Load.instance.LoadObj(objName);
                int index = card.indexOfSprite();
                Sprite sp = Sprites[index];
                obj.GetComponent<Image>().sprite = sp;
                obj.transform.parent = player.playerparent;
                obj.transform.position=player.playerparent.position;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                player.playerObjs.Add(obj);
                // 不是玩家
                if(playerType.dealer == player.type)
                    player.showInfoUI.UpdateCardNum(player.PlayerLeftCardNum);
            }
        }
        Subject.Instance.Notify(Subject.EventName.DealCardsFirstOver);
        foreach (var player in PlayerList)
        {    
            //排序
            player.playerCards.Sort((a, b) => b.GetCardRank() - a.GetCardRank());
            for (int i = 0; i < player.playerCards.Count; i++)
            {
                Card card = player.playerCards[i];
                GameObject obj = player.playerObjs[i];
                int index = card.indexOfSprite();
                Sprite sp = Sprites[index];
                obj.GetComponent<Image>().sprite = sp;
            }
            player.imageLayout.OpenCards();
            // UItimer.instance.StartTime(1,ImageLayout.Instance.OpenCards);
            Subject.Instance.Attach(Subject.EventName.SetShowInfo,()=>{
                SetShowInfo(player);            
            });
            // 不是玩家
            if(playerType.dealer == player.type)
            {
                player.showInfoUI.UpdateCardNum(player.PlayerLeftCardNum);
            }
        }
    }
    public void DealCards()
    {
        int CardLengths = 17;
        Sprites = Load.instance.LoadSprite("poker");
        if(Deck.Instance.getCardsCount()<17)
        {
            CardLengths = Deck.Instance.getCardsCount();
        }
        for (int i = 0; i < CardLengths; i++)
        {
            Card card = Deck.Instance.Deal();
            playerCards.Add(card);
            string objName = type == playerType.player ? "playerCard" : "othersCard";
            GameObject obj = Load.instance.LoadObj(objName);
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
            obj.transform.parent = playerparent;
            obj.transform.position=playerparent.position;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            playerObjs.Add(obj);
        }
        //排序
        playerCards.Sort((a, b) => b.GetCardRank() - a.GetCardRank());
        for (int i = 0; i < playerCards.Count; i++)
        {
            Card card = playerCards[i];
            GameObject obj = playerObjs[i];
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
        }
        imageLayout.CloseCards();

        // UItimer.instance.StartTime(1,ImageLayout.Instance.OpenCards);
        // Subject.Instance.Attach(Subject.EventName.GrabOver,()=>{
        //     if(landlordIndex==1)
        //     {
        //         imageLayout.CloseCards();
        //         showInfoUI.SetPlayerCharcater(CharcaterType.landlord);
        //         showInfoUI.SetPlayerImage(CharcaterType.landlord,sex);
        //     }
        //     else
        //     {
        //         showInfoUI.SetPlayerCharcater(CharcaterType.farmer);
        //         showInfoUI.SetPlayerImage(CharcaterType.farmer,sex);
        //     }
        // });
        // // 不是玩家
        if(playerType.dealer == type)
            showInfoUI.UpdateCardNum(PlayerLeftCardNum);
    }
     public void playTypeSound(CardType handType,List<Card> playerplayed)
    {
        string sexType = sex.ToString()+"_";
        // Debug.Log(sexType);
        switch (handType) {
            case CardType.Single:
                AudioManager.instance.PlaySound(sexType+playerplayed[0].soundName);
                break;
            case CardType.Pair:
                AudioManager.instance.PlaySound(sexType+"d"+playerplayed[0].soundName);
                break;
            case CardType.Three:
                AudioManager.instance.PlaySound(sexType+"30");
                break;
            case CardType.ThreeWithOne:
                AudioManager.instance.PlaySound(sexType+"31-1");
                break;
            case CardType.ThreeWithPair:
                AudioManager.instance.PlaySound(sexType+"32");
                break;
            case CardType.Straight:
                AudioManager.instance.PlaySound(sexType+"1s");
                break;
            case CardType.StraightPair:
                AudioManager.instance.PlaySound(sexType+"2s");
                break;
            case CardType.StraightThree:
                AudioManager.instance.PlaySound(sexType+"3s");
                break;
            case CardType.Bomb:
                AudioManager.instance.PlaySound(sexType+"bomb");
                break;
            case CardType.JokerBomb:
                AudioManager.instance.PlaySound(sexType+"rocket-1");
                break;
        }
    }
     public void playTypeSound(string Name)
    {
        string sexType = sex.ToString()+"_";
        // Debug.Log(sexType);
        AudioManager.instance.PlaySound(sexType+Name);
    }
    public void playSound(string Name)
    {
        AudioManager.instance.PlaySound(Name);
    }
    public void stopBGM()
    {
        AudioManager.instance.StopBGM("背景声");
    }
    public virtual void setShowTip()
    {
        
        
          
        // GameObject single = showTip.transform.GetChild(0).gameObject;
        // GameObject text = showTip.transform.GetChild(1).gameObject;
        // single.SetActive(false);
        // text.SetActive(false);
        showUI.OpenTimerAndCloseTip();
        Timer.instance.Timertext = showUI.GetTimerTip();
        // showUI.OpenTimerAndCloseTip();
        // Timer.instance.Timertext = showUI.GetTimerTip();
        // if (AllCandidatesNum == 0)
        // {
        //     // TextMeshProUGUI tipText = text.GetComponent<TextMeshProUGUI>();
        //     // showTip.SetActive(true);
        //     // text.SetActive(true);
        //     showUI.CloseTimerAndOpenTip();
        // }
        // else
        // {
        //     // showTip.SetActive(true);
        //     // single.SetActive(true);
        //     showUI.OpenTimerAndCloseTip();
        //     Timer.instance.Timertext = showUI.GetTimerTip();
        // }
    }
    public string PlayerLeftCardNum
    {
        get{
            return playerCards.Count.ToString("F0").PadLeft(2, '0');
        }
    }
    public void ShowOneceCards(List<Card> playedCards)
    {
        // 不是玩家
        if(playerType.dealer == type)
            showInfoUI.UpdateCardNum(PlayerLeftCardNum);

        foreach (var obj in ShowOneceCardObjs)
        {
            Destroy(obj);
        }
        ShowOneceCardObjs.Clear();
        for (int i = 0; i < playedCards.Count; i++)
        {
            Card card = playedCards[i];
            GameObject obj = Load.instance.LoadObj("othersCard");
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
            obj.transform.parent = leftOverparent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            ShowOneceCardObjs.Add(obj);
        }
    }
    public static void ShowLeftCards(Transform showLeftParent)
    {
        List<Card> leftCards = Deck.Instance.GetLeftCards();
        for (int i = 0; i < leftCards.Count; i++)
        {
            Card card = leftCards[i];
            GameObject obj = Load.instance.LoadObj("othersCard");
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
            obj.transform.parent = showLeftParent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            leftCardObjs.Add(obj);
        }
    }
    public virtual void Handle()
    {

    }
    public void PassChangeIndex()
    {
        lastPlayer = PlayerList[playerIndex];
        playerIndex++;
        playerIndex = playerIndex % 3;
        passNum++;
    }
    public void ChangeIndex()
    {
        lastPlayer = PlayerList[playerIndex];
        passNum = 0;
        playerIndex++;
        playerIndex = playerIndex % 3;
    }
    public static Player GetCallPlayer()
    {
        playerIndex = PlayerList.FindIndex(x=>x==CallPlayerList[CallplayerIndex]);
        return PlayerList[playerIndex];
    }
    public static void ChangeCallPlayerIndex()
    {
        CallplayerIndex++;
        CallplayerIndex = CallplayerIndex%CallPlayerList.Count;
    }
    public static Player GetPlayer()
    {
        return PlayerList[playerIndex];
    }
    public static void SetIndex(int index)
    {
        playerIndex = index;
    }
     public static int GetIndex()
    {
        return playerIndex;
    }
    public static int GetCallIndex()
    {
        return CallplayerIndex;
    }
    public static int GetPlayerCount()
    {
        return PlayerList.Count;
    }
    public static int GetCallPlayerCount()
    {
        return CallPlayerList.Count;
    }
    public static List<Player> GetPlayers()
    {
        return PlayerList;
    }
    private static void SetShowInfo(Player player)
    {
        if(player.landlordIndex==1)
        {
            player.showInfoUI.SetPlayerInfo(CharcaterType.landlord,player.sex);
        }
        else
        {
            player.showInfoUI.SetPlayerInfo(CharcaterType.farmer,player.sex);
        }
    }
    public static void ChangePlayersIndex()
    {
        // List<Player> newPlayers = new List<Player>(3);
        // for (int i = 0; i < PlayerList.Count; i++)
        // {
        //     Player player = PlayerList[i];
        //     int index = player.landlordIndex;
        //     Debug.Log(index+" "+i);
        //     newPlayers.Insert();
        // }
        // PlayerList = newPlayers;
        PlayerList.Sort((a, b) => a.landlordIndex - b.landlordIndex);
        playerIndex = 1; 
    }
    public static void RestartGame()
    {
        Env.Instance.isGameOver = true;
        Env.Instance.isCallOver = false;
        Env.Instance.isGrabOver = true;
        Env.Instance.StopGame = false; 
        Subject.Instance.Clear();
        foreach (var player in PlayerList)
        {
            foreach (var obj in player.playerObjs)
            {
                Destroy(obj);
            }
            player.playerCards.Clear();
            player.playerObjs.Clear();
        }
        foreach (var obj in Player.leftCardObjs)
        {
            Destroy(obj);
        }
    }
}