using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WheelOfLuck : MonoBehaviour
{
    [SerializeField] private SectionSettings[] _sections;
    [SerializeField] private int[] _firstWinsSectionsIndexes;
    [SerializeField] [Range(1, 20)] private float _spinDuration = 5f;
    [SerializeField] [Range(0, 50)] private int _amountOf360Rotations = 5;
    [SerializeField] private AnimationCurve _spinCurve;
    [SerializeField] private Section _sectionPrefab;
    [SerializeField] private UnityEvent NextSpinsCostEvent;
    [SerializeField] private Button _spinsButton;
    [SerializeField] private TextMeshProUGUI _spinsButtonText;
    [SerializeField] private string _spinText = "Spin";
    [SerializeField] private string _payText = "Pay";



    private int _sectionCount;
    private float _sectionAngle;
    private int _spinAmount = 0;
    private bool _isFirstSpin = true;

    void Start()
    {
        _sectionCount = _sections.Length;
        _sectionAngle = 360f / _sectionCount;
        for (int i = 0; i < _sectionCount; i++)
        {
            Section newSection = Instantiate(_sectionPrefab, transform);
            newSection.CreateSection(_sections[i].Sprite, _sectionAngle);
            newSection.RotateSection(_sectionAngle * i);
        }
        transform.eulerAngles = new Vector3(0, 0, 90 - (_sectionAngle / 2));
    }

    public void Spin()
    {
        if(_isFirstSpin)
        {
            StartCoroutine(SpinCoroutine());
            _spinsButtonText.text = _payText;
            _isFirstSpin = false;
            _spinsButton.interactable = false;
        }
        else
        {
            NextSpinsCostEvent?.Invoke();
            _spinsButtonText.text = _spinText;
            _isFirstSpin = true;
        }
    }

    private IEnumerator SpinCoroutine()
    {
        float elapsedTime = 0f;
        float startAngle = transform.eulerAngles.z;
        int desiredSection;
        bool isWinDetermined = false;

        if (_firstWinsSectionsIndexes != null)
        {
            if (_firstWinsSectionsIndexes.Length > 0)
            {
                if (_spinAmount < _firstWinsSectionsIndexes.Length)
                {
                    isWinDetermined = true;
                }
            }
        }

        if (isWinDetermined == false)
        {
            desiredSection = DetermineWinningSection();
        }
        else
        {
            desiredSection = _firstWinsSectionsIndexes[_spinAmount];
        }

        Debug.Log($"desiredSection = {desiredSection};");

        float endAngle =(90 - (_sectionAngle / 2)) + 360f * _amountOf360Rotations + _sectionAngle * (_sectionCount - desiredSection);

        while (elapsedTime < _spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _spinDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, _spinCurve.Evaluate(t));
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }

        float finalAngle = (endAngle - (90 - (_sectionAngle / 2))) % 360f;
        int selectedSection = _sectionCount - Mathf.FloorToInt(finalAngle / _sectionAngle);
        if (selectedSection == _sectionCount)
        {
            selectedSection = 0;
        }
        Debug.Log($"selectedSection = {selectedSection};");
        _sections[selectedSection].WinEvent?.Invoke();
        _spinAmount++;
        _spinsButton.interactable = true;
    }

    public int DetermineWinningSection()
    {
        float totalChance = 0f;
        foreach (var section in _sections)
        {
            totalChance += section.WinChance;
        }

        if (totalChance == 0)
        {
            Debug.LogError("Total chance is zero. Ensure at least one section has a positive chance.");
            return Random.Range(0, _sectionCount);
        }

        // Create cumulative probabilities
        float cumulativeChance = 0f;
        List<float> cumulativeProbabilities = new List<float>();
        foreach (var section in _sections)
        {
            cumulativeChance += section.WinChance / totalChance;  // Normalize each chance
            cumulativeProbabilities.Add(cumulativeChance);
        }

        float randomValue = Random.Range(0f, 1f);

        // Determine the winning section
        for (int i = 0; i < cumulativeProbabilities.Count; i++)
        {
            if (randomValue <= cumulativeProbabilities[i])
            {
                return i;
            }
        }

        // Fallback in case of rounding errors
        return Random.Range(0, _sectionCount);
    }
}

[System.Serializable]
public class SectionSettings
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] [Range(0f,1f)] private float _winChance;
    [SerializeField] private UnityEvent _winEvent;

    public Sprite Sprite
    {
        get { return _sprite; }
        private set { _sprite = value; }
    }

    public float WinChance
    {
        get { return _winChance; }
        private set { _winChance = value; }
    }

    public UnityEvent WinEvent
    {
        get { return _winEvent; }
        private set { _winEvent = value; }
    }
}
