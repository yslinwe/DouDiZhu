using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIManager : MonoBehaviour
{
    public List<GameObject> uiObjects = new List<GameObject>();

    private void Start()
    {
        // 关闭所有的 UI 对象
        CloseAllUIObjects();
    }
    public GameObject GetUIObj(string uiObjectName)
    {
        // 遍历所有的 UI 对象，找到匹配的名称并开启
        foreach (GameObject uiObject in uiObjects)
        {
            if (uiObject.name == uiObjectName)
            {
                return uiObject;
            }
        }
        return null;
    }
    public void OpenUIObject(string uiObjectName)
    {
        // 遍历所有的 UI 对象，找到匹配的名称并开启
        foreach (GameObject uiObject in uiObjects)
        {
            if (uiObject.name == uiObjectName)
            {
                uiObject.SetActive(true);
                return;
            }
        }

        Debug.LogError($"UI object '{uiObjectName}' not found!");
    }

    public void CloseUIObject(string uiObjectName)
    {
        // 遍历所有的 UI 对象，找到匹配的名称并关闭
        foreach (GameObject uiObject in uiObjects)
        {
            if (uiObject.name == uiObjectName)
            {
                uiObject.SetActive(false);
                return;
            }
        }

        Debug.LogError($"UI object '{uiObjectName}' not found!");
    }

    public void CloseAllUIObjects()
    {
        // 关闭所有的 UI 对象
        foreach (GameObject uiObject in uiObjects)
        {
            uiObject.SetActive(false);
        }
    }
}
