#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using static VRChatAvatarToolkit.MoyuToolkitUtils;
using static VRChatAvatarToolkit.Vmd2AnimUtils;

namespace VRChatAvatarToolkit
{
    [CustomEditor(typeof(DefaultAsset))]
    public class Vmd2Anim : EditorWindow
    {
        [MenuItem("Tools/Vmd2Anim", false, 100)]
        public static void ShowWindow_Vmd2Anim()
        {
            GetWindow(typeof(Vmd2Anim));
        }
        
        
        private Vector2 mainScrollPos;
        private GameObject avatar;

        private DefaultAsset vmdAsset, modelAsset;
        private Vmd2AnimParameter parameter;

        private List<AnimBool> animBoolList = new List<AnimBool>();


        private void OnEnable()
        {
            foreach (var anim in animBoolList)
            {
                anim.valueChanged.RemoveAllListeners();
                anim.valueChanged.AddListener(Repaint);
            }
            animBoolPair.valueChanged.RemoveAllListeners();
            animBoolPair.valueChanged.AddListener(Repaint);
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("MMD动作转换器");
            GUI.skin.label.fontSize = 12;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("by:如梦");
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 12;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("将VMD文件转换为动画");
            GUILayout.Space(10);
            var newVmdAsset = (DefaultAsset)EditorGUILayout.ObjectField("VMD文件", vmdAsset, typeof(DefaultAsset), true);
            if (newVmdAsset != vmdAsset)
            {
                if (newVmdAsset != null)
                {
                    var path = AssetDatabase.GetAssetPath(newVmdAsset);
                    if (!path.ToLower().EndsWith(".vmd"))
                    {
                        EditorUtility.DisplayDialog("提示", "本工具仅支持vmd文件转换", "确认");
                        return;
                    }
                }
                vmdAsset = newVmdAsset;
                parameter = AssetDatabase.LoadAssetAtPath(GetParameterFilePath(), typeof(Vmd2AnimParameter)) as Vmd2AnimParameter;
                ReadParameter();
            }
            if (parameter == null)
            {
                // 没有配置文件时
                OnGUI_CreateParameter();
            }
            else
            {
                // 有配置文件时
                OnGUI_Config();
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUI.skin.label.fontSize = 12;
        }
        // 生成配置文件UI
        private void OnGUI_CreateParameter()
        {
            var newModel = (DefaultAsset)EditorGUILayout.ObjectField("MMD模型", modelAsset, typeof(DefaultAsset), true);
            if (newModel != modelAsset)
            {
                if (newModel != null)
                {
                    var path = AssetDatabase.GetAssetPath(newModel);
                    if (!IsSupportModel(path))
                    {
                        EditorUtility.DisplayDialog("提示", "如需指定模型，请将.pmx 或.pmd文件拖放到该位置。否则请保留空白！", "确认");
                        newModel = modelAsset;
                    }
                }
                modelAsset = newModel;
            }
            GUILayout.Space(10);
            if (vmdAsset == null)
                EditorGUILayout.HelpBox("请先指定一个VMD文件", MessageType.Warning);
            if (modelAsset == null)
                EditorGUILayout.HelpBox("将使用默认模型进行解析，如果动作出现异常，请导入并指定一个支持的MMD模型", MessageType.Info);
            if (vmdAsset == null || modelAsset == null)
                GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("生成配置文件", GUILayout.Width(200)) && vmdAsset != null)
                CreateParameterFile();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        // 设置&输出动画文件
        private AnimBool animBoolPair = new AnimBool();
        private bool showMorphFrames = true;
        private void OnGUI_Config()
        {
            GUILayout.Space(10);

            GUI.skin.label.fontSize = 18;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("基本参数");
            parameter.animAngle = EditorGUILayout.FloatField("姿势旋转角度", parameter.animAngle);

            GUILayout.Space(10);
            GUI.skin.label.fontSize = 18;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("模型形态键映射");
            showMorphFrames = EditorGUILayout.Toggle("显示形态键帧数", showMorphFrames);

            mainScrollPos = GUILayout.BeginScrollView(mainScrollPos);
            if (parameter.modelPairList.Count == 0)
            {
                EditorGUILayout.HelpBox("当前模型列表为空，先点击下面按钮添加一个吧", MessageType.Info);
            }
            else
            {
                var vmdBlendShapesList = new List<string>();
                var vmdBlendShapeNames = new string[parameter.vmdMorphList.Count + 1];
                for (var i = 0; i < parameter.vmdMorphList.Count; i++)
                {
                    var m = parameter.vmdMorphList[i];
                    vmdBlendShapeNames[i + 1] = m.name;
                    if (showMorphFrames) vmdBlendShapeNames[i + 1] += "(" + m.frameList.Count + ")";
                    vmdBlendShapesList.Add(m.name);
                }

                // 左 下拉框参数
                var vmdBlendShapes = new string[vmdBlendShapesList.Count + 1];
                vmdBlendShapes[0] = vmdBlendShapeNames[0] = "(未选择)";
                vmdBlendShapesList.ToArray().CopyTo(vmdBlendShapes, 1);
                var vmdBlendShapeIndexs = new int[vmdBlendShapesList.Count];
                for (var i = 0; i < vmdBlendShapeIndexs.Length; i++)
                    vmdBlendShapeIndexs[i] = i;

                EditorGUI.BeginChangeCheck();
                for (var index = 0; index < parameter.modelPairList.Count; index++)
                {
                    var modelPairInfo = parameter.modelPairList[index];
                    // 百叶窗参数
                    AnimBool animBool;
                    {
                        if (animBoolList.Count == index)
                        {
                            animBool = new AnimBool();
                            animBool.valueChanged.AddListener(Repaint);
                            animBoolList.Add(animBool);
                        }
                        else
                        {
                            animBool = animBoolList[index];
                        }
                        var newTarget = EditorGUILayout.Foldout(animBool.target, modelPairInfo.name, true);
                        if (newTarget != animBool.target)
                        {
                            if (newTarget)
                                foreach (var _animBool in animBoolList)
                                    _animBool.target = false;
                            animBool.target = newTarget;
                            //animBoolPair.target = false;
                        }
                    }
                    // 百叶窗
                    if (EditorGUILayout.BeginFadeGroup(animBool.faded))
                    {
                        // 样式嵌套Start
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        GUILayout.Space(5);
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(5);
                        EditorGUILayout.BeginVertical();

                        // 操作按钮
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (index > 0 && GUILayout.Button("上移", GUILayout.Width(60)))
                        {
                            MoveListItem(ref parameter.modelPairList, index, index - 1);
                            MoveListItem(ref animBoolList, index, index - 1);
                            break;
                        }
                        else if (index < parameter.modelPairList.Count - 1 && GUILayout.Button("下移", GUILayout.Width(60)))
                        {
                            MoveListItem(ref parameter.modelPairList, index, index + 1);
                            MoveListItem(ref animBoolList, index, index + 1);
                            break;
                        }
                        if (GUILayout.Button("删除", GUILayout.Width(60)))
                        {
                            if (RemoveModelPair(index))
                                animBoolPair.target = animBool.target = false;
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        // 配置名称
                        var newName = EditorGUILayout.TextField("名称", modelPairInfo.name);
                        if (newName != modelPairInfo.name)
                        {
                            newName = GetNumberAlpha(newName);
                            var nameList = new List<string>();
                            foreach (var info in parameter.modelPairList)
                                nameList.Add(info.name);
                            if (!nameList.Contains(newName) && newName.Length > 0)
                                modelPairInfo.name = newName;
                        }

                        // 表情映射
                        animBoolPair.target = EditorGUILayout.Foldout(animBoolPair.target, "映射列表", true);
                        var modelBlendShapes = new string[modelPairInfo.blendShapeList.Count + 1];
                        modelBlendShapes[0] = "(未选择)";
                        modelPairInfo.blendShapeList.ToArray().CopyTo(modelBlendShapes, 1);
                        var modelBlendShapeIndexs = new int[modelBlendShapes.Length];
                        for (var i = 0; i < modelBlendShapeIndexs.Length; i++)
                            modelBlendShapeIndexs[i] = i;

                        var beAddPairInfo = false;
                        if (EditorGUILayout.BeginFadeGroup(animBoolPair.faded))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("序号", GUILayout.Width(25));
                            EditorGUILayout.LabelField("源形态键", GUILayout.MinWidth(50));
                            EditorGUILayout.LabelField("模型形态键", GUILayout.MinWidth(50));
                            EditorGUILayout.EndHorizontal();

                            var num = 0;
                            var blendShapesPairList = modelPairInfo.blendShapesPairList;
                            for (var i = 0; i < blendShapesPairList.Count; i++)
                            {
                                var info = blendShapesPairList[i];

                                var index1 = vmdBlendShapesList.Contains(info.source) ? vmdBlendShapesList.IndexOf(info.source) + 1 : 0;
                                var index2 = modelPairInfo.blendShapeList.Contains(info.target) ? modelPairInfo.blendShapeList.IndexOf(info.target) + 1 : 0;

                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(num++ + "", GUILayout.Width(25));
                                var newIndex1 = EditorGUILayout.IntPopup(index1, vmdBlendShapeNames, vmdBlendShapeIndexs);
                                var newIndex2 = EditorGUILayout.IntPopup(index2, modelBlendShapes, modelBlendShapeIndexs);
                                EditorGUILayout.EndHorizontal();
                                if (newIndex1 != index1 || newIndex2 != index2)
                                {
                                    var newSource = newIndex1 == 0 ? "" : vmdBlendShapes[newIndex1];
                                    var newTarget = newIndex2 == 0 ? "" : modelBlendShapes[newIndex2];
                                    blendShapesPairList[i].source = newSource;
                                    blendShapesPairList[i].target = newTarget;
                                }
                                beAddPairInfo = newIndex1 != 0 || newIndex2 != 0;
                                /*if (newIndex1 == 0 && newIndex2 == 0)
                                {
                                    blendShapesPairList.RemoveAt(i);
                                    i--;
                                }*/
                            }
                            if (beAddPairInfo)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(num++ + "", GUILayout.Width(25));
                                var index1 = EditorGUILayout.IntPopup(0, vmdBlendShapeNames, vmdBlendShapeIndexs);
                                var index2 = EditorGUILayout.IntPopup(0, modelBlendShapes, modelBlendShapeIndexs);
                                EditorGUILayout.EndHorizontal();
                                if (index1 != 0 || index2 != 0)
                                {
                                    var newSource = index1 == 0 ? "" : vmdBlendShapes[index1];
                                    var newTarget = index2 == 0 ? "" : modelBlendShapes[index2];
                                    blendShapesPairList.Add(new Vmd2AnimParameter.ModelPairInfo.PairInfo() { source = newSource, target = newTarget });
                                }
                            }
                        }
                        EditorGUILayout.EndFadeGroup();

                        // 样式嵌套End
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(5);
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(5);
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndFadeGroup();
                }
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(parameter);
            }
            GUILayout.EndScrollView();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            avatar = (GameObject)EditorGUILayout.ObjectField(avatar, typeof(GameObject), true);
            if (GUILayout.Button("添加模型"))
            {
                if (avatar != null)
                {
                    AddModelPairParameter(ref parameter, avatar);
                    EditorUtility.SetDirty(parameter);
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请先将你的模型拖拽放入左边的选择框内", "确认");
                }
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("导出全部动画"))
            {
                ClearConsole();
                if (ExportAllAnimation(parameter))
                    EditorUtility.DisplayDialog("提醒", "成功生成所有的动画文件！", "确认");
            }
            GUILayout.Space(10);
        }
        // 移除一个模型配置
        private bool RemoveModelPair(int index)
        {
            if (parameter == null || !EditorUtility.DisplayDialog("注意", "真的要删除这套模型配置吗？", "确认", "取消"))
                return false;
            parameter.modelPairList.RemoveAt(index);
            EditorUtility.SetDirty(parameter);
            return true;
        }
        // 读取配置文件
        private void ReadParameter()
        {
            if (parameter == null) return;
        }
        private void CreateParameterFile()
        {
            ClearConsole();
            // 导出动作动画
            string vmdPath = AssetDatabase.GetAssetPath(vmdAsset);
            string modelPath = modelAsset != null ? AssetDatabase.GetAssetPath(modelAsset) : GetScriptPath() + "Vmd2Anim/Model/model.pmx";
            var clip = GetAnimationClip(vmdPath, modelPath);
            if (clip == null)
            {
                EditorUtility.DisplayDialog("错误", "导出动作动画失败！", "确认");
                return;
            }
            clip.name = "action";

            // 处理表情数据
            var vmdMorphList = new VmdHelper(vmdPath).GetMorphList();
            if (vmdMorphList == null)
            {
                EditorUtility.DisplayDialog("错误", "处理表情数据失败！", "确认");
                return;
            }

            // 生成配置文件
            var parPath = GetParameterFilePath();
            parameter = CreateInstance<Vmd2AnimParameter>();
            parameter.vmdMorphList = vmdMorphList;

            AssetDatabase.CreateAsset(parameter, parPath);
            AssetDatabase.AddObjectToAsset(clip, parPath);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("提醒", "成功生成配置文件！", "确认");
            ReadParameter();
        }
        private string GetParameterFilePath()
        {
            if (vmdAsset == null)
                return null;
            string assetPath = AssetDatabase.GetAssetPath(vmdAsset);
            assetPath = assetPath.Substring(0, assetPath.LastIndexOf(".") + 1);
            assetPath += "asset";
            return assetPath;
        }
    }
}
#endif