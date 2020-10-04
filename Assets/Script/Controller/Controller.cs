using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    protected Rigidbody2D rb;
    private GameManager _manager;
    public Gun gun;
    public bool dead;

    public VisualController visual;
    
    void Awake()
    {
        visual = GetComponent<VisualController>();
        dead = true;
        informations = new List<RecordInformation>();
        rb = GetComponent<Rigidbody2D>();
        _manager = FindObjectOfType<GameManager>();
        gun.owner = this;
    }

    // Update is called once per frame
    void Update()
    {
        bottom = new Rect(new Vector2(transform.position.x-0.2f, transform.position.y-0.6f),new Vector2(0.3f, 0.2f));
        left = new Rect(new Vector2(transform.position.x-0.35f, transform.position.y-0.45f),new Vector2(0.2f, 0.9f));
        right = new Rect(new Vector2(transform.position.x + 0.2f, transform.position.y-0.45f), new Vector2(0.2f, 0.9f));
        CheckSituation();
        if (rewind || play || dead) return;
        
        Move();
        Target();
    }
    
    #region Move
    public float speed = 4;
    public float jumpForce = 10;
    
    private Rect bottom;
    public bool isGrounded;
    public bool jumpButton;
    
    private Rect left;
    protected bool isTouchingLeft;
    
    private Rect right;
    protected bool isTouchingRight;
    

    protected virtual void Move()
    {
        
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed,rb.velocity.y);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    private void CheckSituation()
    {
        isGrounded = Physics2D.OverlapBox(bottom.center, bottom.size, 0f, _manager.groundMask);
        isTouchingLeft = Physics2D.OverlapBox(left.center, left.size, 0f, _manager.wallMask);
        isTouchingRight = Physics2D.OverlapBox(right.center, right.size, 0f, _manager.wallMask);
    }


#if UNITY_EDITOR
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawCube(bottom.center,bottom.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(left.center,left.size);
        Gizmos.DrawCube(right.center,right.size);
    }

#endif
    
    #endregion
    
    #region Target
    private bool shoot = false;

    private void Target()
    {
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection.z = 0;
        gun.Rotate(mouseDirection);
        if (Input.GetButtonDown("Fire1"))
        {
            shoot = gun.Shoot();
            
        }
    }
    
    #endregion
    
    #region Record
    private List<RecordInformation> informations;
    private List<RecordInformation> rewindInformations;
    private int playNumber;
    private int rewindBegin;
    public bool rewind;
    public bool play;

    public void ToRewind()
    {
        visual.Relife();
        rewindBegin = informations.Count;
        rewind = true;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = Vector2.zero;
        if(!dead) gun.StopPlay(gun.rotates.Count);
    }
    public void ToPlay()
    {
        ActivePlayerVisual(false);
        dead = false;
        play = true;
        rewind = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rewindInformations = new List<RecordInformation>();
        
    }
    public void ToControl()
    {
        ActivePlayerVisual(true);
        dead = false;
        play = false;
        rewind = false;   
        informations = new List<RecordInformation>();
        gun.ResetRecord();
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
    }

    public void Reset()
    {
        if (!gameObject.activeSelf) return;
        
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        
    }
    
    void FixedUpdate()
    {
        
        if (rewind) Rewind();
        else if (play && !dead) Play();
        else Record();
    }

    void Rewind()
    {
        playNumber = Mathf.Max(0,Mathf.FloorToInt(_manager.timer/_manager.gameDuration * ((dead && play)?rewindInformations.Count-1: informations.Count-1)));
        
        if (informations.Count > playNumber)
        {
            transform.position = (dead && play)? rewindInformations[playNumber].position:informations[playNumber].position;
            transform.eulerAngles = Vector3.forward * ((dead && play)? rewindInformations[playNumber].rotation:informations[playNumber].rotation);
        }
        gun.Rewind(playNumber);
        
        visual.Grounded(true);
        visual.Velocity(Vector2.zero);
    }

    public float elapsed = 0;
    void Record()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1/20f)
        {
            if(dead && play) rewindInformations.Add(new RecordInformation(transform.position,transform.eulerAngles.z,shoot));
            else informations.Add(new RecordInformation(transform.position,transform.eulerAngles.z,shoot));
            shoot = false;
            if(!dead) gun.Record(playNumber);
            elapsed = 0;
        }
        
        visual.Grounded(isGrounded);
        visual.Velocity(rb.velocity);
    }

    void Play()
    {
        float percent = _manager.timer / _manager.gameDuration * informations.Count;
        int newNumber =  Mathf.FloorToInt(percent);
        bool alreadyShoot = newNumber == playNumber;
        playNumber = newNumber;
        
        if (informations.Count > playNumber)
        {
            transform.position = (informations.Count <= playNumber + 1)? informations[playNumber].position : Vector2.Lerp(informations[playNumber].position,informations[playNumber+1].position,percent-playNumber);
            transform.eulerAngles = Vector3.forward * ((informations.Count <= playNumber + 1)?informations[playNumber].rotation : informations[playNumber].rotation + (informations[playNumber + 1].rotation - informations[playNumber].rotation) * (percent-playNumber));

            if (informations[playNumber].shoot && !alreadyShoot)
            {
                gun.Shoot();
            }
            gun.Play(playNumber);
            
            
            visual.Grounded(isGrounded);
            if (informations.Count <= playNumber + 1) return;
            visual.Velocity((informations[playNumber+1].position - informations[playNumber].position) / informations.Count * _manager.gameDuration);
        }
        else Record();
    }
    #endregion

    public void Dead(Vector3 direction, Controller owner)
    {
        if ((play && !owner.play || !play) && !dead)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(direction * 50, ForceMode2D.Impulse);

            visual.Dead(direction);
            if (play)
            {
                ++_manager.valideState;
                gun.StopPlay(playNumber);
            }
            //while(informations.Count > playNumber) informations.RemoveAt(informations.Count-1);
            for(int i = 0; i < playNumber; ++i)
            {
                rewindInformations.Add(informations[i]);
            }
            dead = true;
        }
        else if (!play)
        {
            dead = true;
        }
        
    }


    public void ActivePlayerVisual(bool active)
    {
        visual.ActivePlayer(active);
    }
}


public class RecordInformation
{
    public Vector2 position;
    public float rotation;
    public bool shoot;

    public RecordInformation(Vector3 _position, float _rotation, bool _shoot)
    {
        rotation = _rotation;
        position = _position;
        shoot = _shoot;
    }
}