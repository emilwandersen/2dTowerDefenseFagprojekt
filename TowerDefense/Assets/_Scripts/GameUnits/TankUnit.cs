using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankUnit : Unit
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetHealth(400);
        SetSpeed(_moveSpeed * 0.4f);
        transform.localScale = new Vector3(.9f, .9f, 0.5f);
    }
}
