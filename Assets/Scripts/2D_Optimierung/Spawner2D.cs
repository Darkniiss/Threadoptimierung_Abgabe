using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    [SerializeField] private bool usesJobs;
    [SerializeField] private Transform entityPrefab;
    [SerializeField] private int spawnAmount;
    private List<Entity2D> entityList;

    public class Entity2D
    {
        public Transform transform;
        public float movementY;
    }

    private void Start()
    {
        entityList = new List<Entity2D>();
        //Spawns entities and adds them to a list
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 rndPos = new Vector3(UnityEngine.Random.Range(-13f, 13f), UnityEngine.Random.Range(-4f, 4f));
            float rndSpeed = UnityEngine.Random.Range(1f, 3f);
            Transform entityTransform = Instantiate(entityPrefab, rndPos, entityPrefab.transform.rotation);

            entityList.Add(new Entity2D
            {
                transform = entityTransform,
                movementY = rndSpeed
            });
        }
    }

    private void Update()
    {
        if (usesJobs)
        {
            NativeArray<float3> positions = new NativeArray<float3>(entityList.Count, Allocator.TempJob);
            NativeArray<float> movements = new NativeArray<float>(entityList.Count, Allocator.TempJob);

            //Transfer all variables to arrays
            for (int i = 0; i < entityList.Count; i++)
            {
                positions[i] = entityList[i].transform.position;
                movements[i] = entityList[i].movementY;
            }

            MovementJob2D job = new MovementJob2D
            {
                deltaTime = Time.deltaTime,
                positions = positions,
                movements = movements
            };

            JobHandle jobHandle = job.Schedule(entityList.Count, 100);
            jobHandle.Complete();

            //Update original values
            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i].transform.position = positions[i];
                entityList[i].movementY = movements[i];
            }

            positions.Dispose();
            movements.Dispose();
        }
        else
        {
            foreach (Entity2D entity in entityList)
            {
                entity.transform.position += new Vector3(0f, entity.movementY * Time.deltaTime);
                if (entity.transform.position.y > 4f)
                {
                    entity.movementY = -math.abs(entity.movementY);
                }
                if (entity.transform.position.y < -4f)
                {
                    entity.movementY = +math.abs(entity.movementY);
                }

                float value = 0f;
                for (int i = 0; i < 2500; i++)
                {
                    value = math.exp10(math.sqrt(value));
                }
            }
        }
    }
}
