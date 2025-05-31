using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad base del jugador bajo el agua
    public float resistenciaAgua = 0.8f;   // Factor de resistencia bajo el agua
    public float gravedadReducida = 0.3f; // Gravedad reducida bajo el agua

    public float sensibilidadMouse = 100f; // Sensibilidad del mouse para la c�mara
    public Transform camaraJugador;        // Referencia a la c�mara del jugador
    public float amplitudFlotacion = 0.1f; // Amplitud del efecto de flotaci�n
    public float velocidadFlotacion = 2f;  // Velocidad del efecto de flotaci�n

    private Rigidbody rb;
    private float rotacionX = 0f;          // Para controlar la rotaci�n vertical
    private float tiempoFlotacion = 0f;    // Para calcular el movimiento de flotaci�n

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivamos la gravedad predeterminada

        // Bloqueamos el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoverJugador();
        ControlarCamara();
        AplicarFlotacion();
    }

    void MoverJugador()
    {
        // Capturamos el input del jugador
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");
        float movimientoAscender = 0f;

        // Permitir ascender/descender con las teclas Espacio y Control
        if (Input.GetKey(KeyCode.Space))
        {
            movimientoAscender = 1f; // Ascender
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            movimientoAscender = -1f; // Descender
        }

        // Calculamos la direcci�n del movimiento
        Vector3 movimiento = transform.right * movimientoHorizontal + transform.forward * movimientoVertical + transform.up * movimientoAscender;

        // Aplicamos resistencia al movimiento
        movimiento *= resistenciaAgua;

        // Movemos al jugador
        rb.AddForce(movimiento * velocidadMovimiento, ForceMode.Acceleration);

        // Aplicamos gravedad reducida
        rb.AddForce(Vector3.down * gravedadReducida, ForceMode.Acceleration);
    }

    void ControlarCamara()
    {
        // Capturamos el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        // Rotamos la c�mara verticalmente (limitamos el �ngulo para evitar giros extremos)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);
        camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // Rotamos el jugador horizontalmente
        transform.Rotate(Vector3.up * mouseX);
    }

    void AplicarFlotacion()
    {
        // Calculamos un movimiento suave hacia arriba y hacia abajo usando una funci�n seno
        tiempoFlotacion += Time.deltaTime * velocidadFlotacion;
        float desplazamientoVertical = Mathf.Sin(tiempoFlotacion) * amplitudFlotacion;

        // Aplicamos el movimiento de flotaci�n a la c�mara
        camaraJugador.localPosition = new Vector3(camaraJugador.localPosition.x, desplazamientoVertical, camaraJugador.localPosition.z);
    }
}
