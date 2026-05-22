using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUIController : MonoBehaviour
{
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpValueTxt;

    void OnEnable()
    {
        _playerStatus.changeHpEvent += UpdateHpHUD;
    }

    void OnDisable()
    {
        _playerStatus.changeHpEvent -= UpdateHpHUD;
    }
    public void UpdateHpHUD(int _value)
    {
        _hpSlider.value = _value;
        _hpValueTxt.text = _value.ToString();
    }
}
