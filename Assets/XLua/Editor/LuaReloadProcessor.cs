using UnityEngine;
using UnityEditor;
using XLua;
using XLuaTest;
using System.IO;

public class LuaReloadProcessor : AssetPostprocessor
{
    public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < importedAsset.Length; i++)
            {
                bool isLuaFile = importedAsset[i].EndsWith(".lua") || importedAsset[i].EndsWith(".lua.txt");
                if (isLuaFile)
                {
                    if (UIBehaviour.luaEnv != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(importedAsset[i]));
                        UIBehaviour.luaEnv.DoString("local hardreload = require 'hardreload' hardreload.reload ('" + fileName + "')");
                    }
                }
            }
        }
    }

}
