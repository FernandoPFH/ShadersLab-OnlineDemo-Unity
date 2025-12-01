using System.Linq;
using UnityEngine;

public class AnimationBackgroundObjects : MonoBehaviour
{
    private AnimationState state;
    private float TimeElapsed;

    void Update()
    {
        TimeElapsed += Time.deltaTime;

        switch (state)
        {
            case AnimationState.Initializing:
                if (TimeElapsed >= AnimationBackgroundObjectsSettings.InitialDelay)
                {
                    TimeElapsed = 0f;
                    state = AnimationState.Runing;
                }
                break;
            case AnimationState.Runing:
                foreach (AnimationBackgroundObjectsSettings.MaterialWithAnimation materialWithAnimation in AnimationBackgroundObjectsSettings.AnimationCurvePerColor.Values)
                    materialWithAnimation.material.SetFloat(AnimationBackgroundObjectsSettings.ShaderProperty, materialWithAnimation.animationCurve.Evaluate(TimeElapsed));

                if (TimeElapsed >= AnimationBackgroundObjectsSettings.AnimationCurvePerColor.First().Value.animationCurve.keys.Last().time)
                {
                    TimeElapsed = 0f;
                    state = AnimationState.OnDelay;
                }
                break;
            case AnimationState.OnDelay:
                if (TimeElapsed >= AnimationBackgroundObjectsSettings.BetweenDelay)
                {
                    TimeElapsed = 0f;
                    state = AnimationState.Runing;
                }
                break;
        }
    }

    private void OnApplicationQuit()
    {
        foreach (AnimationBackgroundObjectsSettings.MaterialWithAnimation materialWithAnimation in AnimationBackgroundObjectsSettings.AnimationCurvePerColor.Values)
            materialWithAnimation.material.SetFloat(AnimationBackgroundObjectsSettings.ShaderProperty, materialWithAnimation.animationCurve.Evaluate(0f));
    }

    private enum AnimationState
    {
        Initializing,
        Runing,
        OnDelay
    }
}
