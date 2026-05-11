using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PuzzleLight
{
    public class PuzzleLightHatchHandler : MonoBehaviour
    {
        [System.Serializable]
        public struct PuzzleLighSocketRequirement
        {
            public XRSocketInteractor Socket;
            public string requiredTag;
        }
        
        [Header("Sockets Configuration")]
        [SerializeField] private List<PuzzleLighSocketRequirement> _socketRequirements = new();

        [Header("DrawerToOpen")] 
        [SerializeField] private Transform _drawerTransform;
        [SerializeField] private Vector3 _drawerOffset = new Vector3(0, 0, 0.5f);
        [SerializeField] private float _openSpeed = 2f;

        private Vector3 _closedPosition;
        private Vector3 _targetPosition;
        private bool _isResolved = false;
    
        void Start()
        {
            _closedPosition = _drawerTransform.localPosition;
            _targetPosition = _closedPosition;

            foreach (var req in _socketRequirements)
            {
                req.Socket.selectEntered.AddListener(OnSocketChanged);
                req.Socket.selectExited.AddListener(OnSocketChanged);
            }
        }

        void Update()
        {
            if (_isResolved)
            {
                _drawerTransform.localPosition = Vector3.Lerp(_drawerTransform.localPosition, _targetPosition, Time.deltaTime * _openSpeed);
            }
        }

        private void OnSocketChanged(BaseInteractionEventArgs arg)
        {
            CheckPuzzleState();
        }

        private void CheckPuzzleState()
        {
            int correctCount = 0;
            foreach (var req in _socketRequirements)
            {
                if (req.Socket.hasSelection)
                {
                    IXRInteractable obj = req.Socket.interactablesSelected[0];
                    if (obj.transform.CompareTag(req.requiredTag))
                    {
                        correctCount++;
                    }
                }
            }

            if (correctCount == _socketRequirements.Count && !_isResolved)
            {
                ResolvePuzzle();
            }
        }

        private void ResolvePuzzle()
        {
            _isResolved = true;
            _targetPosition = _closedPosition + _drawerOffset;
        }

        private void OnDestroy()
        {
            foreach (var req in _socketRequirements)
            {
                req.Socket.selectExited.RemoveListener(OnSocketChanged);
                req.Socket.selectEntered.RemoveListener(OnSocketChanged);
            }
        }
    }
}
