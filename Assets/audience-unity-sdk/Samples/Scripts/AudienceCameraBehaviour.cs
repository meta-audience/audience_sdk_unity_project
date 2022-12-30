using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudienceSDK;

namespace AudienceSDK.Sample
{
    public class AudienceCameraBehaviour : AudienceCameraBehaviourBase
    {
        public override void Init(AudienceSDK.Camera camera)
        {
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this.DelayedInit(camera);
        }

        protected override void DelayedInit(AudienceSDK.Camera camera)
        {
            base.DelayedInit(camera);
        }

        protected override void OnGUI()
        {
            base.OnGUI();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
