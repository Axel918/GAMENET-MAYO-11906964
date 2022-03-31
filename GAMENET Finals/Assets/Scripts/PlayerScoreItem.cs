using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreItem : MonoBehaviour
{
    
    public TextMeshProUGUI scoreText;
    public int score;
    public int currentScore;

    public float r;
    public float g;
    public float b;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        

        /*scoreText.text = currentScore.ToString("0");

        if (score > 0)
        {
            score = 0;
        }*/
    }
}
