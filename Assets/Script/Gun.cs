using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public List<float> rotates;
    public float cooldown = 0.2f;
    [HideInInspector] public Controller owner;
    public GameObject bullet;
    public GameObject gunEntity;

    private int startRewind;
    private bool dead;
    // Start is called before the first frame update
    void Awake()
    {
        rotates = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
    }

    public void Rotate(Vector3 mouse)
    {
        Vector2 diff = mouse - transform.position;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }
    
    public bool Shoot()
    {
        if (cooldown > 0) return false;
        cooldown = 0.2f;
        GameObject bObject = Instantiate(bullet, gunEntity.transform.position, transform.rotation);
        bObject.GetComponent<Bullet>().owner = owner;
        bObject.GetComponent<Bullet>().SetColor = owner.GetComponentInChildren<SpriteRenderer>().color;
        return true;
    }

    public void Rewind(int playNumber)
    {
        if(rotates.Count > playNumber && playNumber < startRewind) transform.eulerAngles = Vector3.forward * rotates[playNumber];
    }

    public void Record(int playNumber)
    {
        rotates.Add(transform.rotation.eulerAngles.z);
    }

    public void Play(int playNumber)
    {
        if(rotates.Count > playNumber) transform.eulerAngles = Vector3.forward * rotates[playNumber];
    }

    public void ResetRecord()
    {
        rotates = new List<float>();
    }

    public void StopPlay(int playNumber)
    {
        startRewind = playNumber;
    }
}
