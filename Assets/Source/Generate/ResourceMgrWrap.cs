﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ResourceMgrWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ResourceMgr), typeof(System.Object));
		L.RegFunction("PreloadBaseBundle", PreloadBaseBundle);
		L.RegFunction("PreloadLuaBundles", PreloadLuaBundles);
		L.RegFunction("LoadCfgFile", LoadCfgFile);
		L.RegFunction("GetAssetBundle", GetAssetBundle);
		L.RegFunction("New", _CreateResourceMgr);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("updatePath", get_updatePath, null);
		L.RegVar("instance", get_instance, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateResourceMgr(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				ResourceMgr obj = new ResourceMgr();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: ResourceMgr.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PreloadBaseBundle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ResourceMgr obj = (ResourceMgr)ToLua.CheckObject<ResourceMgr>(L, 1);
			obj.PreloadBaseBundle();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PreloadLuaBundles(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ResourceMgr obj = (ResourceMgr)ToLua.CheckObject<ResourceMgr>(L, 1);
			obj.PreloadLuaBundles();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadCfgFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ResourceMgr obj = (ResourceMgr)ToLua.CheckObject<ResourceMgr>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			string o = obj.LoadCfgFile(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAssetBundle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ResourceMgr obj = (ResourceMgr)ToLua.CheckObject<ResourceMgr>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			UnityEngine.AssetBundle o = obj.GetAssetBundle(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_updatePath(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ResourceMgr obj = (ResourceMgr)o;
			string ret = obj.updatePath;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index updatePath on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_instance(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, ResourceMgr.instance);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

