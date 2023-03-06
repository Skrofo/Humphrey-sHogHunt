using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") //If player collides with game object (in this case the platform) it will "stick" to the platform
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") //If player leaves object it no longer gets dragged along with the platform
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
