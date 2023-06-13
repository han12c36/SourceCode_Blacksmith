using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    //public void StartCorou(IEnumerable coroutine) => StartCoroutine(coroutine);
    //public new void StartCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
    //public new void StopCoroutine(IEnumerator coroutine) => StopCoroutine(coroutine);

}

public static class CachedCoroutine
{
    public static WaitForSeconds waitForSeconds = new WaitForSeconds(.0f);
    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

}
