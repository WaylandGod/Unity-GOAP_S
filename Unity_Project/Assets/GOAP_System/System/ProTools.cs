﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

namespace GOAP_S.PT
{
    public enum NodeUIMode
    {
        SET_STATE, //State in which the user can set action node attributes
        EDIT_STATE //State in which the user can set node description/name
    }

    public enum VariableType
    {
        _undefined = 0,
        _int,
        _float,
        _char,
        _string,
        _vector2,
        _vector3,
        _vector4,
        _array,
        _enum,
        _class
    }

    public static class ProTools
    {
        //Defines ===============================
        public const int BLACKBOARD_MARGIN = 250;
        public const int INITIAL_ARRAY_SIZE = 10;

        //Universal allocate class method =======
        public static T AllocateClass<T>(this object myobj)
        {
            //Get the class to allocate type
            System.Type class_ty = ((MonoScript)myobj).GetClass();
            //Instantiate a class of type class_ty
            object x = System.Activator.CreateInstance(class_ty, false);
            //Return the allocated class casted to type T
            return (T)x;
        }

        //Find assets by type T =================
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            //Get all the assets GUID
            string[] guids = AssetDatabase.FindAssets(null, new[] { "Assets" });
            for (int i = 0; i < guids.Length; i++)
            {
                //Transform GUIDs to paths
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                //Get type T assets using the paths
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                {
                    //Add the asset to the list of type T
                    assets.Add(asset);
                }
            }
            return assets;
        }

        //Find asset by path ====================
        public static T FindAssetByPath<T>(string path) where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets(null, path.Split('\\'));

            if (guids.Length == 0 || guids.Length > 1)
            {
                return null;
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            //Get type T assets using the paths
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            return asset;
        }

        //Find properties methods ===============
        public static PropertyInfo[] FindAllPropertiesInGameObject(GameObject target)
        {
            //Total strings to allocate in the array
            int total_paths_count = 0;

            //Count game object properties
            int game_object_properties_count = target.GetType().GetProperties().Length;
            total_paths_count += game_object_properties_count;

            //Count components properties
            Component[] agent_components = target.GetComponents(typeof(Component));
            foreach (Component comp in agent_components)
            {
                total_paths_count += comp.GetType().GetProperties().Length;
            }

            //Allocate array
            PropertyInfo[] properties_info = new PropertyInfo[total_paths_count];
            int index = 0;

            //Add game object properties
            foreach (PropertyInfo property_info in typeof(GameObject).GetProperties())
            {
                properties_info[index] = property_info;
                index += 1;
            }

            //Add components properties
            foreach (Component comp in agent_components)
            {
                PropertyInfo[] comp_properties = comp.GetType().GetProperties();
                foreach (PropertyInfo comp_property_info in comp_properties)
                {
                    properties_info[index] = comp_property_info;
                    index+=1;
                }
            }

            return properties_info;

        }

        public static PropertyInfo[] FindConcretePropertiesInGameObject(GameObject target, Type target_property_type)
        {
            //Allocate the list
            List<PropertyInfo> properties_list = new List<PropertyInfo>();

            //Collect game object properties that match with the target type
            foreach (PropertyInfo property_info in typeof(GameObject).GetProperties())
            {
                if(property_info.PropertyType == target_property_type)
                {
                    properties_list.Add(property_info);
                }
            }

            //Collect components properties that match with the target type
            Component[] agent_components = target.GetComponents(typeof(Component));
            foreach (Component comp in agent_components)
            {
                PropertyInfo[] comp_properties = comp.GetType().GetProperties();
                foreach (PropertyInfo comp_property_info in comp_properties)
                {
                    if (comp_property_info.PropertyType == target_property_type)
                    {
                        properties_list.Add(comp_property_info);
                    }
                }
            }

            //Retur list converted to array
            return properties_list.ToArray();
        }


        //VariableType to System.Type ===========
        public static Type VariableTypeToSystemType(VariableType var_type)
        {
            switch (var_type)
            {
                case VariableType._int:     return typeof(int);     
                case VariableType._float:   return typeof(float);   
            }

            //No found type return
            return null;
        }

/*public static T Cast<T>(this object myobj)
{
    System.Type ty = myobj.GetType();
    if (ty == UnityEngine.Object)
    {
        return myobj;
    }
    try
    {
        return (T)Convert.ChangeType(myobj, typeof(T));
    }
    catch (InvalidCastException)
    {
        return default(T);
    }
}*/


        /*public static T Cast<T>(this object myobj)
{
    System.Type objectType = myobj.GetType();
    System.Type target = typeof(T);
    object x = System.Activator.CreateInstance(target, false);
    MemberInfo[] member_info_array = objectType.GetMembers();
    List<MemberInfo> list = new List<MemberInfo>(objectType.GetMembers());

    var z = from source in list
            where source.MemberType == MemberTypes.Property
            select source;

    var d = from source in target.GetMembers().//.ToList()
            where source.MemberType == MemberTypes.Property
            select source;
    List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name).ToList().Contains(memberInfo.Name)).ToList();
    PropertyInfo propertyInfo;
    object value;
    foreach (var memberInfo in members)
    {
        propertyInfo = typeof(T).GetProperty(memberInfo.Name);
        value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

        propertyInfo.SetValue(x, value, null);
    }
    return (T)x;
}*/
        /*
            MemberInfo[] member_info_array = class_ty.GetMembers();
            Action_GS t = new MoveAction_GS();
            PropertyInfo property_info;
            object val;
            /*foreach (var variable in member_info_array)
            {
                if (variable.MemberType == MemberTypes.Field)
                {
                    property_info = typeof(T).GetProperty(variable.Name);
                    val = myobj.GetType().GetProperty(variable.Name).GetValue(myobj, null);

                    property_info.SetValue(x, val, null);
                }
            }

            
        }
        
             List<MemberInfo> list = new List<MemberInfo>(objectType.GetMembers());

             var z = from source in list
                     where source.MemberType == MemberTypes.Property
                     select source;

             var d = from source in target.GetMembers()//.ToList()
                     where source.MemberType == MemberTypes.Property
                     select source;

             List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name).ToList().Contains(memberInfo.Name)).ToList();
             PropertyInfo propertyInfo;
             object value;
             foreach (var memberInfo in members)
             {
                 propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                 value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                 propertyInfo.SetValue(x, value, null);
             }
             return (T)x;
         }
         
        public static object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();

            UnityEditor.Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance,
                null, o, null);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }

            return p;
        }*/
    }
}