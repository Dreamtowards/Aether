#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static VRChatAvatarToolkit.MoyuToolkitUtils;

namespace VRChatAvatarToolkit
{
    public class Vmd2AnimUtils
    {
        public static bool ExportAllAnimation(Vmd2AnimParameter parameter)
        {
            try
            {
                var sourceFilePath = AssetDatabase.GetAssetPath(parameter);
                var targetFilePath = sourceFilePath.Substring(0, sourceFilePath.LastIndexOf("."));
                var sourceClip = AssetDatabase.LoadAssetAtPath(sourceFilePath, typeof(AnimationClip)) as AnimationClip;
                {
                    // 修改Anim设置
                    AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(sourceClip);
                    clipSetting.orientationOffsetY = parameter.animAngle;
                    clipSetting.loopTime = false;
                    clipSetting.loopBlendOrientation = true;
                    clipSetting.loopBlendPositionY = true;
                    clipSetting.loopBlendPositionXZ = true;
                    AnimationUtility.SetAnimationClipSettings(sourceClip, clipSetting);
                }
                {
                    var outputPath = targetFilePath + "_NoMorph.anim";
                    var clip = new AnimationClip();
                    EditorUtility.CopySerialized(sourceClip, clip);
                    AssetDatabase.DeleteAsset(outputPath);
                    AssetDatabase.CreateAsset(clip, outputPath);
                }
                foreach (var modelPair in parameter.modelPairList)
                {
                    var clip = new AnimationClip();
                    EditorUtility.CopySerialized(sourceClip, clip);

                    var frameRate = clip.frameRate;
                    foreach (var pairInfo in modelPair.blendShapesPairList)
                    {
                        if (pairInfo.source == "" || pairInfo.target == "") continue;
                        var bind = new EditorCurveBinding
                        {
                            path = "Body",
                            propertyName = "blendShape." + pairInfo.target,
                            type = typeof(SkinnedMeshRenderer)
                        };
                        List<Vmd2AnimParameter.MorphKeyFrameRecord.Frame> frameList = null;
                        foreach (var info in parameter.vmdMorphList)
                            if (info.name == pairInfo.source)
                                frameList = info.frameList;
                        if (frameList == null) continue;

                        var kfSum = frameList.Count;
                        var keyframes = new Keyframe[kfSum];
                        for (var i = 0; i < kfSum; i++)
                        {
                            var frame = frameList[i];
                            keyframes[i] = new Keyframe(frame.time / frameRate, frame.value * 100f);
                        }
                        AnimationUtility.SetEditorCurve(clip, bind, new AnimationCurve(keyframes));
                    }
                    var outputPath = targetFilePath + "_" + modelPair.name + ".anim";
                    AssetDatabase.DeleteAsset(outputPath);
                    AssetDatabase.CreateAsset(clip, outputPath);
                }
                AssetDatabase.Refresh();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return false;
        }
        public static bool AddModelPairParameter(ref Vmd2AnimParameter parameter, GameObject avatar)
        {
            if (parameter == null || avatar == null) return false;
            // 处理模型数据
            var modelBlendShapeList = new List<string>();
            {
                var body = avatar.transform.Find("Body") ?? avatar.transform.Find("body");
                if (body == null)
                {
                    EditorUtility.DisplayDialog("错误", "在模型中找不到Body元素！", "确认");
                    return false;
                }
                var skinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer == null)
                {
                    EditorUtility.DisplayDialog("错误", "在Body元素中找不到SkinnedMeshRenderer组件！", "确认");
                    return false;
                }
                var sharedMesh = skinnedMeshRenderer.sharedMesh;
                var blendShapeCount = sharedMesh.blendShapeCount;
                for (var i = 0; i < blendShapeCount; i++)
                    modelBlendShapeList.Add(sharedMesh.GetBlendShapeName(i));
            }

            // 添加参数
            var item = new Vmd2AnimParameter.ModelPairInfo();
            var name = GetNumberAlpha(avatar.name);
            item.blendShapeList = modelBlendShapeList;

            foreach (var morphInfo in parameter.vmdMorphList)
            {
                if (morphInfo.frameList.Count <= 1) continue;
                var morphName = morphInfo.name;
                var simList = GetSimilarName(morphName, modelBlendShapeList);
                if (simList.Count > 0)
                    foreach (var val in simList)
                        item.blendShapesPairList.Add(new Vmd2AnimParameter.ModelPairInfo.PairInfo() { source = morphName, target = val });
                else
                    item.blendShapesPairList.Add(new Vmd2AnimParameter.ModelPairInfo.PairInfo() { source = morphName, target = "" });
            }

            // 名字查重
            {
                var names = new List<string>();
                foreach (var info in parameter.modelPairList)
                    names.Add(info.name);
                while (names.Contains(name))
                    name += "_new";
            }
            item.name = name;
            parameter.modelPairList.Add(item);
            EditorUtility.SetDirty(parameter);
            return true;
        }
        public static AnimationClip GetAnimationClip(string vmdPath, string modelPath)
        {
            vmdPath = vmdPath.Replace("\\", "/");
            Print("开始转换VMD文件...");
            var animPath = vmdPath.Substring(0, vmdPath.Length - 4);
            animPath += ".anim";
            var animName = vmdPath.Substring(vmdPath.LastIndexOf("/") + 1);
            animName = animName.Substring(0, animName.LastIndexOf("."));
            var dir = Environment.CurrentDirectory + "/";
            var tempDir = GetScriptPath() + "Vmd2Anim/Temp/";
            var exePath = dir + GetScriptPath() + "Vmd2Anim/PMX2FBX/pmx2fbx.exe";
            if (File.Exists(animPath))
            {
                File.Delete(animPath);
                File.Delete(animPath + ".meta");
            }
            try
            {
                // 准备Temp目录
                ClearTempFile();
                Directory.CreateDirectory(tempDir);
                File.Copy(vmdPath, tempDir + "0.vmd");
                File.Copy(modelPath, tempDir + "0." + GetFileType(modelPath));
                vmdPath = dir + tempDir + "0.vmd";
                modelPath = dir + tempDir + "0." + GetFileType(modelPath);

                // 执行
                GUILayout.Label("正在转换为FBX文件...");
                Debug.Log(exePath);
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", modelPath, vmdPath);
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    if (process.ExitCode > 0)
                        throw new Exception("PMX2FBX ExitCode：" + process.ExitCode);
                    else
                        Print("用户手动结束");
                    return null;
                }
                // 删除不需要的文件
                var files = Directory.GetFiles(tempDir);
                foreach (var path in files)
                {
                    if (!path.EndsWith(".fbx"))
                        File.Delete(path);
                }
                // 重新加载资源
                AssetDatabase.Refresh();
                // 提取Anim
                var oldClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(tempDir + "0.fbx", typeof(AnimationClip));
                var clip = new AnimationClip();
                EditorUtility.CopySerialized(oldClip, clip);
                clip.name = animName;
                // 修改Anim设置
                AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
                clipSetting.loopTime = false;
                clipSetting.loopBlendOrientation = true;
                clipSetting.loopBlendPositionY = true;
                clipSetting.loopBlendPositionXZ = true;
                AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
                // 成功后删除Temp文件
                ClearTempFile();
                return clip;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("错误", "转换过程中出现错误，无法继续！请检查PMX和VMD文件是否有效！", "确认");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
            return null;
        }
        internal static List<string> GetSimilarName(string sourceName, List<string> nameList)
        {
            var renameDictionary = new Dictionary<string, string>();
            {
                var lines = ReadTxt(GetScriptPath() + "Vmd2Anim/RenameDictionary.ini").Split('\n');
                foreach (var line in lines)
                {
                    var vals = line.Split('|');
                    renameDictionary.Add(vals[0], vals[1]);
                }
            }
            foreach (var pair in renameDictionary)
                sourceName = sourceName.Replace(pair.Key, pair.Value);
            var targetName = "";
            {
                var similar = 0;
                var _name1 = sourceName.ToLower();
                foreach (var name in nameList)
                {
                    var _name2 = name.ToLower();
                    if (_name1 == _name2)
                    {
                        targetName = name;
                        break;
                    }
                    else if (similar < 1 && _name2.Contains(_name1.ToLower()))
                    {
                        targetName = name;
                        similar = 1;
                    }
                    else if (similar < 2 && _name2.EndsWith(_name1))
                    {
                        targetName = name;
                        similar = 2;
                    }
                    else if (similar < 3 && _name2.EndsWith("_" + _name1))
                    {
                        targetName = name;
                        similar = 3;
                    }
                }
            }

            var list = new List<string>();
            if (targetName == "") return list;
            list.Add(targetName);

            var another = "";
            if (targetName.EndsWith("Left"))
                another = targetName.Replace("Left", "Right");
            else if (targetName.EndsWith("left"))
                another = targetName.Replace("left", "right");
            else if (targetName.EndsWith("_L"))
                another = targetName.Replace("_L", "_R");
            else if (targetName.EndsWith("_l"))
                another = targetName.Replace("_l", "_r");
            if (another != "" && nameList.Contains(another))
                list.Add(another);
            return list;
        }
        internal static bool IsSupportModel(string path)
        {
            path = path.ToLower();
            return path.EndsWith(".pmx") || path.EndsWith(".pmd");
        }
        private static void ClearTempFile()
        {
            var tempDir = GetScriptPath() + "Vmd2Anim/Temp";
            try
            { Directory.Delete(tempDir, true); }
            catch (Exception) { }
        }
    }
    public class AutoChangeImporter_Vmd2Anim : AssetPostprocessor
    {
        public void OnPreprocessModel()
        {
            var modelImporter = (ModelImporter)assetImporter;
            // Vmd2Anim
            if (assetPath == GetScriptPath() + "Vmd2Anim/Temp/0.fbx")
                modelImporter.animationType = ModelImporterAnimationType.Human;
        }
    }
}
#endif