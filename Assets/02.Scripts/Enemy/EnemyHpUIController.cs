using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUIController : MonoBehaviour
{
    [SerializeField] private EnemyStatus _enemyStatus;
    [SerializeField] private Slider _hpSlider;

    void OnEnable()
    {
        _enemyStatus.changeHpEvent += UpdateHpHUD;
        _hpSlider.maxValue = _enemyStatus.MaxHP;
        UpdateHpHUD(_enemyStatus.CurrentHP);
    }

    void OnDisable()
    {
        _enemyStatus.changeHpEvent -= UpdateHpHUD;
    }
    public void UpdateHpHUD(int _value)
    {
        _hpSlider.gameObject.SetActive(_value > 0);
        _hpSlider.value = _value;
    }
}
