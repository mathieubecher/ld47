using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        play,rewind,wait
    }

    public State stateTurn;
    
    public float gameDuration = 5;
    public int appearPlayer3 = 5;
    [HideInInspector] public float timer;
    [HideInInspector] public float wait;
    [HideInInspector] public float bug;
    public float glitch;
    private bool rewind = false;
    private float rewindSpeed;
    [SerializeField] private AnimationCurve rewindCurve;
    public SpriteRenderer timerImage;
    public Timer waiting;

    public int state = 0;
    public int valideState = 0;
    [Header("Player")]
    public Controller player1;
    public Controller player2;
    public Controller player3;
    public List<Bullet> bullets;
    public List<Controller> players;
    
    public LayerMask groundMask;
    public LayerMask wallMask;

    public Score score;
    
    void Start()
    {
        timer = 0;
        players= new List<Controller>();
        players.Add(player1);
        players.Add(player2);
        /*
        groundMask = LayerMask.GetMask("Ground");
        groundMask = LayerMask.GetMask("Wall");
        */
        wait = 5;
        waiting.color = new Color(0.7f, 0.36f, 0.36f);
        timerImage.color = waiting.color;
        
        for (int i = 0; i < players.Count; ++i)
        {
            players[i].ActivePlayerVisual(i == state % players.Count);
        }

    }
    
    void Update()
    {
        

        timerImage.transform.localScale = new Vector3(timer/gameDuration,1,1);

        if (bug > 0)
        {
            bug -= Time.deltaTime;
        }
        else if (glitch > 0)
        {
            glitch -= Time.deltaTime;
            if (glitch <= 7.1f)
            {
                waiting.LaunchTimeLine();
                player3.gameObject.SetActive(true);
            }
            if (glitch <= 0)
            {
                float lastWait = wait;
                Wait();
                wait = lastWait;
            }
            foreach (var player in players)
            {
                player.Reset();
            }

        }
        else if (rewind)
        {
            rewindSpeed = rewindCurve.Evaluate((5-timer)/5f);
            timer -= Time.deltaTime * rewindSpeed;
            if (timer <= 0)
            {
                rewind = !rewind;
                timer = 0;
                Wait();
                
            }
        }
        else if (wait > 0)
        {
            
            if (state == appearPlayer3 && players.Count < 3 && wait < 2)
            {
                glitch = 10;
                waiting.Glitch(wait);
                
                players.Add(player3);
                state = 2;
                
                //gameDuration += 2;
            }
            else
            {
                wait -= Time.deltaTime * 1.5f;
                if(wait<3) waiting.TimeInfo(wait);
                if(wait <= 0) Play(); 
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= gameDuration)
            {
                rewind = !rewind;
                Rewind();
                timer = gameDuration;
                bug = 1.5f;
            }
        }
    }

    void Wait()
    {
        stateTurn = State.wait;
        CameraEffect.Saturate(-36.6f,0.3f);
        CameraEffect.Grain(0.3f,0.2f);
        wait = 3.8f;
        if (valideState == players.Count - 1)
        {
            score.SetScore();
            ++state;
        }
        else if(!players[state%players.Count].dead) score.IncrementBuffer();
        
        switch (state % players.Count)
        {
            case 0 : waiting.color = new Color(0.7f,0.36f,0.36f); break;
            case 1 : waiting.color = new Color(0.36f,0.57f,0.7f); break;
            default : waiting.color = new Color(0.39f,0.7f,0.36f);  break;
            
        }

        timerImage.color = waiting.color;
        
        for (int i = 0; i < players.Count; ++i)
        {
            players[i].ActivePlayerVisual(i == state % players.Count);
        }
    }

    void Play()
    {
        stateTurn = State.play;
        foreach(Bullet b in bullets) if(b!= null) Destroy(b.gameObject);
        bullets = new List<Bullet>();

        for (int i = 0; i < players.Count; ++i)
        {
            if(i == state%players.Count) players[i].ToControl();
            else players[i].ToPlay();
        }
        valideState = 0;

    }

    void Rewind()
    {
        stateTurn = State.rewind;
        CameraEffect.Saturate(-90,1.5f);
        CameraEffect.Grain(0.9f,0.5f);
        for (int i = 0; i < players.Count; ++i)players[i].ToRewind();
        
        foreach (Bullet b in bullets)
        {
            b.Rewind();
        }
    }

    public void AddBullet(Bullet bullet)
    {
        bullets.Add(bullet);
    }
}
