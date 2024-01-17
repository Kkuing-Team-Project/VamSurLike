using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void InitCoroutineHandler()
    {
        if (monoInstance != null)
            return;

        monoInstance = new GameObject($"[{nameof(CoroutineHandler)}]").AddComponent<CoroutineHandler>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(IEnumerator coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
        coroutine = null;
    }
}
