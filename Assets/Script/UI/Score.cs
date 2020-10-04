using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public int buffer = 0;
    public int bufferI = 0;
    public static int score;

    public TextMeshPro scoreText;

    public List<Bar> lifebar;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        buffer = 500;
        bufferI = lifebar.Count - 1;
        scoreText.text = "Score : " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore()
    {
        score += buffer;
        scoreText.text = "Score : " + score;
        bufferI = lifebar.Count-1;
        foreach(var bar in lifebar) bar.Activate(true);
        buffer = 500;
    }

    public void IncrementBuffer()
    {
        buffer -= 100;
        
        CameraEffect.Shake(2,0.5f);
        lifebar[bufferI].Activate(false);
        --bufferI;
        if (bufferI == 1)
        {
            for (int i = 0; i <= bufferI; ++i)
            {
                lifebar[i].SetColor(Color.red);
            }
        }
        if(bufferI == 0) lifebar[0].Shake();

        if (buffer < 0) buffer = 0;
        if (buffer == 0) GameOver();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}
