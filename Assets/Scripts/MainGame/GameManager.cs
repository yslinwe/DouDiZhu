using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    List<Player> players;
    public Transform showleftParent;
    private void Start() {
        
        Subject.Instance.Attach(Subject.EventName.GameOver,()=>{
            Env.Instance.StopGame = true;
        });
        // GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        // foreach (GameObject playerObj in playersObj)
        // {     
        //     Player player = playerObj.GetComponent<Player>();
        //     player.DealCards();
        // }
        Debug.Log(Player.GetPlayerCount());
        Deck.Instance.Shuffle();
        Player.DealCardsOnbyOne();
        Player.ShowLeftCards(showleftParent);
        // // 叫地主
        // players[0].Call();
        // // 抢地主
        // players[0].Grab();
        // foreach (var player in Player.GetPlayers())
        // {            
        //     player.DealCards();
        // }
        
        AudioManager.instance.PlayBGM("背景声");
        startCallIndex = Random.Range(0, 3);
        Player.SetIndex(startCallIndex);// 随机选择一个玩家进行叫地主
    }
    int startCallIndex;
    int startGrabIndex;
    
    private void LandlordGame()
    { 
        if(!Env.Instance.isCallOver)
        {
            Player player = Player.GetPlayer();
            player.CallGame();
            player.ChangeIndex();//转换下一位
            Env.Instance.isCallOver = startCallIndex == Player.GetIndex();
            if(Env.Instance.isCallOver)
            {
                //抢地主开始
                startGrabIndex = Player.GetCallIndex();   
                Env.Instance.isGrabOver = false;
                Subject.Instance.Attach(Subject.EventName.GrabOver,()=>{
                    Env.Instance.isGrabOver = true;
                    Player.ChangePlayersIndex();
                    Env.Instance.isGameOver = false;
                    Player player = Player.GetPlayer();
                    player.DealCards();
                    Subject.Instance.Notify(Subject.EventName.EnableButton);
                    Subject.Instance.Notify(Subject.EventName.SetShowInfo);
                });
            }
        }
        else
        {
            if(Env.Instance.isGrabOver)
            {
                Subject.Instance.Notify(Subject.EventName.GrabOver);
                return;
            }  
            int CallPlayerCount = Player.GetCallPlayerCount();
            switch(CallPlayerCount)
            {
                case 0:
                    Player.RestartGame();
                    Deck.Instance.CreateCards();
                    Deck.Instance.Shuffle();
                    Player.DealCardsOnbyOne();
                    Player.ShowLeftCards(showleftParent);
                    startCallIndex = Random.Range(0, 3);
                    Player.SetIndex(startCallIndex);// 随机选择一个玩家进行叫地主
                    break;
                case 1:
                    // 抢地主结束
                    Subject.Instance.Notify(Subject.EventName.GrabOver);
                    break;
                default:
                    Timer.instance.StartTimer(1,()=>{
                        //获取player 
                        Player player = Player.GetCallPlayer();
                        player.GrabGame();
                        Player.ChangeCallPlayerIndex();
                        Env.Instance.isGrabOver = startGrabIndex == Player.GetCallIndex();
                    });
                    break;
            }
        }
    }
    private void Game()
    {   
        Player player = Player.GetPlayer();
        player.Handle();
    }
    void Update()
    {
        if(Env.Instance.StopGame)
        {
            return;
        }
        if(!Timer.instance.IsRunning)
       {
            if (!Env.Instance.isGameOver)
                Game();
            else
            {
                LandlordGame();
            }
       }
    }
}