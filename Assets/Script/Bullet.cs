using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Vector3 = UnityEngine.Vector3;

public class Bullet : MonoBehaviour
{
    public Controller owner;
    public float speed;
    private bool rewind;
    public List<Vector2> positions;
    private float timerEnd;
    private float timerBegin;
    private SpriteRenderer _renderer;
    private bool active = true;
    private GameManager _manager;
    

    public Color SetColor
    {
        set
        {
            GetComponent<SpriteRenderer>().color = value;
            TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(value, 0.0f), new GradientColorKey(value, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 0.8f) }
            );
            trail.colorGradient = gradient;
            GetComponentInChildren<Light2D>().color = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        positions = new List<Vector2>();
        _manager = FindObjectOfType<GameManager>();
        _renderer = GetComponent<SpriteRenderer>();
        _manager.AddBullet(this);
        timerBegin = _manager.timer;
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if (rewind) return;
        
        if(active) transform.position += transform.rotation * Vector3.right * Time.deltaTime * speed;
    }

    private float elapsed = 0;
    void FixedUpdate()
    {
        if (rewind)
        {
            //TODO pas fini
            if (_manager.timer > timerEnd) return;
            
            int i = Mathf.FloorToInt((_manager.timer - timerBegin)/(timerEnd-timerBegin) * positions.Count - 1);
            if (i < 0 || i >= positions.Count)
            {
                Destroy(this.gameObject);
                return;
            }

            if (!active)
            {
                _renderer.enabled = true;
                transform.GetComponentInChildren<TrailRenderer>().enabled = true;
                active = true;
            }
            
            transform.position = positions[i];
        }
        else if(active)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 1 / 10f)
            {
                positions.Add(transform.position);
                elapsed = 0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.gameObject == gameObject || owner == null || owner.gameObject == other.gameObject) return;
        
        Vector3 near = (other.ClosestPoint(transform.position));
        Vector3 normal =  (near - transform.position).normalized;
        
        if (other.TryGetComponent(out Bullet bullet))
        {
            End();
            bullet.End();
        }
        if (other.transform.TryGetComponent(out Controller target))
        {
            if (owner.play && target.play) return;
            HitPlayer(target);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        HitGround(other.contacts[0].normal);
    }

    protected virtual void HitPlayer(Controller target)
    {
        target.Dead(transform.rotation * new Vector3(1, 0), owner);
        End(true);
    }

    protected virtual void HitGround(Vector3 normal)
    {
        if(!rewind) End();
        
    }
    protected void End(bool kill = false)
    {
        if(!kill && !owner.play) CameraEffect.Shake(0.5f,0.1f);
        timerEnd = _manager.timer;
        active = false;
        _renderer.enabled = false;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponentInChildren<Collider2D>().enabled = false;
        transform.GetComponentInChildren<TrailRenderer>().enabled = false;
        
    }

    public void Rewind()
    {
        if (active) timerEnd = _manager.timer;
        rewind = true;
        gameObject.SetActive(true);
    }
}
