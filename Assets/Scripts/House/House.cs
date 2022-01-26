using System;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private Door[] _doors;
    public event Action<Criminal> LeavedCriminal;
    public event Action<Criminal, Door> EnteredCriminal;

    private void OnEnable()
    {
        foreach(Door door in _doors)
        {
            door.EnteredCriminal += EnterCriminal;
        }
    }

    private void OnDisable()
    {
        foreach (Door door in _doors)
        {
            door.EnteredCriminal -= EnterCriminal;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Criminal criminal))
        {
            LeavedCriminal?.Invoke(criminal);
        }
    }

    private void EnterCriminal(Criminal criminal, Door door)
    {
        EnteredCriminal?.Invoke(criminal, door);
    }
}
