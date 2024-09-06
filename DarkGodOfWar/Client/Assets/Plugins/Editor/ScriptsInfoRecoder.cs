
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：ScriptsInfoRecoder.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 12:00
    功能：Nothing
***************************************/
#endregion

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class ScriptsInfoRecoder : UnityEditor.AssetModificationProcessor {
    private static void OnWillCreateAsset(string path) {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs")) {
            string str = File.ReadAllText(path);
            str = str.Replace("#CreateAuthor#", Environment.UserName).Replace(
                              "#CreateTime#", string.Concat(DateTime.Now.Year, "/", DateTime.Now.Month, "/",
                                DateTime.Now.Day, " ", DateTime.Now.Hour, ":", DateTime.Now.Minute));
            File.WriteAllText(path, str);
        }
    }
}