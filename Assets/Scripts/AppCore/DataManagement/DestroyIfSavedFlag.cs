using System;
using UnityEngine;

namespace AppCore.DataManagement {
    public class DestroyIfSavedFlag : MonoBehaviour {
        [SerializeField] private bool generateNewID;
        [SerializeField] private string flagName;

        private void OnValidate() {
            if (generateNewID) {
                flagName = name + Guid.NewGuid();
                generateNewID = false;
            }
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