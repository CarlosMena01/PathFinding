using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float delay;
    [SerializeField] AnimationCurve animationCurve;

    [SerializeField] RectTransform _target;

    [SerializeField] Vector2 startPoint, endPoint;

    public void FadeIn () {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(startPoint, endPoint));
        
    }
    public void FadeOut () {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(endPoint, startPoint));
        
    }

    IEnumerator FadeInCoroutine(Vector2 a, Vector2 b){
        Vector2 startPoint = a;
        Vector2 endPoint = b;
        float time = 0;
        while(time <= delay){
            time += Time.deltaTime;
            yield return null;
        }
        
        time = 0;
        while(time <= duration){
            float percent = time/duration;

            float curventPercent = animationCurve.Evaluate(percent);

            Vector2 currentPosition = Vector2.Lerp(startPoint, endPoint, curventPercent);

            _target.anchoredPosition = currentPosition;

            time += Time.deltaTime;
            yield return null;
        }
        _target.anchoredPosition = endPoint;
    }
}
