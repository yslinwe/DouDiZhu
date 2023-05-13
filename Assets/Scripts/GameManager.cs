using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    List<Player> players;
    private void Start() {
        
        Deck.Instance.Shuffle();
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in playersObj)
        {     
            Player player = playerObj.GetComponent<Player>();
            player.DealCards();
        }
      
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
                    // RestartGame()
                    break;
                case 1:
                    // 抢地主结束
                    Subject.Instance.Notify(Subject.EventName.GrabOver);
                    break;
                default:
                    //获取player 
                    Player player = Player.GetCallPlayer();
                    player.GrabGame();
                    Player.ChangeCallPlayerIndex();
                    Env.Instance.isGrabOver = startGrabIndex == Player.GetCallIndex();
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