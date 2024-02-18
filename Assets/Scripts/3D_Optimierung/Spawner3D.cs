using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Spawner3D : MonoBehaviour
{
    [SerializeField] private bool usesJobs;
    [SerializeField] private int spawnAmount;
    [SerializeField] private Transform entityPrefab;
    private List<Entity3D> entityList;

    public class Entity3D
    {
        public Transform transform;
        public Vector3 rotation;
        public Vector3 moveVec;
        public Vector3 rotationVec;
    }

    private void Start()
    {
        entityList = new List<Entity3D>();
        //Spawns entities and adds them to a list
        for (int i = 0; i < spawnAmount; i++)
        {
            Transform entityTransform = Instantiate(entityPrefab, new Vector3(UnityEngine.Random.Range(-9f, 9f), 0.5f, UnityEngine.Random.Range(-9f, 9f)), Quaternion.identity);
            entityList.Add(new Entity3D
            {
                transform = entityTransform,
                rotation = entityTransform.rotation.eulerAngles,
                moveVec = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)),
                rotationVec = new Vector3(UnityEngine.Random.Range(-45f, 45f), UnityEngine.Random.Range(-45f, 45f), UnityEngine.Random.Range(-45f, 45f)),
            });
        }
    }

    private void Update()
    {
        if (usesJobs)
        {
            NativeArray<float3> positionArray = new NativeArray<float3>(entityList.Count, Allocator.TempJob);
            NativeArray<float3> moveVecArray = new NativeArray<float3>(entityList.Count, Allocator.TempJob);
            NativeArray<float3> rotationArray = new NativeArray<float3>(entityList.Count, Allocator.TempJob);
            NativeArray<float3> rotateVecArray = new NativeArray<float3>(entityList.Count, Allocator.TempJob);

            //Transfer all variables to arrays
            for (int i = 0; i < entityList.Count; i++)
            {
                positionArray[i] = entityList[i].transform.position;
                moveVecArray[i] = entityList[i].moveVec;
                rotationArray[i] = entityList[i].rotation;
                rotateVecArray[i] = entityList[i].rotationVec;
            }

            MovementJob3D job = new MovementJob3D
            {
                deltaTime = Time.deltaTime,
                positionArray = positionArray,
                moveVecArray = moveVecArray,
                rotationArray = rotationArray,
                rotateVecArray = rotateVecArray
            };

            JobHandle jobHandle = job.Schedule(entityList.Count, 100);
            jobHandle.Complete();

            //Update original values
            for (int i = 0; i < entityList.Count; i++)
            {
                entityList[i].transform.position = positionArray[i];
                entityList[i].moveVec = moveVecArray[i];
                entityList[i].rotation = rotationArray[i];
                entityList[i].rotationVec = rotateVecArray[i];
            }

            positionArray.Dispose();
            moveVecArray.Dispose();
            rotationArray.Dispose();
            rotateVecArray.Dispose();
        }
        else
        {

            foreach (Entity3D entity in entityList)
            {
                entity.transform.position += entity.moveVec * Time.deltaTime;
                entity.rotation += entity.rotationVec * Time.deltaTime;

                if (entity.transform.position.x > 9f || entity.transform.position.z > 9f)
                {
                    entity.moveVec = -math.abs(entity.moveVec);
                }
                if (entity.transform.position.x < -9f || entity.transform.position.z < -9f)
                {
                    entity.moveVec = +math.abs(entity.moveVec);
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


