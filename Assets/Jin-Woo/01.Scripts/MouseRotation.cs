using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float angle;
    public Vector2 mouse;
    Player player;
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - player.transform.position.y, mouse.x - player.transform.position.x)*Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }


}


