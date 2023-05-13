using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class JumpOnButtonDown : MonoBehaviour
{
    public float jumpHeight = 10.0f; // 跳跃高度
    public float jumpDuration = 0.5f; // 跳跃时间
    public float startHeight = 0f;
    private Button button;
    private RectTransform rectTransform;
    private bool frist;
    private int onclickNum = 0;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnMouseDown);
        frist = true;
        onclickNum = 0;
        button.enabled = false;
        Subject.Instance.Attach(Subject.EventName.GrabOver,()=>{
            button.enabled=true;
        });
    }
    private void OnMouseDown()
    {
        AudioManager.instance.PlaySound("select");
        if(frist)
        {
            startHeight = (rectTransform.position.y);
            frist = false;
        }
        onclickNum = onclickNum%2;
        if (onclickNum==0)
        {
            // 使用 DOMoveY 函数实现向上跳动的效果
            rectTransform.DOMoveY(startHeight + jumpHeight, jumpDuration).SetEase(Ease.OutQuad);
            HumanPlayer.playerChooseObj.Add(this.gameObject);
        }
        else
        {
            // 使用 DOMoveY 函数实现向下跳动的效果
            rectTransform.DOMoveY(startHeight, jumpDuration).SetEase(Ease.InQuad);
            HumanPlayer.playerChooseObj.Remove(this.gameObject);
        }
        onclickNum++;
    }
}
