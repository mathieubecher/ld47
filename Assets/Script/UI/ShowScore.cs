using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshPro>().text = "Score : " + Score.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
