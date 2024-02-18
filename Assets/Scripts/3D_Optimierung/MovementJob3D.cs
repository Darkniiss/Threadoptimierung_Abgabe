using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct MovementJob3D : IJobParallelFor
{
    public NativeArray<float3> positionArray;
    public NativeArray<float3> moveVecArray;
    public NativeArray<float3> rotationArray;
    public NativeArray<float3> rotateVecArray;
    public float deltaTime;

    public void Execute(int index)
    {
        //Entity movement
        positionArray[index] += moveVecArray[index] * deltaTime;
        rotationArray[index] += rotateVecArray[index] * deltaTime;

        //Bounds
        if (positionArray[index].x > 9f || positionArray[index].z > 9f)
        {
            moveVecArray[index] = -math.abs(moveVecArray[index]);
        }
        if (positionArray[index].x < -9f || positionArray[index].z < -9f)
        {
            moveVecArray[index] = +math.abs(moveVecArray[index]);
        }

        float value = 0f;
        for (int i = 0; i < 2500; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}
