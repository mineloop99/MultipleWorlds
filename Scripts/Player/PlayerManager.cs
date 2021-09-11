using Unity.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Player;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct PlayerManager : IComponentData
{
    public int id;
    public int playerCount;
}
[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PlayerManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        PlayerStats playerStats = new PlayerStats { maxHealth = 100f, currenthHealth = 100 };

        dstManager.SetComponentData(entity, playerStats);
    }

}

public class PlayerHandle : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!GameManager.isLogedIn)
            return default;

        var jh = Entities.WithoutBurst().ForEach((ref Translation translation, ref Rotation rotation, in PlayerInformation playerInfomation) =>
        {
            for (int i = 0; i < GameManager.playerId.Count; i++)
            {
                if (playerInfomation.id == GameManager.players[GameManager.playerId[i]].id)
                    translation.Value = GameManager.players[GameManager.playerId[i]].position;
            }
        }).Schedule(inputDeps);
        jh.Complete();
        return jh;
    }
}

