using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private InputAction _restartAction;

    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private EnemyStatus _enemyStatus;
    [SerializeField] private CameraController _cameraController;

    private bool _isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _restartAction = _playerInput.actions.FindAction("Restart");

    }

    private void OnEnable()
    {
        _playerStatus.deathEvent += HandlePlayerDeath;
        if (_restartAction != null)
        {
            _restartAction.performed += OnRestartPerformed;
        }
    }

    private void OnDisable()
    {
        _playerStatus.deathEvent -= HandlePlayerDeath;
        if (_restartAction != null)
        {
            _restartAction.performed -= OnRestartPerformed;
        }
    }

    private void HandlePlayerDeath()
    {
        _isGameOver = true;

        _playerInput.SwitchCurrentActionMap("Game");
        _restartPanel.SetActive(true);
    }

    private void OnRestartPerformed(InputAction.CallbackContext ctx)
    {
        if (!_isGameOver) return;

        RestartGame();
    }

    private void RestartGame()
    {
        _isGameOver = false;

        if (_respawnPoint != null)
        {
            _player.transform.position = _respawnPoint.position;
            _player.transform.rotation = _respawnPoint.rotation;
        }

        _playerStatus.ResetStatus();
        _enemyStatus.ResetStatus();
        _cameraController.ResetCamera();
        _playerInput.SwitchCurrentActionMap("Player");
        _restartPanel.SetActive(false);
    }
}