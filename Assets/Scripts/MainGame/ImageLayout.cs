using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageLayout : MonoBehaviour
{
    public Transform centerPoint;
    public float spacing = 50f;
    public float animationDuration = 1f;
    public List<Transform> images = new List<Transform>();
    private List<Vector3> targetPositions = new List<Vector3>();

    public void OpenCards()
    {
        images.Clear();
        // 创建图片对象
        for (int i = 0; i < centerPoint.childCount; i++)
        {
            Transform imageTransform = centerPoint.GetChild(i);
            imageTransform.localPosition = Vector3.zero;
            images.Add(imageTransform);
        }
        
        // 计算目标位置
        CalculateTargetPositions();

        // 展开图片
        StartCoroutine(ExpandImages());
    }
  
    public void CloseCards()
    {
        // 收回图片
        StartCoroutine(CloseImages());
        OpenCards();
    }
    private void Update()
    {
        // 监听图片销毁事件
        for (int i = 0; i < images.Count; i++)
        {
            if (images[i] == null)
            {
                images.RemoveAt(i);
                targetPositions.RemoveAt(i);
                CalculateTargetPositions();
                StartCoroutine(MoveImagesToTargetPositions());
                break;
            }
        }
    }
   
    private void CalculateTargetPositions()
    {
        float totalWidth = (images.Count - 1) * spacing;
        Vector3 leftOffset = new Vector3(-totalWidth / 2f, 0f, 0f);

        targetPositions.Clear();

        for (int i = 0; i < images.Count; i++)
        {
            float offset = i * spacing;
            Vector3 targetPosition = leftOffset + new Vector3(offset, 0f, 0f);
            targetPositions.Add(targetPosition);
        }
    }
    private IEnumerator ExpandImages()
    {
        float startTime = Time.time;
        float endTime = startTime + animationDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / animationDuration;

            for (int i = 0; i < images.Count; i++)
            {
                if(images[i]!=null)
                {
                    images[i].localPosition = Vector3.Lerp(Vector3.zero, targetPositions[i], t);
                }
            }

            yield return null;
        }
    }
    private IEnumerator CloseImages()
    {
        float startTime = Time.time;
        float endTime = startTime + animationDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / animationDuration;

            for (int i = 0; i < images.Count; i++)
            {
                if(images[i]!=null)
                {
                    images[i].localPosition = Vector3.Lerp(images[i].localPosition,Vector3.zero, t);
                }
            }

            yield return null;
        }
    }

    private IEnumerator MoveImagesToTargetPositions()
    {
        float startTime = Time.time;
        float endTime = startTime + animationDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / animationDuration;

            for (int i = 0; i < images.Count; i++)
            {
                if(images[i]!=null)
                {
                    images[i].localPosition = Vector3.Lerp(images[i].localPosition, targetPositions[i], t);
                }
            }
            yield return null;
        }
    }
}
