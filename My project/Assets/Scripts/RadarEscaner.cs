using UnityEngine;

public class RadarEscaner : MonoBehaviour
{
    public ParticleSystem sistemaParticulas; // Sistema de partículas del radar
    public float radioExplosión = 5f;       // Radio de la explosión de partículas
    public float cooldownRadar = 10f;      // Tiempo de espera entre usos del radar
    public float tiempoDesaparicion = 2f;  // Tiempo antes de que las partículas desaparezcan

    private float tiempoUltimoUso = -10f;  // Registro del último uso del radar
    private ParticleSystem.Particle[] particulas; // Array para manejar las partículas

    void Update()
    {
        // Activar el radar con la tecla "E"
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= tiempoUltimoUso + cooldownRadar)
        {
            ActivarRadar();
            tiempoUltimoUso = Time.time; // Actualizamos el tiempo del último uso
        }
    }

    void ActivarRadar()
    {
        // Emitimos las partículas
        sistemaParticulas.transform.position = transform.position; // Posicionamos el sistema de partículas en el jugador
        sistemaParticulas.Play();
    }

    void OnParticleCollision(GameObject other)
    {
        // Detectamos el objeto con el que colisionó la partícula
        if (other.CompareTag("Maquina"))
        {
            CambiarColorParticulas(Color.gray);
        }
        else if (other.CompareTag("Enemigo"))
        {
            CambiarColorParticulas(Color.red);
        }
        else if (other.CompareTag("Piso"))
        {
            CambiarColorParticulas(Color.white);
        }
    }

    void CambiarColorParticulas(Color color)
    {
        // Cambiamos el color de las partículas al colisionar
        var main = sistemaParticulas.main;
        main.startColor = color;

        // Configuramos un tiempo para que las partículas desaparezcan
        Invoke(nameof(DesaparecerParticulas), tiempoDesaparicion);
    }

    void DesaparecerParticulas()
    {
        sistemaParticulas.Stop();
    }
}
