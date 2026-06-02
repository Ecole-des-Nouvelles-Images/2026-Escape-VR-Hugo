using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace MonoBehiavors
{
    public class MainClockPuzzle : MonoBehaviour
    {
        [SerializeField] private int _numberRightKey;

        [SerializeField] private GameObject _drawer;
    
        [SerializeField] private GameObject _gear;
        
        public void AddKey()
        {
            _numberRightKey++;
            if (_numberRightKey == 3) OpenDrawer();
            if (_numberRightKey == 3) StartCoroutine(OpenDrawer());
        }

        [ContextMenu("OpenDrawer")]
        private IEnumerator OpenDrawer()
        {
            Vector3 rotation = _gear.transform.rotation.eulerAngles;
            rotation.x += 180;
            _gear.transform.DORotate(rotation, 1f);
            yield return new WaitForSeconds(2.5f);
            _drawer.transform.DOMoveX(transform.position.x - 0.45f, 1);
        }
    }
}

