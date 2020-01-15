/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

namespace XLuaTest
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class UIBehaviour : MonoBehaviour
    {
        public TextAsset luaScript;
        public static LuaEnv luaEnv; //all lua behaviour shared one luaenv only!
        private Action<LuaTable> luaStart;
		private LuaTable scriptEnv;
        public Injection[] injections;

        void Awake()
        {
            luaEnv = new LuaEnv();
            luaEnv.AddBuildin("clonefunc", XLua.LuaDLL.Lua.OpenCloneFunc);
            luaEnv.DoString(@"local hardreload = require 'hardreload'
require = hardreload.require");

            luaEnv.DoString(@"require 'ButtonInteraction'");

            scriptEnv = luaEnv.Global.Get<LuaTable>("ui");;

            scriptEnv.Get("start", out luaStart);
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }
        }

        // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart(scriptEnv);
            }
        }

        void OnDestroy()
        {
            luaStart = null;
            scriptEnv.Dispose();
        }
    }
}
