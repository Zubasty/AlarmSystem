using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event Action<Criminal, Door> EnteredCriminal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Criminal criminal))
        {
            EnteredCriminal?.Invoke(criminal, this);
        }
    }
}
