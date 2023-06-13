using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolingManager : SubManager
{
    public GameObject[] prefabs;

    Dictionary<string, Queue<GameObject>> poolingObjDic;
    GameObject[] objBoxes;


    public override void SettingManagerForNextScene(int nextSceneIndex)
    {
        if (!isActivated) isActivated = true;

    }
    public override void ManagerInitailized()
    {
        CreateBoxes();
        FillAllBoxs();
    }
    public void CreateBoxes()
    {
        objBoxes = new GameObject[prefabs.Length];

        for (int i = 0; i < prefabs.Length; ++i)
        {
            if (prefabs[i])
            {
                GameObject box = new GameObject(prefabs[i].name + Constants.Box);
                box.transform.SetParent(this.gameObject.transform);
                objBoxes[i] = box;
            }
        }
    }

    private void FillAllBoxs(int count = 50)
    {
        if (poolingObjDic == null) poolingObjDic = new Dictionary<string, Queue<GameObject>>();

        for (int i = 0; i < prefabs.Length; i++)
        {
            Queue<GameObject> tempQueue = new Queue<GameObject>();

            for (int k = 0; k < count; ++k)
            {
                GameObject tempObj = Instantiate(prefabs[i], objBoxes[i].transform);
                tempObj.name.Replace(Constants.Clone, string.Empty);
                tempObj.SetActive(false);
                tempObj.transform.SetParent(objBoxes[i].transform);
                tempQueue.Enqueue(tempObj);
            }
            poolingObjDic.Add(prefabs[i].name, tempQueue);
        }
    }
    public void FillBox(string objName, int count)
    {
        var queueInDic = poolingObjDic.FirstOrDefault(t => t.Key == objName);
        string boxName = objName + Constants.Box;

        for (int i = 0; i < objBoxes.Length; ++i)
        {
            if (objBoxes[i] != null)
            {
                if (objBoxes[i].name.Equals(boxName))
                {
                    for (int k = 0; k < count; k++)
                    {
                        GameObject tempObj = Instantiate(prefabs[i], objBoxes[i].transform);
                        tempObj.name.Replace(Constants.Clone, string.Empty);
                        tempObj.SetActive(false);
                        tempObj.transform.SetParent(objBoxes[i].transform);
                        queueInDic.Value.Enqueue(tempObj);
                    }
                }
            }
        }
    }

    public GameObject LentalObj(string objName, int count = 1)
    {
        var queueInDic = poolingObjDic.FirstOrDefault(t => t.Key == objName);

        if (queueInDic.Value.Count < count)
        {
            FillBox(objName, count * 2);
            return LentalObj(objName, count);
        }
        else
        {
            GameObject tempObj = queueInDic.Value.Dequeue();
            tempObj.SetActive(true);
            tempObj.transform.SetParent(null);
            return tempObj;
        }
    }

    public void ReturnObj(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        string realName = obj.name.Replace(Constants.Clone, string.Empty);
        string boxName = realName + Constants.Box;
        var queueInDic = poolingObjDic.FirstOrDefault(t => t.Key == realName);

        for (int i = 0; i < objBoxes.Length; ++i)
        {
            if (objBoxes[i] != null)
            {
                if (objBoxes[i].name.Equals(boxName))
                {
                    obj.transform.SetParent(objBoxes[i].transform);
                    obj.SetActive(false);
                    queueInDic.Value.Enqueue(obj);
                }
            }
        }
    }


    public void PlayEffect(string name, Transform trans, GameObject owner)
    {
        GameObject effect = LentalObj(name);
        effect.transform.position = trans.position;
        effect.transform.rotation = trans.rotation;
        effect.GetComponentInChildren<ParticleSystem>().transform.localScale = owner.transform.localScale;
    }
    public void PlayEffect(string name, Transform trans)
    {
        GameObject effect = LentalObj(name);
        effect.transform.position = trans.position;
        effect.transform.rotation = trans.rotation;
    }

    public void PlayEffect_DontRotation(string name, Transform trans, GameObject owner)
    {
        GameObject effect = LentalObj(name);
        effect.transform.position = trans.position;
        Vector3 vec = new Vector3(0.0f, owner.transform.rotation.eulerAngles.y, 0.0f);
        effect.transform.eulerAngles = vec;
        effect.GetComponentInChildren<ParticleSystem>().transform.localScale = owner.transform.localScale;
    }

    
}
