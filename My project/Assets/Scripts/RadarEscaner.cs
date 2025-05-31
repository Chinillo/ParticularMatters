using UnityEngine;

public class RadarEscaner : MonoBehaviour
{
    public ParticleSystem sistemaParticulas; // Sistema de part�culas del radar
    public float radioExplosi�n = 5f;       // Radio de la explosi�n de part�culas
    public float cooldownRadar = 10f;      // Tiempo de espera entre usos del radar
    public float tiempoDesaparicion = 2f;  // Tiempo antes de que las part�culas desaparezcan

    private float tiempoUltimoUso = -10f;  // Registro del �ltimo uso del radar
    private ParticleSystem.Particle[] particulas; // Array para manejar las part�culas

    void Update()
    {
        // Activar el radar con la tecla "E"
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= tiempoUltimoUso + cooldownRadar)
        {
            ActivarRadar();
            tiempoUltimoUso = Time.time; // Actualizamos el tiempo del �ltimo uso
        }
    }

    void ActivarRadar()
    {
        // Emitimos las part�culas
        sistemaParticulas.transform.position = transform.position; // Posicionamos el sistema de part�culas en el jugador
        sistemaParticulas.Play();
    }

    void OnParticleCollision(GameObject other)
    {
        // Detectamos el objeto con el que colision� la part�cula
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
        // Cambiamos el color de las part�culas al colisionar
        var main = sistemaParticulas.main;
        main.startColor = color;

        // Configuramos un tiempo para que las part�culas desaparezcan
        Invoke(nameof(DesaparecerParticulas), tiempoDesaparicion);
    }

    void DesaparecerParticulas()
    {
        sistemaParticulas.Stop();
    }
}
