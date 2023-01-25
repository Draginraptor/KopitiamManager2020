using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText = null;
    int _seconds = 0;
    private WaitForSeconds _timerTick = new WaitForSeconds(1f);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimerTick());
    }

    // Increment the num of seconds every second
    IEnumerator TimerTick()
    {
        while(true)
        {
            yield return _timerTick;
            _seconds++;
            _timerText.text = "Time: " + _seconds;
        }
    }
}
