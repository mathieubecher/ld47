using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _animator.SetBool("active",!_animator.GetBool("active"));
        }
    }

    public void OnExit()
    {
        SceneManager.LoadScene(0);
    }
}
