using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour {

    static Text _text;
    static float _clearTime;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    public static void Showtext(string txt, float seconds)
    {
        _text.text = txt;
        _clearTime = Time.time + seconds;
        _text.StartCoroutine(ConditionalClear(seconds));
    }

    private static IEnumerator ConditionalClear(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (Time.time > _clearTime) _text.text = "";
    }
}
