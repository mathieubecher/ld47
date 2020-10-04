using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{

    private TextMeshProUGUI text;

    private List<float> times;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        times = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if(times.Count > 60) times.RemoveAt(0);
        times.Add(Time.deltaTime);
        int i = 0;
        float sum = 0;
        while (i < times.Count)
        {
            sum += times[i];
            ++i;
        }
        text.text = "Fps  : " + Mathf.Floor(1/(sum/(float)i));
    }
}
