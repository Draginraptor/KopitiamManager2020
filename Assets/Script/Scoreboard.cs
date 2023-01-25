using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard Instance = null;

    [SerializeField] private TextMeshProUGUI _scoreText = null;

    private int _score = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Call to initialise the text
        ChangeScore(0);
    }

    public void ChangeScore(int scoreDelta)
    {
        _score += scoreDelta;
        _scoreText.text = "Score: " + _score;
    }
}
