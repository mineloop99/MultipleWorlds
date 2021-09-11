using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Player;
using Unity.Rendering;
using Unity.Jobs;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    public static GameManager instance;
    public static bool isLogedIn = false;
    public static Dictionary<int, PlayerManagement> players = new Dictionary<int, PlayerManagement>();
    public static List<int> playerId = new List<int>();

    EntityManager entityManager; 
    Entity entityCam;
    Entity _player; 
    BlobAssetStore blobAssetStore;
    GameObjectConversionSettings settings;

    ComponentType[] componentTypes = new ComponentType[2];
    private void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
    void Start()
    {
        EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(Camera));
        blobAssetStore = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        entityCam = entityQuery.GetSingletonEntity();
    }
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        //Declare entity settings
        Entity entity;
        //Add specific component and instantiate entity
        if (_id == Client.instance.myId)
        {
            entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
            _player = entityManager.Instantiate(entity);
            entityManager.AddComponentData(_player, new LocalPlayer { });
            entityManager.AddComponentData(entityCam, new CameraAuthoring { });
            isLogedIn = true;
        }
        else
        {
            entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
            _player = entityManager.Instantiate(entity);
        }

        //SetComponent and AddComponent
        float3 pos = new float3(_position);
        entityManager.AddComponentData(_player, new PlayerStats { });
        entityManager.AddComponentData(_player, new PlayerInformation { id = _id });
        entityManager.SetComponentData(_player, new Translation { Value = pos });
        entityManager.SetComponentData(_player, new Rotation { Value = Quaternion.identity });

        //Add to Static list and static dictionary
        players.Add(_id, new PlayerManagement { id = _id , username = _username});
        playerId.Add(_id);

        //Dispose and destroy
        entityManager.DestroyEntity(entity);

        //
    }


}

//public class GameManagerConstructor : GameObjectConversionSystem
//{
  
//    protected override void OnUpdate()
//    {
//        using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp))
//        {
//            ref PlayerShapeBlobAsset playerBlobAsset =  ref blobBuilder.ConstructRoot<PlayerShapeBlobAsset>();

//            var playerShape = blobBuilder.Allocate(ref playerBlobAsset.playerShapePtr);
//            playerShape = new PlayerShape { };

//            BlobAssetReference<PlayerShapeBlobAsset> blobAssetReference = blobBuilder.CreateBlobAssetReference<PlayerShapeBlobAsset>(Allocator.Persistent);
//            Debug.Log("BLOBASSET IS CREATED: "+blobAssetReference.IsCreated);
//        }
//    }
//}
