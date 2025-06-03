using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Coflow.iMotionsPlugin.Vive
{

    public class ExampleUIToggleRecording : MonoBehaviour
    {
        UnityiMotionsManager manager;
        Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            manager = FindObjectOfType<UnityiMotionsManager>();

            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                    manager?.StartRecording();
                else
                    manager?.StopRecording();
            });
        }



    }
}