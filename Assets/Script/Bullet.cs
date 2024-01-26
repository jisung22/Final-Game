using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
        else if (!isMelee && collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}