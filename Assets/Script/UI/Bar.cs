using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public Vector3 origin;

    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private float timer;

    private bool shake;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 1)
            {
                Color c = _renderer.color;
                c.a = timer;
                _renderer.color = c;
            }
            if(timer <= 0) gameObject.SetActive(false);
        }
        else if (shake)
        {
            transform.position = origin + Quaternion.Euler(0,0,Random.value *360) * Vector3.up * 0.1f;
        }
    }

    public void Activate(bool b)
    {
        if (b)
        {
            shake = false;
            gameObject.SetActive(true);
            _rb.isKinematic = true;
            transform.position = origin;
            transform.rotation = Quaternion.identity;
            _renderer.color = Color.white;
            timer = 0;
        }
        else
        {
            shake = false;
            _rb.isKinematic = false;
            Vector2 force = Quaternion.Euler(0,0,Random.value *360) * Vector3.up;
            Debug.Log(force);
            _rb.AddForce(force * 10,ForceMode2D.Impulse);
            timer = 4;
        }
    }

    public void Shake()
    {
        shake = true;
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }
}
