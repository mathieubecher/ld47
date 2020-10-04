using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBullet : Bullet
{
    public int maxHit = 2;
    private int hit = 0;
    protected override void HitGround(Vector3 normal)
    {
        ++hit;
        if (hit == maxHit)
        {
            End();
            return;
        }

        Vector3 direction = transform.rotation * Vector3.right;
        Vector3 newDirection = Vector3.Reflect(direction, normal);
        
        float rot_z = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }
}
