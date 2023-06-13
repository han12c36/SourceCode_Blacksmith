using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public void StartCorou(IEnumerator coroutine) => StartCoroutine(coroutine);
    public void StopCorou(IEnumerator coroutine) => StopCoroutine(coroutine);
    public void StopAllCorou(IEnumerator coroutine) => StopAllCoroutines();
}

public static class CachedCoroutine
{
    public static WaitForSeconds waitForSeconds = new WaitForSeconds(.0f);
    public static WaitForSeconds waitForHalf = new WaitForSeconds(.5f);

    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
}
