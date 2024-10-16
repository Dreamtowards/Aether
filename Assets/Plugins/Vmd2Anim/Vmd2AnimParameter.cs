#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace VRChatAvatarToolkit
{
    public class Vmd2AnimParameter : ScriptableObject
    {
        // VMD表情动作类
        [System.Serializable]
        public class MorphKeyFrameRecord
        {
            public string name;
            public List<Frame> frameList;
            [System.Serializable]
            public class Frame
            {
                public int time;
                public float value;
            }
        }
        // 模型映射信息类
        [System.Serializable]
        public class ModelPairInfo
        {
            public string name;
            public List<string> blendShapeList = new List<string>();
            public List<PairInfo> blendShapesPairList = new List<PairInfo>();
            [System.Serializable]
            public class PairInfo
            {
                public string source = "";
                public string target = "";
            }
        }

        //public DefaultAsset vmdAsset, modelAsset;
        public float animAngle = 0;
        public List<MorphKeyFrameRecord> vmdMorphList = new List<MorphKeyFrameRecord>();
        public List<ModelPairInfo> modelPairList = new List<ModelPairInfo>();
    }
}
#endif