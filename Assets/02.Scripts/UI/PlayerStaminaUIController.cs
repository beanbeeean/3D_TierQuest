using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUIController : MonoBehaviour
{
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private Slider _stSlider;
    [SerializeField] private TextMeshProUGUI _stValueTxt;
    [SerializeField] private TextMeshProUGUI _buffTimerTxt;
    [SerializeField] private GameObject _buffTxtInSlider;




    void OnEnable()
    {
        _playerStatus.changeStaminaEvent += UpdateStHUD;
        _playerStatus.changeBuffTimerEvent += UpdateBuffHUD;
    }

    void OnDisable()
    {
        _playerStatus.changeStaminaEvent -= UpdateStHUD;
        _playerStatus.changeBuffTimerEvent -= UpdateBuffHUD;
    }
    public void UpdateStHUD(float _value)
    {
        _stSlider.value = Mathf.RoundToInt(_value);
        _stValueTxt.text = Mathf.RoundToInt(_value).ToString();
    }

    public void UpdateBuffHUD(float _value)
    {
        if (_playerStatus.HasInfiniteStamina)
        {
            _buffTxtInSlider.SetActive(true);
            _buffTimerTxt.gameObject.SetActive(true);
            _buffTimerTxt.text = String.Format($"Infinite Stamina... \n{Mathf.RoundToInt(_value)}s");
        }
        else
        {
            _buffTimerTxt.gameObject.SetActive(false);
            _buffTxtInSlider.SetActive(false);
        }
        
    }
}
