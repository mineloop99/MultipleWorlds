using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;

public class CameraController : JobComponentSystem
{
    readonly float sensitivity = 200f;
    readonly float clampAngle = 85f;


    float verticalRotation = 0;
    float horizontalRotation = 0;
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!GameManager.isLogedIn)
            return default;
        float deltaTime = Time.DeltaTime;
        Entities.WithAll<CameraAuthoring>().WithoutBurst().ForEach((ref Translation translation, ref Rotation rotation) =>
        {
            translation.Value = GameManager.players[Client.instance.myId].position;
            float _mouseVertical = -Input.GetAxis("Mouse Y");
            float _mouseHorizontal = Input.GetAxis("Mouse X");

            verticalRotation += _mouseVertical * sensitivity * deltaTime;
            horizontalRotation += _mouseHorizontal * sensitivity * deltaTime;

            verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

            rotation.Value = Quaternion.Euler(0f, horizontalRotation, 0f);

            GameManager.players[Client.instance.myId].rotation = rotation.Value;

            rotation.Value = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        }).Run();
        return default;
    }
}
