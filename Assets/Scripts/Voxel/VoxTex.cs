using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    // just for Convenient Access. 
    [ExecuteInEditMode]
    public class VoxTex : MonoBehaviour
    {
        [ShowInInspector]
        public VoxelRegistry m_VoxReg;

        void Start()
        {
            Refresh();
        }

        [Button]
        void Refresh()
        {
            Refresh(m_VoxReg);
            Debug.Log("VoxTex Registry initialized. ");
        }

        public static VoxelRegistry.VoxelProto 
            brick_roof, concrete, dirt, dirt_black, dirt_farmland, grass, grass_meadow, grass_moss, iron, iron_treadplate,
            log_oak, plank, rock, rock_cliff, rock_deepslate, rock_jungle, rock_mossy, rock_smooth, rock_volcanic, sand,
            stone, stone_brick;

        static void Refresh(VoxelRegistry reg)
        {
            brick_roof = reg.Get("brick_roof");
            concrete = reg.Get("concrete");
            dirt = reg.Get("dirt");
            dirt_black = reg.Get("dirt_black");
            dirt_farmland = reg.Get("dirt_farmland");
            grass = reg.Get("grass");
            grass_meadow = reg.Get("grass_meadow");
            grass_moss = reg.Get("grass_moss");
            iron = reg.Get("iron");
            iron_treadplate = reg.Get("iron_treadplate");
            log_oak = reg.Get("log_oak");
            plank = reg.Get("plank");
            rock = reg.Get("rock");
            rock_cliff = reg.Get("rock_cliff");
            rock_deepslate = reg.Get("rock_deepslate");
            rock_jungle = reg.Get("rock_jungle");
            rock_mossy = reg.Get("rock_mossy");
            rock_smooth = reg.Get("rock_smooth");
            rock_volcanic = reg.Get("rock_volcanic");
            sand = reg.Get("sand");
            stone = reg.Get("stone");
            stone_brick = reg.Get("stone_brick");
            
        }

    }
}