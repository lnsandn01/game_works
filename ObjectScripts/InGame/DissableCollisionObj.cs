using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollisionObj : MonoBehaviour
{
    [SerializeField]private List<string> dont_destroy;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!dont_destroy.Contains(collision.gameObject.name))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
