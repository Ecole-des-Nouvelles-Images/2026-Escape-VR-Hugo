using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CodePadLockHandler : PadLock
{
    [SerializeField] private string RightCode;
    [SerializeField] private string CurrentCode;

    [SerializeField] private int NumberOne;
    [SerializeField] private int NumberTwo;
    
    [SerializeField] private GameObject CodePadLock;

    [Header("Visual small padLock")]
    
    [SerializeField] private GameObject _lockSmall;
    
    [Header("Visual big padLock")]

    [FormerlySerializedAs("GearOne")] [SerializeField] private GameObject _gearOne;
    [FormerlySerializedAs("GearTwo")] [SerializeField] private GameObject _gearTwo;
    [FormerlySerializedAs("Lock")] [SerializeField] private GameObject _lock;
    
    private bool _bigPadLockSpawned;
    private GameObject _player;
    private bool _canRotateGear = true;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (_bigPadLockSpawned)
        {
            if (Vector3.Distance(Camera.main.gameObject.transform.position, CodePadLock.transform.position) > 2.5f)
            {
                DespawnBigPadLock();
            }
        }
    }

    [ContextMenu("Interact")]
    public void Interact()
    {
        if (!IsLock) return;
        SpawnBigPadLock();
    }


    #region RegionModifyCode

    public void AddNumberOne()
    {
        if (!_canRotateGear) return;
        NumberOne++;
        if (NumberOne < 0) NumberOne = 9;
        if (NumberOne > 9) NumberOne = 0;
        SetCode();
        RotateGear(_gearOne, 36);
    }
    public void RemoveNumberOne()
    {
        if (!_canRotateGear) return;
        NumberOne--; 
        if (NumberOne < 0) NumberOne = 9;
        if (NumberOne > 9) NumberOne = 0;
        SetCode();
        RotateGear(_gearOne, -36);
    }
    
    public void AddNumberTwo()
    {
        if (!_canRotateGear) return;
        NumberTwo++;
        if (NumberTwo < 0) NumberTwo = 9;
        if (NumberTwo > 9) NumberTwo = 0;
        SetCode();
        RotateGear(_gearTwo, 36);
    }
    public void RemoveNumberTwo()
    {
        if (!_canRotateGear) return;
        NumberTwo--;
        if (NumberTwo < 0) NumberTwo = 9;
        if (NumberTwo > 9) NumberTwo = 0;
        SetCode();
        RotateGear(_gearTwo, -36);
    }
    
    #endregion

    private void SetCode()
    {
        CurrentCode = new string(NumberOne + "" + NumberTwo);
        VerifyIfCodeIsRight();
    }
    
    private void VerifyIfCodeIsRight()
    {
        if (RightCode == CurrentCode)
        {
            Debug.Log("Code is good");
            OpenLockPad();
        }
        else
        {
            Debug.Log("Code is bad");
        }
    }
    
    private void SpawnBigPadLock()
    {
        CodePadLock.SetActive(true);
        _bigPadLockSpawned = true;
    }

    private void DespawnBigPadLock()
    {
        CodePadLock.SetActive(false);
        _bigPadLockSpawned = false;
    }

    private void OpenLockPad()
    {
        IsLock = false;
        UnityEvent?.Invoke();
        AnimationUnlock();
    }

    private void RotateGear(GameObject gearTarget, float rotateValue)
    {
        Vector3 rotation =  gearTarget.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(rotation.x ,rotation.y + rotateValue, rotation.z);
        _canRotateGear = false;
        gearTarget.transform.DORotate(newRotation, 0.3f).OnComplete(() =>
        {
            _canRotateGear = true;
        });
    }

    [ContextMenu("AnimationUnlock")]
    private void AnimationUnlock()
    {
        Vector3 rotation =  _lock.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(rotation.x ,rotation.y, rotation.z + -30);
        _lock.transform.DORotate(newRotation ,1).OnComplete(() =>
        {
            CodePadLock.transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
            {
                CodePadLock.SetActive(false);
                AnimateReelPadLock();
            });
        });
    }

    private void AnimateReelPadLock()
    {
        Vector3 rotation =  _lockSmall.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(rotation.x ,rotation.y, rotation.z + -30);
        _lockSmall.transform.DORotate(newRotation, 1).OnComplete(() =>
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        });
    }
}
