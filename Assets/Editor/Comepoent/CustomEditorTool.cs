using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ComponentSelector : EditorWindow
{
    private GameObject selectedGameObject;
    private List<Component> components = new List<Component>();
    private List<ObjBinder.Item> selectedItems = new List<ObjBinder.Item>();
    

    [MenuItem("tools/Component Selector")]
    static void Init()
    {
        ComponentSelector window = (ComponentSelector)EditorWindow.GetWindow(typeof(ComponentSelector));
        window.Show();
    }
    private ObjBinder objBinder;
    private ObjBinder.Item item;
    private string objname;
    void OnGUI()
    {
        selectedGameObject = Selection.activeGameObject;

        if (selectedGameObject != null)
        {
            GUILayout.Label("所选物体：" + selectedGameObject.name);

            if (GUILayout.Button("获取组件"))
            {
                components.Clear();
                components.AddRange(selectedGameObject.GetComponents<Component>());
                objBinder = selectedGameObject.GetComponentInParent<ObjBinder>();
                selectedItems = objBinder.items.ToList();
            }
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("设置选择组件名称：");
            GUILayout.EndHorizontal();
            objname = GUILayout.TextField(objname);
            GUILayout.BeginHorizontal();
            GUILayout.Label(selectedGameObject.GetType().Name);
            if (GUILayout.Button("选择"))
            {
                if(string.IsNullOrEmpty(objname))
                {
                    Debug.Log("请输入名称！");
                    GUILayout.EndHorizontal();
                }
                else
                {
                    // item.name = selectedGameObject.name + " - " + component.GetType().Name;
                    item = new ObjBinder.Item();
                    item.name = objname;
                    item.obj = selectedGameObject;
                    selectedItems.Add(item);
                    objBinder.items = selectedItems.ToArray();
                }
            }
            GUILayout.EndHorizontal();
            foreach (Component component in components)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(component.GetType().Name);
                if (GUILayout.Button("选择"))
                {
                    if(string.IsNullOrEmpty(objname))
                    {
                        Debug.Log("请输入名称！");
                        GUILayout.EndHorizontal();
                        continue;
                    }
                    item = new ObjBinder.Item();
                    item.name = objname;
                    // item.name = selectedGameObject.name + " - " + component.GetType().Name;
                    item.obj = component;
                    selectedItems.Add(item);
                    objBinder.items = selectedItems.ToArray();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            GUILayout.Label("选择组件：");

            foreach (ObjBinder.Item item in selectedItems)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(item.name);

                if (GUILayout.Button("取消"))
                {
                    selectedItems.Remove(item);
                    objBinder.items = selectedItems.ToArray();
                    GUILayout.EndHorizontal();
                    break;
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.Space(20);
            if (GUILayout.Button("清空"))
            {
                selectedItems.Clear();
                objBinder.items = selectedItems.ToArray();
            }
        }
        else
        {
            GUILayout.Label("No GameObject selected.");
        }
    }
}