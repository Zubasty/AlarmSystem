using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(House))]
public class Alarm : MonoBehaviour
{
    private House _house;
    private HashSet<Criminal> _criminalsInHouse;
    private int _countCriminalsForOnAlarm;
    private int _countCriminalsForOffAlarm;

    public event Action HouseVacated;
    public event Action HouseUnvacated;

    private void Awake()
    {
        _criminalsInHouse = new HashSet<Criminal>();
        _house = GetComponent<House>();
        _countCriminalsForOnAlarm = 1;
        _countCriminalsForOffAlarm = 0;
    }

    private void OnEnable()
    {
        _house.EnteredCriminal += OnEnteredCriminal;
        _house.LeavedCriminal += OnLeavedCriminal;
    }

    private void OnDisable()
    {
        _house.EnteredCriminal -= OnEnteredCriminal;
        _house.LeavedCriminal -= OnLeavedCriminal;
    }

    private void OnEnteredCriminal(Criminal criminal, Door door)
    {
        _criminalsInHouse.Add(criminal);
        if (_criminalsInHouse.Count == _countCriminalsForOnAlarm)
        {
            HouseUnvacated?.Invoke();
        }
    }

    private void OnLeavedCriminal(Criminal criminal)
    {
        _criminalsInHouse.Remove(criminal);
        if (_criminalsInHouse.Count == _countCriminalsForOffAlarm)
        {
            HouseVacated?.Invoke();
        }
    }
}
