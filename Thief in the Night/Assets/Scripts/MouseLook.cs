using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector3 worldMousePosition;
    Vector2 shootDirection;
    public Transform shootPointTF;
    public GameObject handcuffs;
    public float handcuffSpeed;
    void Update()
    {
        GetMousePosition();
        //GetKeyDown for one shot per click, not continuous
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootHandcuffs();
        }
    }
    void GetMousePosition()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shootDirection = (Vector2)(worldMousePosition - transform.position);
        shootDirection.Normalize();
    }
    void ShootHandcuffs()
    {
        var handcuffObj = Instantiate(handcuffs, shootPointTF.position, Quaternion.identity);
        var handcuffRB = handcuffObj.GetComponent<Rigidbody2D>();
        handcuffRB.velocity = shootDirection * handcuffSpeed;
    }
}
