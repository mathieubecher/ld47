using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private float timer;
    public float speed = 2;
    public float size = 4;
    private SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.material.SetFloat("_size",size);
    }

    public void SetColor(Color c)
    {
        GetComponentInChildren<SpriteRenderer>().color = c;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;
        _renderer.material.SetFloat("_time",timer);
        if(timer > 1) Destroy(gameObject);
    }
}
