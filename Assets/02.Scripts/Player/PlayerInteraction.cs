using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 5f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Transform _rayOrigin;

    private PlayerController _controller;
    private IInteractable _currentInteractable;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        DetectInteractable();
    }

    void OnEnable()
    {
        _controller.interactAction += TryInteract;
    }

    void OnDisable()
    {
        _controller.interactAction -= TryInteract;
    }

    private void DetectInteractable()
    {
        Ray ray = new Ray(_rayOrigin.position, _rayOrigin.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance, _interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (_currentInteractable != interactable)
                {
                    ClearCurrentInteractable();

                    _currentInteractable = interactable;
                    _currentInteractable.ActivatePopup();
                }

                Debug.DrawRay(_rayOrigin.position, _rayOrigin.forward * _interactDistance, Color.green);
                return;
            }
        }

        ClearCurrentInteractable();
        Debug.DrawRay(_rayOrigin.position, _rayOrigin.forward * _interactDistance, Color.red);
    }

    private void TryInteract()
    {
        if (_currentInteractable == null)
        {
            return;
        }

        _currentInteractable.Interact();
    }

    private void ClearCurrentInteractable()
    {
        if (_currentInteractable == null)
        {
            return;
        }

        _currentInteractable.InActivatePopup();
        _currentInteractable = null;
    }
}