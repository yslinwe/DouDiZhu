using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player : DoudiZhuRule
{
    public static Sprite [] Sprites;
    public static List<Card> lastPlayed;
    public static List<GameObject> ShowOneceCardObjs;
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
    public Transform playerparent;
    protected ShowTip showUI;
    public Sex sex;
    public int landlordIndex = -1; // 地主索引，0：地主上家，1：地主，2：地主下家
    public bool shouldCallLandlord = false;
    public bool shouldGrabLandlord = false;
    public bool isGrab = false;
    public Player lastPlayer;
    public enum Sex{
        M,
        F
    }
    public enum PlayCardType{
        min,
        max
    }
    public PlayCardType playCardType; 
    [HideInInspector]
    public int AllCandidatesNum;
    // public Player() {
    //     if(PlayerList==null)
    //     {
            
    //     }
    // }
    private void Awake() {
        playerCards = new List<Card>();
        PlayerList = new List<Player>();
        CallPlayerList = new List<Player>();
        lastPlayed = new List<Card>();
        ShowOneceCardObjs = new List<GameObject>();
        playerObjs = new List<GameObject>();
        playerparent = transform.GetChild(0);
        showUI = showTip.GetComponent<ShowTip>();
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
    public void DealCards()
    {
        int CardLengths = Deck.Instance.getCardsCount()>17? 17:Deck.Instance.getCardsCount();
        for (int i = 0; i < CardLengths; i++)
        {
            Card card = Deck.Instance.Deal();
            playerCards.Add(card);
            string objName = type == playerType.player ? "playerCard" : "othersCard";
            GameObject obj = Load.instance.LoadObj(objName);
            Sprites = Load.instance.LoadSprite("card-deck");
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
            obj.transform.parent = playerparent;
            obj.transform.localRotation = Quaternion.identity;
            playerObjs.Add(obj);
        }
        //排序
        playerCards.Sort((a, b) => a.GetCardRank() - b.GetCardRank());
        for (int i = 0; i < CardLengths; i++)
        {
            Card card = playerCards[i];
            GameObject obj = playerObjs[i];
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
        }
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
     public void ShowOneceCards(List<Card> playerCards)
    {
        foreach (var obj in ShowOneceCardObjs)
        {
            Destroy(obj);
        }
        ShowOneceCardObjs.Clear();
        for (int i = 0; i < playerCards.Count; i++)
        {
            Card card = playerCards[i];
            GameObject obj = Load.instance.LoadObj("othersCard");
            int index = card.indexOfSprite();
            Sprite sp = Sprites[index];
            obj.GetComponent<Image>().sprite = sp;
            obj.transform.parent = leftOverparent;
            obj.transform.localRotation = Quaternion.identity;
            ShowOneceCardObjs.Add(obj);
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
}