using UnityEngine;
using DG.Tweening;

public class ShakeEffect : MonoBehaviour
{
    private float duration = 2f;

    public void Shake()
    {
        transform.DOShakeRotation(duration:2f,new Vector3(0f, 0f, 20f),vibrato:20);
    }
}
