using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public enum State
    {
        move,jump,shoot,red,blue,green,end
    }
    public TextMeshPro move;
    public TextMeshPro jump;
    public TextMeshPro shoot;
    public TextMeshPro red;
    public TextMeshPro blue;
    public TextMeshPro green;
    private GameManager _manager;
    public State state;

    public bool change;
    public bool endState;
    // Start is called before the first frame update
    void Start()
    {
        state = State.move;
        _manager = FindObjectOfType<GameManager>();
        change = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.move: Move(); break;
            case State.jump: Jump(); break;
            case State.shoot: Shoot(); break;
            case State.red: Red(); break;
            case State.blue: Blue(); break;
            case State.green: Green(); break;
        }
    }

    public void Move()
    {
        if (change && _manager.stateTurn == GameManager.State.play)
        {
            move.GetComponent<Animator>().SetBool("active", true);
            change = false;
        }
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f && _manager.stateTurn == GameManager.State.play)
        {
            move.GetComponent<Animator>().SetBool("active", false);
            state = State.jump;
            change = true;
        }
    }

    public void Jump()
    {
        if (change && _manager.stateTurn == GameManager.State.play)
        {
            jump.GetComponent<Animator>().SetBool("active", true);
            change = false;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) && _manager.stateTurn == GameManager.State.play)
        {
            jump.GetComponent<Animator>().SetBool("active", false);
            state = State.shoot;
            change = true;
        }
    }

    public void Shoot()
    {
        if (change && _manager.stateTurn == GameManager.State.play)
        {
            shoot.GetComponent<Animator>().SetBool("active", true);
            change = false;
        }
        if (Input.GetMouseButtonDown(0) && _manager.stateTurn == GameManager.State.play)
        {
            shoot.GetComponent<Animator>().SetBool("active", false);
            state = State.red;
            change = true;
        }
    }

    public void Red()
    {
        if (change)
        {
            red.GetComponent<Animator>().SetBool("active", true);
            change = false;
        }
        if (_manager.state % _manager.players.Count == 1 && _manager.stateTurn == GameManager.State.play)
        {
            red.GetComponent<Animator>().SetBool("active", false);
            state = State.blue;
            change = true;
        }
    }

    public void Blue()
    {
        if (change)
        {
            blue.GetComponent<Animator>().SetBool("active", true);
            change = false;
        }
        if (!endState && _manager.state % _manager.players.Count != 1)
        {
            blue.GetComponent<Animator>().SetBool("active", false);
            endState = true;
        }

        if (_manager.state % _manager.players.Count == 2 && _manager.stateTurn == GameManager.State.play)
        {
            state = State.green;
            change = true;
        }
        
    }

    public void Green()
    {
        if (change)
        {
            green.GetComponent<Animator>().SetBool("active", true);
            change = false;
        }
        if (_manager.state % _manager.players.Count != 2)
        {
            green.GetComponent<Animator>().SetBool("active", false);
            state = State.end;
        }
    }
}
