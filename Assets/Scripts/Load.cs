using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Load : MonoBehaviour {
    public static Load instance;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
   public GameObject LoadObj(string path)
   {
        GameObject myPrefab = Resources.Load<GameObject>(string.Format("Prefabs/{0}",path));
        GameObject newObj = Instantiate(myPrefab,Vector3.zero,Quaternion.identity);
        return newObj;
   }
   public Texture2D LoadTexture2D(string path)
   {
        Texture2D myTexture = Resources.Load<Texture2D>(string.Format("Textures/{0}",path));
        return myTexture;
   }
    public Material LoadMaterial(string path)
    {
        Material myMaterial = Resources.Load<Material>(string.Format("Materials/{0}",path));
        return myMaterial;
    }
    public  Sprite[] LoadSprite(string spritePath)
    {
        // 扑克花色分别为黑桃（Spade）、红桃（Heart）、方块（Diamond）、梅花（Club）
        // joker 13 27
        // 牌背 41
        // Diamond 0-12 2到A
        // Club 14-26 2到A
        // Heart 28-40 2到A
        // Spade 42-54 2到A
        Sprite[] spriteToLoad = new Sprite[56];
        spriteToLoad = Resources.LoadAll<Sprite>(string.Format("Sprites/{0}",spritePath));
        
        return spriteToLoad;
    }
    // public Sprite getSprite(int spriteIndex)
    // {
    //     return spriteToLoad[spriteIndex];
    // }
} 