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
    [SerializeField] private int NumberThree;
    
    [SerializeField] private GameObject CodePadLock;

    [Header("Visual small padLock")]
    
    [SerializeField] private GameObject _lockSmall;
    
    [Header("Visual big padLock")]

    [FormerlySerializedAs("GearOne")] [SerializeField] private GameObject _gearOne;
    [FormerlySerializedAs("GearTwo")] [SerializeField] private GameObject _gearTwo;
    [FormerlySerializedAs("GearThree")] [SerializeField] private GameObject _gearThree;
    [FormerlySerializedAs("Lock")] [SerializeField] private GameObject _lock;
    
    private bool _bigPadLockSpawned;
    private GameObject _player;
    private List<Button> _allButtons;

    private void Awake()
    {
        _allButtons = GetComponentsInChildren<Button>(true).ToList();
    }

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
        NumberOne++;
        if (NumberOne < 0) NumberOne = 9;
        if (NumberOne > 9) NumberOne = 0;
        SetCode();
        RotateGear(_gearOne, 36);
    }
    public void RemoveNumberOne()
    {
        NumberOne--; 
        if (NumberOne < 0) NumberOne = 9;
        if (NumberOne > 9) NumberOne = 0;
        SetCode();
        RotateGear(_gearOne, -36);
    }
    
    public void AddNumberTwo()
    {
        NumberTwo++;
        if (NumberTwo < 0) NumberTwo = 9;
        if (NumberTwo > 9) NumberTwo = 0;
        SetCode();
        RotateGear(_gearTwo, 36);
    }
    public void RemoveNumberTwo()
    {
        NumberTwo--;
        if (NumberTwo < 0) NumberTwo = 9;
        if (NumberTwo > 9) NumberTwo = 0;
        SetCode();
        RotateGear(_gearTwo, -36);
    }

    public void AddNumberThree()
    {
        NumberThree++;
        if (NumberThree < 0) NumberThree = 9;
        if (NumberThree > 9) NumberThree = 0;
        SetCode();
        RotateGear(_gearThree, 36);
    }
    public void RemoveNumberThree()
    {
        NumberThree--;
        if (NumberThree < 0) NumberThree = 9;
        if (NumberThree > 9) NumberThree = 0;
        SetCode();
        RotateGear(_gearThree, -36);
    }

    #endregion

    private void SetCode()
    {
        CurrentCode = new string(NumberOne + "" + NumberTwo + "" + NumberThree);
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
        foreach (Button button in _allButtons) button.interactable = false;
        gearTarget.transform.DORotate(newRotation, 0.3f).OnComplete(() =>
        {
            foreach (Button button in _allButtons) button.interactable = true;
        });
    }

    [ContextMenu("AnimationUnlock")]
    private void AnimationUnlock()
    {
        Vector3 rotation =  _lock.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(rotation.x ,rotation.y, rotation.z + 90);
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
        Vector3 newRotation = new Vector3(rotation.x ,rotation.y, rotation.z + 90);
        _lockSmall.transform.DORotate(newRotation, 1).OnComplete(() =>
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        });
    }
}
