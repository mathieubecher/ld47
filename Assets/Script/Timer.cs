using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class Timer : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public Color color;
    private Animator _animator;
    //[SerializeField] private PlayableDirector glitchTimeLine;

    public List<Sprite> numbers;
    public List<Sprite> glitch;
    private float glitchTimer = 0;
    
    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private float glitchIteration = 2;
    void Update()
    {
        if (glitchTimer > 0)
        {
            glitchTimer -= Time.deltaTime*glitchIteration;
            glitchIteration *= 1.01f;
            if (glitchTimer < 0) glitchTimer = 0;
            _renderer.material.SetTexture("_Glitch", glitch[Mathf.FloorToInt(glitchTimer%glitch.Count)].texture);
            _renderer.material.SetFloat("_isGlitched", 0);
        } 
    }

    private int waitTime = 0;
    public void TimeInfo(float time)
    {
        if (Mathf.CeilToInt(time) != waitTime)
        {
            waitTime = Mathf.CeilToInt(time);
            
            LogInfo(waitTime);
            _animator.SetTrigger("Play");
        }
    }
    
    public void Glitch(float time)
    {
        waitTime = Mathf.CeilToInt(time);
        LogInfo(waitTime);
        _animator.SetTrigger("Glitch");
        //glitchTimeLine.Play();
        
    }
    public void LogInfo(int info)
    {
        _renderer.material.SetFloat("_isGlitched", 1);
        _renderer.enabled = true;
         _renderer.color = color;
         _renderer.sprite = numbers[info];
         //Invoke("Stop",time);
    }

    public void Stop()
    {
        _renderer.enabled = false;
    }

    public void Shake()
    {
        CameraEffect.Shake(2,0.1f);
        CameraEffect.Chromatic(0.8f, 0.3f);
    }

    public ParticleSystem particleBoum;
    public void Boum()
    {
        CameraEffect.Shake(2,1f);
        particleBoum.Play();
    }
    
    public void GlitchEvent()
    {
        glitchTimer = 80;
    }

    public PlayableDirector timeLine;
    public bool startLine;
    public void LaunchTimeLine()
    {
        if (startLine) return;
        timeLine.Play();
        startLine = true;
    }
}
