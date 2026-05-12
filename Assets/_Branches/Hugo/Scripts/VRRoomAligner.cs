using Unity.XR.CoreUtils;
using UnityEngine;

namespace _Branches.Hugo.Scripts
{
    public class VRRoomAligner : MonoBehaviour
    {
        [SerializeField] private XROrigin _xrOrigin;
        [SerializeField] private Transform _targetSpawnPoint; // Un objet vide au centre de votre salle virtuelle

        void Start()
        {
            AlignPlayer();
        }

        [ContextMenu("Align Player")]
        public void AlignPlayer()
        {
            if (_xrOrigin == null || _targetSpawnPoint == null) return;

            // 1. On aligne la rotation
            // On calcule la différence d'angle sur l'axe Y entre le casque et la cible
            float rotationAngleY = _targetSpawnPoint.rotation.eulerAngles.y - _xrOrigin.Camera.transform.rotation.eulerAngles.y;
            _xrOrigin.transform.Rotate(0, rotationAngleY, 0);

            // 2. On aligne la position
            // On calcule le vecteur entre la position actuelle du casque et la cible
            Vector3 distanceDiff = _targetSpawnPoint.position - _xrOrigin.Camera.transform.position;
        
            // On déplace le XR Origin de cette distance (en ignorant la hauteur Y pour ne pas enfoncer le joueur dans le sol)
            _xrOrigin.transform.position += new Vector3(distanceDiff.x, 0, distanceDiff.z);
        
            Debug.Log("Zone VR recalée sur la salle virtuelle.");
        }
    }
}