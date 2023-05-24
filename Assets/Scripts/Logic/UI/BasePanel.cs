using UnityEngine;
using LuaInterface;
using System;
public class BasePanel : MonoBehaviour
{
    private LuaFunction onShow;
    private LuaFunction onHide;
    private LuaFunction onUpdate;
    private LuaFunction onRegistEvent;
    private LuaFunction onUnRegistEvet;
    private string panelName;
    public GameObject panelObj { get; private set; }
    private LuaManager luaMgr;
    public void Initialize(string panelName, GameObject panelObj)
    {
        this.panelName = panelName;
        this.panelObj = panelObj;
        if(Test.lua!=null)
        {
            onShow = Test.lua.GetFunction(panelName + ".OnShow", false);
            onHide = Test.lua.GetFunction(panelName + ".OnHide", false);
            onUpdate = Test.lua.GetFunction(panelName + ".OnUpdate", false);
        }

    }

    public void Show()
    {
        if (null != onShow)
        {
            onShow.Call(panelObj);
        }
    }

    public void Hide()
    {
        Destroy(this.panelObj);
        ClearMemory();
        if (null != onHide)
            onHide.Call();
    }

    private void Update()
    {
        if (null != onUpdate)
            onUpdate.Call();
    }
    /// <summary>
    /// 清理内存
    /// </summary>
    public void ClearMemory()
    {
        GC.Collect(); 
        Resources.UnloadUnusedAssets();
        if (luaMgr != null) luaMgr.LuaGC();
    }
}
