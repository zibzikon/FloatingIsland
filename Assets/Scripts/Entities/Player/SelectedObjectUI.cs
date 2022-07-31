    using System;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.UI;

    public class SelectedObjectUI : MonoBehaviour, ISwitchable
    {
        [SerializeField] private Button _setDestroyingTargetButton;
        [SerializeField] private Button _interactButton;

        private ISelectable _selectedObject;

        private Player _player;

        public void Initialize(Player player)
        {
            _player = player;
        }

        private void Awake()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _setDestroyingTargetButton.onClick.AddListener(OnSetDestroyingTargetButtonClicked);
            _interactButton.onClick.AddListener(OnInteractButtonClicked);
        }
        
        private void OnSetDestroyingTargetButtonClicked()
        {
            _player.StartAttackingTarget();
            Disable();
        }

        private void OnInteractButtonClicked()
        {
            _player.TryInteractWithSelectedInteractable();
            Disable();
        }

        public void SetSelectedObject(ISelectable selectable)
        {
            _selectedObject = selectable;
            Enable();
        }

        public void RemoveSelectedObject()
        {
            _selectedObject = null;
            Disable();
        }

        public void Enable()
        {
            GeneralGameSettings.RayCastIsBlocked = true;
            if (_selectedObject is IInteractable)
            {
                _interactButton.gameObject.SetActive(true);
            }
            if (_selectedObject is ITarget)
            {
                _setDestroyingTargetButton.gameObject.SetActive(true);
            }
        }

        public void Disable()
        {
            GeneralGameSettings.RayCastIsBlocked = false;
            _setDestroyingTargetButton.gameObject.SetActive(false);
            _interactButton.gameObject.SetActive(false);
        }
    }
