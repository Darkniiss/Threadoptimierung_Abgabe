using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct MovementJob2D : IJobParallelFor
{
    public NativeArray<float3> positions;
    public NativeArray<float> movements;
    public float deltaTime;

    public void Execute(int index)
    {
        //Entity movement
        positions[index] += new float3(0f, movements[index] * deltaTime, 0f);
        if (positions[index].y > 4f)
        {
            movements[index] = -math.abs(movements[index]);
        }
        if (positions[index].y < -4f)
        {
            movements[index] = +math.abs(movements[index]);
        }

        float value = 0f;
        for (int i = 0; i < 2500; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}
