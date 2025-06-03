using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Vive
{
    public abstract class PassthroughController : MonoBehaviour
    {
        public abstract void EnablePassthrough();

        public abstract void DisablePassthrough();
    }
}