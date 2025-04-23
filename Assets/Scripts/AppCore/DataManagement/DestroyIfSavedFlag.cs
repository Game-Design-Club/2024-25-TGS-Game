using System;
using UnityEngine;
using UnityEngine.Events;

namespace AppCore.DataManagement {
    public class DestroyIfSavedFlag : MonoBehaviour, DataID {
        [SerializeField] private string flagName;
        [SerializeField] private UnityEvent onDestroyAction;

        public void GenerateID() {
            flagName = name + Guid.NewGuid();
        }

        private void Reset() {
            GenerateID();
        }

        private void Awake() {
            if (flagName == "") {
                Debug.LogWarning("Flag name not set on " + name, gameObject);
                return;
            }
            if (App.Get<DataManager>().TryGetFlag(out bool flag, flagName)) {
                if (flag) Destroy(gameObject);
            } else {
                App.Get<DataManager>().SetFlag(flagName, false);
            }
        }

        public void SetFlag() {
            App.Get<DataManager>().SetFlag(flagName, true);
        }
    }
}