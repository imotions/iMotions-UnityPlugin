using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coflow.iMotionsPlugin.Meta
{
    public class EyeBlinkRetriever_Meta : EyeBlinkRetriever
    {
        OVRFaceExpressions faceExpressions;

        protected override void Awake()
        {
            base.Awake();

            faceExpressions = FindObjectOfType<OVRFaceExpressions>();
        }

        protected override void Update()
        {
            base.Update();

            if (faceExpressions)
            {
                // Retrieve left eye closed weight
                faceExpressions.TryGetFaceExpressionWeight(
                    OVRFaceExpressions.FaceExpression.EyesClosedL,
                    out leftEyeBlinking
                );

                // Retrieve right eye closed weight
                faceExpressions.TryGetFaceExpressionWeight(
                    OVRFaceExpressions.FaceExpression.EyesClosedR,
                    out rightEyeBlinking
                );
            }
            else
            {
                leftEyeBlinking = 0;
                rightEyeBlinking = 0;
            }
        }
    }
}
