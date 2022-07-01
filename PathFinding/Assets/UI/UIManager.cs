using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float delay;
    [SerializeField] AnimationCurve animation;

    [SerializeField] RectTransform _target;

    [SerializeField] Vector2 startPoint, endPoint;

    public void FadeIn () {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
        
    }

    IEnumerator FadeInCoroutine(){
        float time = 0;
        while(time <= delay){
            time += Time.deltaTime;
            yield return null;
        }
        
        time = 0;
        while(time <= duration){
            float percent = time/duration;

            float curventPercent = animation.Evaluate(percent);

            Vector2 currentPosition = Vector2.Lerp(startPoint, endPoint, curventPercent);

            _target.anchoredPosition = currentPosition;

            time += Time.deltaTime;
            yield return null;
        }
        _target.anchoredPosition = endPoint;
    }

    IEnumerator FadeOutCoroutine(){
        yield return null;
    }
}
