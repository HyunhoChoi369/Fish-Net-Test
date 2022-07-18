using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival.Ingame.Gun
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Data/Gun")]
    public class GunData : ScriptableObject
    {
        public int Index;
        // ���콺 ������ ������ �����ϴ°�
        public bool CanAuto;
        // ���� �ӵ� (�Ƹ� �ѹ� ��� ������ ������ ����?)
        public float ShotDelay;
        // ��ź��
        public int MagazineCapacity;
        // �ִ� ���� ź��
        public int AmmoMax;
        //��Ÿ�
        public float Range;
        // ������
        public float Damage;
        // ��ź
        [Range(0f, 45f)]
        public float Spread;
    }
}