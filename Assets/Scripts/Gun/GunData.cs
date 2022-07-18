using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival.Ingame.Gun
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Data/Gun")]
    public class GunData : ScriptableObject
    {
        public int Index;
        // 마우스 누르고 있으면 연사하는가
        public bool CanAuto;
        // 연사 속도 (아마 한번 쏘고 다음거 나가는 간격?)
        public float ShotDelay;
        // 장탄수
        public int MagazineCapacity;
        // 최대 보유 탄약
        public int AmmoMax;
        //사거리
        public float Range;
        // 데미지
        public float Damage;
        // 집탄
        [Range(0f, 45f)]
        public float Spread;
    }
}