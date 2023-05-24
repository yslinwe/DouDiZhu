using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using System;
using UnityEngine.UI;

public class PrefabObjBinder : MonoBehaviour
{
    // [System.Serializable]
    // public class Item
    // {
    //     public string name;
    //     public UObject obj;
    // }

    // public Item[] items = new Item[0];

    private Dictionary<string, GameObject> m_itemsDic = new Dictionary<string, GameObject>();
    // private void Awake()
    // {
    //     foreach (Transform child in transform)
    //     {
    //         GameObject childObject = child.gameObject;
    //         m_itemsDic[childObject.name]=childObject;
    //         GetChildObjects(childObject);
    //     }
        
    // }
    private void GetChildObjects(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject childObject = child.gameObject;
            m_itemsDic[childObject.name]=childObject;

            // 递归获取子孙物体
            GetChildObjects(childObject);
        }
    }
    public UObject GetObj(string name)
    {
        if (0 == m_itemsDic.Count)
        {
            foreach (Transform child in transform)
            {
                GameObject childObject = child.gameObject;
                m_itemsDic[childObject.name]=childObject;
                GetChildObjects(childObject);
            }
        }
        if (m_itemsDic.ContainsKey(name))
        {
            return m_itemsDic[name];
        }
        return null;
    }

    public T GetObj<T>(string name) where T : UObject
    {
        GameObject obj = GetObj(name) as GameObject;
        return obj.GetComponent<T>();
    }

    public Button SetBtnClick(string name, Action cb)
    {
        var obj = GetObj<Button>(name);
        if (null == obj)
        {
            Debug.LogError("SetClick Error, null == obj, name:" + name);
            return null;
        }
        var btn = obj as Button;
        btn.onClick.AddListener(() =>
        {
            cb?.Invoke();
        });
        return btn;
    }
    public Text SetText(string name, string text)
    {
        var obj = GetObj<Text>(name);
        if (null == obj)
        {
            Debug.LogError("SetText Error, null == obj, name:" + name);
            return null;
        }
        var txt = obj as Text;
        txt.text = text;
        return txt;
    }

    public Toggle SetToggle(string name, Action<bool> cb)
    {
        var obj = GetObj<Toggle>(name);
        if (null == obj)
        {
            Debug.LogError("SetToggle Error, null == obj, name:" + name);
            return null;
        }
        var tgl = obj as Toggle;
        tgl.onValueChanged.AddListener((v) =>
        {
            cb?.Invoke(v);
        });
        return tgl;
    }
}
