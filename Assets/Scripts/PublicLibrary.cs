using Enums;
using Structs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract partial class PublicLibrary
{
    public static float Lerp_L_1(float p0, float p1, float x) { return (p1 - p0) * x + p0; }
    public static Vector3 Lerp_L_1(Vector3 p0, Vector3 p1, float x) { return (p1 - p0) * x + p0; }
    public static float Lerp_L_2(float p0, float p1, float p2, float x)
    {
        float a = 1.0f - x;
        return (a * a * p0) + (2 * x * a * p1) + (x * x * p2);
    }
    public static Vector3 Lerp_L_2(Vector3 p0, Vector3 p1, Vector3 p2, float x)
    {
        float a = 1.0f - x;
        return (a * a * p0) + (2 * x * a * p1) + (x * x * p2);
    }
    public static float Lerp_L_3(float p0, float p1, float p2, float p3, float x)
    {
        float a = 1.0f - x;
        return a * a * a * p0 + 3 * a * a * x * p1 + 3 * a * x * x * p2 + x * x * x * p3;
    }
    public static Vector3 Lerp_L_3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float x)
    {
        float a = 1.0f - x;
        return a * a * a * p0 + 3 * a * a * x * p1 + 3 * a * x * x * p2 + x * x * x * p3;
    }
    public static float Lerp_LC(float a, float b, float x)
    {
        return (b - a) * (1.0f - Mathf.Cos(x * Mathf.PI)) * 0.5f + a;
    }
    public static Vector3 Lerp_LC_V(Vector3 a, Vector3 b, float x)
    {
        return (b - a) * (1.0f - Mathf.Cos(x * Mathf.PI)) * 0.5f + a;
    }
    public static Quaternion Lerp_LC_Q(Vector3 a, Vector3 b, float x)
    {
        return Quaternion.Euler((b - a) * (1.0f - Mathf.Cos(x * Mathf.PI)) * 0.5f + a);
    }

    public static GameObject RegisterInBox(GameObject ParentBox, string ChildBoxName)
    {
        GameObject parent = GameObject.Find(ParentBox.name);
        if (parent == null) parent = PublicLibrary.CreateBox(ParentBox.name);

        GameObject childBox = PublicLibrary.CreateBox(ChildBoxName);
        childBox.transform.SetParent(parent.transform);
        return childBox;
    }

    public static GameObject CreateBox(string BoxName)
    {
        GameObject boxObj = GameObject.Find(BoxName);
        if (boxObj == null)
        {
            boxObj = new GameObject();
            boxObj.name = BoxName;
        }
        return boxObj;
    }

    public static GameObject CreateBox(string parentBoxName, string childBoxName)
    {
        GameObject parentBox = CreateBox(parentBoxName);
        GameObject childBox = CreateBox(childBoxName);
        childBox.transform.SetParent(parentBox.transform);
        return parentBox;
    }

    public static GameObject CreateBox(GameObject parentBox, string childBoxName)
    {
        GameObject childBox = CreateBox(childBoxName);
        childBox.transform.SetParent(parentBox.transform);
        return childBox;
    }
    public static GameObject GetNameBox(string BoxName)
    {
        GameObject boxObj = GameObject.Find(BoxName);
        if (boxObj == null)
        {
            boxObj = new GameObject();
            boxObj.name = BoxName;
        }
        return boxObj;
    }
    public static int GetGCD(int a, int b)
    {
        int n;
        if (a < b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        while (b != 0)
        {
            n = a % b;
            a = b;
            b = n;
        }
        return a;
    }
    public static Vector2 RandomVec(Vector2 originPos, float range)
    {
        float range_X, range_Y;
        range_X = Random.Range((range / 2) * -1, range / 2);
        range_Y = Random.Range((range / 2) * -1, range / 2);
        Vector2 randomPos = new Vector2(range_X, range_Y);

        Vector2 RandomPos = originPos + randomPos;
        return RandomPos;
    }

    public static Vector3 RandomVec(Vector3 originPos, float range)
    {
        float range_X, range_Z;
        range_X = Random.Range((range / 2) * -1, range / 2);
        range_Z = Random.Range((range / 2) * -1, range / 2);
        Vector3 randomPos = new Vector3(range_X, 0f, range_Z);

        Vector3 RandomPos = originPos + randomPos;
        return RandomPos;
    }
    public static int Think(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs) { total += elem; }
        float randomPoint = Random.value * total;
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i]) return i;
            else randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }

    public static bool IsAnimationAlmostFinish(Animator animCtrl, string animationName, float ratio = 0.95f)
    {
        Debug.Log(ratio);
        //Debug.Log(animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
            if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsAnimationCompletelyFinish(Animator animCtrl, string animationName, float ratio = 1.0f)
    {
        if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            //여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
            if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsAnimationPlay(Animator animCtrl, string animationName, int animationLayer)
    {
        if (animCtrl.GetCurrentAnimatorStateInfo(animationLayer).IsName(animationName))
        {
            //여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
            return true;
        }
        return false;
    }

    public static float GetAnimPercent(Animator animCtrl, string animationName)
    {
        if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            //여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
            return animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        Debug.Log("이 애니메이션 아닌디?");
        return 0f;
    }

    public static float GetAnimLength(Animator animCtrl, string name)
    {
        float time = 0;

        RuntimeAnimatorController ac = animCtrl.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;
        }

        return time;
    }

    public static float GetAngle(Vector3 from, Vector3 to) // -180 ~ 180
    {
        Vector3 v = to - from;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static float CalculateAngle(Vector3 from, Vector3 to) // 0 ~ 360
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    public static string GetThousandCommaText(int data)
    {
        return string.Format("{0:#,###}", data);
    }

    public static void SortListByField<T>(List<T> list, System.Func<T, int> selector)
    {
        Comparer<T> comparer = Comparer<T>.Create((x, y) => selector(x).CompareTo(selector(y)));
        list.Sort(comparer);
    }

    public static void SortArrayByField<T>(T[] array, System.Func<T, int> selector)
    {
        System.Array.Sort(array, Comparer<T>.Create((x, y) => selector(x).CompareTo(selector(y))));
    }
    public static string[] CustomSortArrayByName(string[] array)
    {
        System.Array.Sort(array, (a, b) =>
        {
            int aInt, bInt;
            bool aIsInt = int.TryParse(a, out aInt);
            bool bIsInt = int.TryParse(b, out bInt);

            if (aIsInt && bIsInt)
            {
                return aInt.CompareTo(bInt);
            }
            else if (aIsInt)
            {
                return 1; // a is greater than b
            }
            else if (bIsInt)
            {
                return -1; // a is less than b
            }
            else
            {
                return a.CompareTo(b);
            }
        });
        return null;
    }

    public static Button FindButtonByName(Button[] btns, string buttonName)
    {
        foreach (Button button in btns)
        {
            if (button.name == buttonName)
            {
                return button;
            }
        }
        return null;
    }

    public static Window FindWindowByName(Window[] btns, string buttonName)
    {
        foreach (Window button in btns)
        {
            if (button.name == buttonName)
            {
                return button;
            }
        }
        return null;
    }
}

namespace CustomDic
{
    [System.Serializable]
    //[CanEditMultipleObjects]
    //[ExecuteInEditMode]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public List<TKey> g_InspectorKeys;
        public List<TValue> g_InspectorValues;

        public SerializableDictionary()
        {
            g_InspectorKeys = new List<TKey>();
            g_InspectorValues = new List<TValue>();
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// 새로운 KeyValuePair을 추가하며, 인스펙터도 업데이트
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// KeyValuePair을 삭제하며, 인스펙터도 업데이트
        /// </summary>
        /// <param name="key"></param>
        public new void Remove(TKey key)
        {
            base.Remove(key);
            SyncInspectorFromDictionary();
        }

        public void OnBeforeSerialize()
        {
        }
        /// <summary>
        /// 인스펙터를 딕셔너리로 초기화
        /// </summary>
        public void SyncInspectorFromDictionary()
        {
            //인스펙터 키 밸류 리스트 초기화
            g_InspectorKeys.Clear();
            g_InspectorValues.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                g_InspectorKeys.Add(pair.Key); g_InspectorValues.Add(pair.Value);
            }
        }

        /// <summary>
        /// 딕셔너리를 인스펙터로 초기화
        /// </summary>
        public void SyncDictionaryFromInspector()
        {
            //딕셔너리 키 밸류 리스트 초기화
            foreach (var key in Keys.ToList())
            {
                base.Remove(key);
            }

            for (int i = 0; i < g_InspectorKeys.Count; i++)
            {
                //중복된 키가 있다면 에러 출력
                if (this.ContainsKey(g_InspectorKeys[i]))
                {
                    Debug.LogError("중복된 키가 있습니다.");
                    break;
                }
                base.Add(g_InspectorKeys[i], g_InspectorValues[i]);
            }
        }

        public void OnAfterDeserialize()
        {
            Debug.Log(this + string.Format("인스펙터 키 수 : {0} 값 수 : {1}", g_InspectorKeys.Count, g_InspectorValues.Count));

            //인스펙터의 Key Value가 KeyValuePair 형태를 띌 경우
            if (g_InspectorKeys.Count == g_InspectorValues.Count)
            {
                SyncDictionaryFromInspector();
            }
        }
    }
}


