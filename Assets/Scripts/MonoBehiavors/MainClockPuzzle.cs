using System.Collections;
using Core.Audio;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

namespace MonoBehiavors
{
    public class MainClockPuzzle : MonoBehaviour
    {
        [SerializeField] private int _numberRightKey;

        [SerializeField] private GameObject _drawer;
    
        [SerializeField] private GameObject _gear;

        [Header("===== FMOD AUDIO =====")] 
        [SerializeField] private EventReference _gearTurnSFX;
        [SerializeField] private EventReference _drawerOpenSFX;
        public void AddKey()
        {
            _numberRightKey++;
            if (_numberRightKey == 3) StartCoroutine(OpenDrawer());
        }

        [ContextMenu("OpenDrawer")]
        private IEnumerator OpenDrawer()
        {
            Vector3 rotation = _gear.transform.rotation.eulerAngles;
            rotation.x += 180;
            
            if (AudioManager.Instance && !_gearTurnSFX.IsNull)
            {
                AudioManager.Instance.PlayAtPosition(_gearTurnSFX, _gear.transform.position);
            }
            
            _gear.transform.DORotate(rotation, 1f);
            yield return new WaitForSeconds(2.5f);

            if (AudioManager.Instance && !_drawerOpenSFX.IsNull)
            {
                AudioManager.Instance.PlayAtPosition(_drawerOpenSFX, _drawer.transform.position);
            }
            
            _drawer.transform.DOMoveX(transform.position.x - 0.45f, 1);
        }
    }
}

