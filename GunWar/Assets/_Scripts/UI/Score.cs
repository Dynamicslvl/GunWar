using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    [SerializeField] private bool isHighScore = false;
    [SerializeField] private string prefix = "";

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Utility.highScore < Utility.score)
        {
            Utility.haveNewBest = true;
            Utility.highScore = Utility.score;
        }
        int score = isHighScore ? Utility.highScore : Utility.score;
        scoreText.text = prefix + score;
    }
}
