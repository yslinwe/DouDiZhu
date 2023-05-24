using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
public class ShowInfo : UIManager{
    public static Sprite [] Sprites;
     private void Awake()
    {
        foreach (Transform child in transform)
        {
            GameObject childObject = child.gameObject;
            childObject.SetActive(false);        
            uiObjects.Add(childObject);
            GetChildObjects(childObject);
        }
        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     uiObjects.Add(transform.GetChild(i).gameObject);       
        //     for (int j = 0; j < transform.GetChild(i).childCount; j++)
        //     {        
        //         GameObject obj = transform.GetChild(i).GetChild(j).gameObject;
        //         obj.SetActive(false);        
        //         uiObjects.Add(obj);
        //     }
        // }
    }
    private void GetChildObjects(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject childObject = child.gameObject;
            childObject.SetActive(false);        
            uiObjects.Add(childObject);

            // 递归获取子孙物体
            GetChildObjects(childObject);
        }
    }
    private void Start() {
        Sprites = Load.instance.LoadSprite("people");
    }
    public void UpdateCardNum(string num)
    {
        GetUIObj("leftcardstext").GetComponent<TextMeshProUGUI>().text = num;
    }
    public void SetPlayerInfo(Player.CharcaterType CharcaterType, Player.Sex sex)
    {
        SetPlayerCharcater(CharcaterType);
        SetPlayerImage(CharcaterType,sex);
        OpenAllUIObjects();
    }
    public void SetPlayerCharcater(Player.CharcaterType CharcaterType)
    {
        TextMeshProUGUI text = GetUIObj("nickType").GetComponent<TextMeshProUGUI>();
        if(CharcaterType == Player.CharcaterType.landlord)
            text.text = "地主";
        else
            text.text = "农民";
    }
    public void SetPlayerImage(Player.CharcaterType CharcaterType, Player.Sex sex)
    {
        Image img =  GetUIObj("nickImage").GetComponent<Image>();
        if(CharcaterType == Player.CharcaterType.landlord)
        {
            //  地主女 2 地主男 3 农民女 6 农民男 7 
            if(sex == Player.Sex.F)
            {
                img.sprite = Sprites[2]; 
            }
            else
            {
                img.sprite = Sprites[3]; 
            }
        }
        else
        {   
            if(sex == Player.Sex.F)
            {
                img.sprite = Sprites[6]; 
            }
            else
            {
                img.sprite = Sprites[7]; 
            }
        }
    }
}