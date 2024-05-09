using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CohesionBehavior : Steering
{
    [SerializeField]
    private float threshold = 1f;

    [SerializeField]
    private float decayCoefficient = 5f;

    private Transform[] targets;

    void Start()
    {
        SteeringBehaviorController[] agents = FindObjectsOfType<SteeringBehaviorController>();
        targets = new Transform[agents.Length - 1];
        int count = 0;

        foreach (SteeringBehaviorController agent in agents)
        {
            if (agent.gameObject != gameObject)
            {
                targets[count++] = agent.transform;
            }
        }
    }

    public override SteeringData GetSteering(SteeringBehaviorController steeringController)
    {
        SteeringData steering = new SteeringData();

        Vector3 media = CalcularMediaTargets();
 
        float distancia = media.magnitude;
        if (distancia < threshold)
        {
            float strength = Mathf.Min(decayCoefficient, steeringController.maxAcceleration);
            media.Normalize();

            steering.linear += strength * new Vector2(media.x, media.y);
        }

        return steering;
    }

    private Vector3 CalcularMediaTargets()
    {
        // Con esta parte se calcula la posicion media de todas las abejas
        // La parte de luego seguir al jugador no sabemos como hacerla
        Vector3 suma = Vector3.zero;

        foreach (Transform target in targets)
        {
            Vector3 direccion = target.position - transform.position;
            suma += direccion;
        }

        return suma.normalized / targets.Length;
    }
}