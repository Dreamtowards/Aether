#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static VRChatAvatarToolkit.Vmd2AnimParameter;

namespace VRChatAvatarToolkit
{
    public class VmdHelper
    {
        private StreamHelper streamHelper;
        public VmdHelper(string vmdPath)
        {
            vmdPath = vmdPath.Replace("\\", "/");
            streamHelper = new StreamHelper(vmdPath);
        }
        // 处理信息到配置文件
        internal List<MorphKeyFrameRecord> GetMorphList()
        {
            var morphList = new List<MorphKeyFrameRecord>();
            streamHelper.Reset();

            // 基本信息
            var version = 0;
            {
                var versionName = streamHelper.ReadString(30);
                if (versionName == "Vocaloid Motion Data file")
                    version = 1;
                else if (versionName == "Vocaloid Motion Data 0002")
                    version = 2;
                if (version == 0)
                {
                    EditorUtility.DisplayDialog("提示", "VMD数据错误，请检查文件是否已损坏！", "确认");
                    return null;
                }
                streamHelper.SkipByte(version == 1 ? 10 : 20); // 跳过文件名
            }
            // 骨骼数据 BoneKeyFrameRecord
            {
                // 跳过
                var sum = streamHelper.ReadInt();
                streamHelper.SkipByte(111 * sum);
            }
            // 表情数据 MorphKeyFrameRecord
            var morphKeyFrameRecordMap = new Dictionary<string, MorphKeyFrameRecord>();
            {
                var sum = streamHelper.ReadInt();
                for (var i = 0; i < sum; i++)
                {
                    var name = streamHelper.ReadString(15);
                    var frameTime = streamHelper.ReadInt();
                    var value = streamHelper.ReadFloat();

                    if (!morphKeyFrameRecordMap.ContainsKey(name))
                    {
                        morphKeyFrameRecordMap.Add(name, new MorphKeyFrameRecord()
                        {
                            name = name,
                            frameList = new List<MorphKeyFrameRecord.Frame>()
                        });
                    }

                    var frame = new MorphKeyFrameRecord.Frame
                    {
                        time = frameTime,
                        value = value
                    };
                    morphKeyFrameRecordMap[name].frameList.Add(frame);
                }
            }
            var morphKeyFrameRecordList = new List<MorphKeyFrameRecord>();
            foreach (var record in morphKeyFrameRecordMap.Values)
                morphKeyFrameRecordList.Add(record);
            return morphKeyFrameRecordList;
        }

        // 文件流读取类
        public class StreamHelper
        {
            private Stream stream;
            public StreamHelper(string vmdPath)
            {
                FileStream fileStream = new FileStream(vmdPath, FileMode.Open);
                var fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);
                stream = new MemoryStream(fileBytes);
                fileStream.Close();
            }
            public void Reset()
            {
                stream.Position = 0;
            }
            public int ReadByte()
            {
                return stream.ReadByte();
            }
            public byte[] ReadByte(int len)
            {
                var data = new byte[len];
                for (var i = 0; i < len; i++)
                    data[i] = (byte)stream.ReadByte();
                return data;
            }
            public void SkipByte(int len)
            {
                stream.Position += len;
            }
            public string ReadString(int len)
            {
                var data = ReadByte(len);
                var count = len;
                for (var i = 0; i < len; i++)
                {
                    if (data[i] == 0)
                    {
                        count = i;
                        break;
                    }
                }
                return System.Text.Encoding.GetEncoding("Shift-JIS").GetString(data, 0, count);
            }
            public int ReadInt()
            {
                var data = ReadByte(4);
                return BitConverter.ToInt32(data, 0);
            }
            public float ReadFloat()
            {
                var data = ReadByte(4);
                return BitConverter.ToSingle(data, 0);
            }
        }
    }
}
#endif