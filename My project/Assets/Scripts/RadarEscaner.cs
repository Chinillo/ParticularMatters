using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadarEscaner : MonoBehaviour
{
    public ParticleSystem sistemaParticulas;
    public ParticleSystem subEmisor; // Asignar el sistema de partículas del sub-emisor en el Inspector
    public float cooldownRadar = 10f;
    public float tiempoDesaparicion = 2f;

    private float tiempoUltimoUso = -10f;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        sistemaParticulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= tiempoUltimoUso + cooldownRadar)
        {
            ActivarRadar();
            tiempoUltimoUso = Time.time;
        }
    }

    void ActivarRadar()
    {
        sistemaParticulas.transform.position = transform.position;
        sistemaParticulas.transform.rotation = Quaternion.identity;
        sistemaParticulas.Play();
        Invoke(nameof(DesaparecerParticulas), tiempoDesaparicion);
    }

    void OnParticleCollision(GameObject other)
    {
        if (collisionEvents == null)
            collisionEvents = new List<ParticleCollisionEvent>();
        else
            collisionEvents.Clear();

        int numCollisionEvents = sistemaParticulas.GetCollisionEvents(other, collisionEvents);

        Color colorColision = Color.magenta;
        if (other.CompareTag("Enemigo"))
            colorColision = Color.red;
        else if (other.CompareTag("Maquina"))
            colorColision = Color.yellow;
        else if (other.CompareTag("Piso"))
            colorColision = Color.white;

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 colPos = collisionEvents[i].intersection;

            // Emite una partícula en el punto de colisión con el color deseado
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.position = colPos;
            emitParams.startColor = colorColision;
            emitParams.startSize = 1f; // Ajusta el tamaño del punto
            subEmisor.Emit(emitParams, 1);
        }
    }

    void DesaparecerParticulas()
    {
        sistemaParticulas.Stop();
    }
}
