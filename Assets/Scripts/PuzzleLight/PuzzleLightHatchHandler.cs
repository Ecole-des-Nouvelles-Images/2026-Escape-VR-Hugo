using System;
using System.Collections.Generic;
using Core.Audio;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace PuzzleLight
{
    public class PuzzleLightHatchHandler : MonoBehaviour
    {
        [Serializable]
        public struct PuzzleLightSocketRequirement
        {
            public XRSocketInteractor Socket;
            public GameObject RequiredGo;
        }
        
        public bool IsResolved { get; private set; }
        
        [Header("===== SOCKETS =====")]
        [SerializeField] private List<PuzzleLightSocketRequirement> _socketRequirements = new();

        [Header("===== SETTINGS DRAWER =====")] 
        [SerializeField] private Transform _drawerTransform;
        [SerializeField] private Vector3 _drawerOffset = new(0, 0, 0.5f);
        
        [Header("===== ANIMATION =====")]
        [SerializeField] private float _openDuration = 2f;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("===== SFX =====")]
        [SerializeField] private EventReference _drawerOpenSFX;
        [SerializeField] private EventReference _statuePlaceSFX;
        [SerializeField] private EventReference _statueRemoveSFX;

        private Vector3 _closedPos;

        [ContextMenu("ResolvePuzzle")]
        public void IsResolve()
        {
            IsResolved = true;
        }
    
        void Start()
        {
            _closedPos = _drawerTransform.localPosition;
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            foreach (var req in _socketRequirements)
            {
                req.Socket.selectEntered.AddListener(OnSocketChanged);
                req.Socket.selectExited.AddListener(OnSocketChanged);
            }
        }

        private void OnDisable()
        {
            foreach (var req in _socketRequirements)
            {
                req.Socket.selectExited.RemoveListener(OnSocketChanged);
                req.Socket.selectEntered.RemoveListener(OnSocketChanged);
            }
        }
        
        private void OnSocketChanged(BaseInteractionEventArgs arg)
        {
            if (AudioManager.Instance)
            {
                Vector3 socketPos = arg.interactorObject.transform.position;
                if (arg is SelectEnterEventArgs)
                {
                    if (!_statuePlaceSFX.IsNull)
                        AudioManager.Instance.PlayAtPosition(_statuePlaceSFX, socketPos);
                }
                else if (arg is SelectExitEventArgs)
                {
                    if (!_statueRemoveSFX.IsNull)
                        AudioManager.Instance.PlayAtPosition(_statueRemoveSFX, socketPos);
                }
            }
            CheckPuzzleState();
        }

        #endregion

        private void CheckPuzzleState()
        {
            int correctCount = 0;
            foreach (var req in _socketRequirements)
            {
                if (req.Socket.hasSelection)
                {
                    IXRInteractable obj = req.Socket.interactablesSelected[0];
                    if (obj.transform.gameObject == req.RequiredGo)
                    {
                        correctCount++;
                    }
                }
            }

            if (correctCount == _socketRequirements.Count && !IsResolved)
            {
                ResolvePuzzle();
            }
        }

        private void ResolvePuzzle()
        {
            IsResolved = true;
            _drawerTransform.DOLocalMove(_closedPos + _drawerOffset, _openDuration)
                .SetEase(_animationCurve);

            Invoke(nameof(DisableInteraction), 0.1f);
            
            // SFX
            AudioManager.Instance.PlayAtPosition(_drawerOpenSFX, transform.position);
        }

        private void DisableInteraction()
        {
            foreach (var req in _socketRequirements)
            {
                // req.Socket.enabled = false;
                req.RequiredGo.GetComponent<XRGrabInteractable>().enabled = false;
            }
        }
    }
}
