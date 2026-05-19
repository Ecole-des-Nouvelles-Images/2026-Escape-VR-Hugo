using DG.Tweening;
using UnityEngine;

namespace MonoBehiavors
{
    public class MainClockPuzzle : MonoBehaviour
    {
        [SerializeField] private int _numberRightKey;

        [SerializeField] private GameObject _drawer;
    
        public void AddKey()
        {
            _numberRightKey++;
            if (_numberRightKey == 3) OpenDrawer();
        
        }

        [ContextMenu("OpenDrawer")]
        private void OpenDrawer()
        {
            _drawer.transform.DOMoveX(transform.position.x + 0.45f, 1);
        }
    }
}
