using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreItem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;
    public int currentScore;
    public GameObject[] tiles;

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
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<SpriteRenderer>().color == new Color(r, g, b))
            {
                score++;
            }
        }

        currentScore = score;

        scoreText.text = currentScore.ToString("0");

        if (score > 0)
        {
            score = 0;
        }
    }
}
