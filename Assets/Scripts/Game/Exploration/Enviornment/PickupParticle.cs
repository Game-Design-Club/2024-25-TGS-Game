using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Exploration.Enviornment
{
    public class PickupParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem system;
        public void PlayAndDetach()
        {
            transform.parent = null;
            system.Play();
            StartCoroutine(SelfDestruct());
        }
        
        IEnumerator SelfDestruct()
        {

            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }
    }
}