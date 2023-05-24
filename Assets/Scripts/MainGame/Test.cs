using UnityEngine;
using LuaInterface;

public class Test : MonoBehaviour {
    public static LuaState lua;
    LuaFunction main;
    private void Awake() {

        PanelMgr.instance.Init();
        ResourcesCfg.instance.LoadCfg();
        // 预加载基础的Bundle
        ResourceMgr.instance.PreloadBaseBundle();
        AfterHotUpdate();
        // UpdatePanel.Create(()=> 
        // {
        //     // 更新完毕
        // }); 
        
    }
    private void AfterHotUpdate()
    {
        // 预加载Lua的AssetBundle
        ResourceMgr.instance.PreloadLuaBundles();

        // 启动lua框架
        lua = new LuaState();
        lua.Start();
        string rootPath = Application.dataPath;
        lua.AddSearchPath(rootPath + "/Scripts");
        lua.AddSearchPath(rootPath + "/Scripts/Lua");
        lua.AddSearchPath(rootPath + "/ToLua/Lua");
        OpenLibs();
        lua.LuaSetTop(0);

        LuaBinder.Bind(lua);
        DelegateFactory.Init();
        LuaCoroutine.Register(lua, this);
        
        string funcName = "Test.lua";
        lua.DoFile(funcName);
        main = lua.GetFunction("Main");
        main.Call();
        main.Dispose();
        main = null;  
    }
    //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
    protected void OpenCJson() {
        lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
        lua.OpenLibs(LuaDLL.luaopen_cjson);
        lua.LuaSetField(-2, "cjson");

        lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
        lua.LuaSetField(-2, "cjson.safe");
    }
    /// <summary>
    /// 初始化加载第三方库
    /// </summary>
    void OpenLibs() {
        // lua.OpenLibs(LuaDLL.luaopen_pb);      
        // // lua.OpenLibs(LuaDLL.luaopen_sproto_core);
        // // lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
        // lua.OpenLibs(LuaDLL.luaopen_lpeg);
        // lua.OpenLibs(LuaDLL.luaopen_bit);
        // lua.OpenLibs(LuaDLL.luaopen_socket_core);

        this.OpenCJson();
    }
      void OnApplicationQuit()
    {
        if(lua!=null)
        {
            lua.Dispose();
            lua = null;
        }
#if UNITY_5 || UNITY_2017 || UNITY_2018	
        Application.logMessageReceived -= Log;
#else
        Application.RegisterLogCallback(null);
#endif 
    }
     
}