using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtr : SingleTone<CameraCtr>
{
    public Transform target;
    public float speed;

    void Start()
    {

        //target = GameObject.Find("Circle").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        this.transform.position = new Vector3(target.position.x, target.position.y, -10);
    }

    void UpdatePos()
    {
        if(target != null)
        {
            Vector3 newForm = (target.transform.position - this.transform.position).normalized;
            newForm = new Vector3(newForm.x, newForm.y, 0);
            this.transform.Translate(newForm*speed);
        }
    }
}
