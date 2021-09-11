using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
namespace Player
{
    public class PlayerManagement
    {
        public int id;
        public string username;
        public float3 position;
        public Quaternion rotation;
    }
    public struct PlayerInformation : IComponentData
    {
        public int id;
        public float3 position;
    }
    public struct PlayerStats : IComponentData
    {
        public float maxHealth;
        public float currenthHealth;
        public float moveSpeed;
    }
    public struct PlayerShape
    {
        public Mesh mesh;
        public Material material;
    }

    public struct PlayerShapeBlobAsset
    {
        public BlobPtr<PlayerShape> playerShapePtr;
    }
    public struct LocalPlayer : IComponentData
    { }
}