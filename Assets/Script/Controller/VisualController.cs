using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VisualController : MonoBehaviour
{
    private Light2D _light;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private Controller _controller;
    private Color _activeColor;
    private bool _active;
    public Ring ring;
    public ParticleSystem _particleDead;

    public void ActivePlayer(bool active)
    {
        _active = active;
        if (active)
        {
            _light.color = _activeColor;
            _light.intensity = 1.2f;
        }
        else
        {
            _light.color = Color.white;
            _light.intensity = 0.5f;
        }
    }

    public void Dead(Vector3 direction)
    {
        
        float rot_z = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg;
        _particleDead.transform.eulerAngles = new Vector3(rot_z,90,0);
        _particleDead.Play();
        Ring instance = Instantiate(ring, transform.position, Quaternion.identity);
        _animator.SetBool("dead", true);
        instance.SetColor(_renderer.color);
        if (_active)
        {
            CameraEffect.Shake(1.5f,0.5f);
            CameraEffect.Chromatic(0.8f, 0.8f);
        }
        else CameraEffect.Shake(0.5f,0.4f);
    }

    public void Relife()
    {
        _animator.SetBool("dead", false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<Controller>();
        _light = GetComponentInChildren<Light2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _activeColor = _light.color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Grounded(bool grounded)
    {
        _animator.SetBool("onGround",grounded);
    }

    public void Velocity(Vector2 velocity)
    {
        _animator.SetFloat("velocity",velocity.x);
        _animator.SetFloat("fall",velocity.y);
    }
}
