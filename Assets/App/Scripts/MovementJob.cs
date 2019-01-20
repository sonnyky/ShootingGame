using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Jobs;

namespace ShootingGame.JobSystem
{
    [BurstCompile]
    public struct MovementJob : IJobParallelForTransform
    {
        public float moveSpeed;
        public float topBound;
        public float bottomBound;
        public float deltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 pos = transform.position;
            pos.y += moveSpeed * deltaTime;
            transform.position = pos;
        }

      
    }
}