using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmAudio : MonoBehaviour
{
    [SerializeField] private Alarm _alarm;
    [SerializeField, Range(0, 1)] private float _minVolume;
    [SerializeField, Range(0, 1)] private float _maxVolume;
    [SerializeField] private float _speedSettingVolume;
    private AudioSource _audio;
    private Coroutine _nowCoroutine;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _alarm.HouseVacated += StartOffAudio;
        _alarm.HouseUnvacated += StartOnAudio;
    }

    private void OnDisable()
    {
        _alarm.HouseVacated -= StartOffAudio;
        _alarm.HouseUnvacated -= StartOnAudio;
    }

    private void OnValidate()
    {
        if(_minVolume < 0 || _minVolume > 1)
        {
            _minVolume = 0;
            Debug.LogError("Минимальная громкость должна быть в диапазоне от 0 до 1");
        }
        if (_maxVolume < 0 || _maxVolume > 1)
        {
            _maxVolume = 1;
            Debug.LogError("Максимальная громкость должна быть в диапазоне от 0 до 1");
        }
        if (_minVolume > _maxVolume)
        {
            _minVolume = 0;
            _maxVolume = 1;
            Debug.LogError("Минимальная громкость не должна превышать максимальную");
        }
        if (_speedSettingVolume <= 0)
        {
            _speedSettingVolume = 1;
            Debug.LogError("Скорость изменения громкости должна быть положительной");
        }
    }
    
    private void StartOnAudio()
    {
        if(_nowCoroutine != null)
        {
            StopCoroutine(_nowCoroutine);
        }
        if (_audio.isPlaying == false)
        {
            _audio.volume = _minVolume;
            _audio.Play();
        }
        _nowCoroutine = StartCoroutine(GettingLouder());
    }

    private void StartOffAudio()
    {
        if (_nowCoroutine != null)
        {
            StopCoroutine(_nowCoroutine);
        }
        _nowCoroutine = StartCoroutine(GettingQuieter());
    }

    private IEnumerator GettingLouder()
    {
        while(_audio.volume < _maxVolume)
        {
            _audio.volume = Mathf.MoveTowards(_audio.volume, _maxVolume, _speedSettingVolume * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator GettingQuieter()
    {
        while (_audio.volume > _minVolume)
        {
            _audio.volume = Mathf.MoveTowards(_audio.volume, _minVolume, _speedSettingVolume * Time.deltaTime);
            yield return null;
        }
        _audio.Stop();
        yield return null;
    }
}
