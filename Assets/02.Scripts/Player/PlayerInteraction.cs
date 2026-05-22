using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 5f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Transform _rayStartPoint;

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
        Ray ray = new Ray(_rayStartPoint.position, _rayStartPoint.forward);

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

                return;
            }
        }

        ClearCurrentInteractable();
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