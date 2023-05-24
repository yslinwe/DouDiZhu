using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class ObjBinder : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class Item
    {
        public string name;
        public UnityEngine.Object obj;
    }
    public Item[] items = new Item[0];
    private Dictionary<string, UnityEngine.Object> m_itemsDic = new Dictionary<string, UnityEngine.Object>();
    
    public UnityEngine.Object GetObj(string name)
    {
        if (0 == m_itemsDic.Count)
        {
             foreach (Item item in items)
            {
                m_itemsDic[item.name]=item.obj;
            }
        }
        if (m_itemsDic.ContainsKey(name))
        {
            return m_itemsDic[name];
        }
        else
        {
            Debug.Log("没有"+name);
        }
        return null;
    }
     public T GetObj<T>(string name) where T : UnityEngine.Object
    {
        return GetObj(name) as T;
    }

    public Button SetBtnClick(string name, Action cb)
    {
        var obj = GetObj(name);
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
        var obj = GetObj(name);
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
        var obj = GetObj(name);
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
