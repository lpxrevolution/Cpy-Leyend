using UnityEngine;

public class MapController : MonoBehaviour
{
    private Vector2 touchStartPos; // Almacena la posición inicial del toque
    private Vector2 touchEndPos; // Almacena la posición final del toque
    private Vector3 startPos; // Almacena la posición inicial del objeto del mapa

    // Ajusta estos valores para tus preferencias
    private float mapWidth = 3400f; // Ancho del mapa
    private float mapHeight = 3400f; // Alto del mapa
    private float scalaX;
    private float scalaY; 


    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position; // Almacena la posición inicial del toque
                startPos = transform.position; // Almacena la posición inicial del objeto del mapa
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchEndPos = touch.position; // Almacena la posición final del toque
                Vector3 offset = new Vector3((touchEndPos - touchStartPos).x, (touchEndPos - touchStartPos).y, 0f);
                transform.position = startPos + offset; // Calcula la nueva posición del objeto del mapa
                RestrictMapToScreen(); // Aplica restricciones para que no salga de los límites
            }
        }
        if (Input.touchCount == 2)
        {
            // Obtiene los dos toques
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Calcula la distancia entre los dos toques
            float distance = Vector2.Distance(touch0.position, touch1.position);

            // Calcula la diferencia de distancia entre los dos toques en el fotograma anterior
            float prevDistance = Vector2.Distance(touch0.position - touch0.deltaPosition, touch1.position - touch1.deltaPosition);

            // Calcula el cambio de distancia entre los dos toques
            float deltaDistance = distance - prevDistance;

            // Aplica el zoom según el cambio de distancia
            Zoom(deltaDistance);
        }
        if (Input.touchCount == 0)
        {
            if (transform.localScale.magnitude < 1)
            {
                transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
        }
    }

    private void RestrictMapToScreen()
    {
        scalaX = transform.localScale.x;
        scalaY = transform.localScale.y;
        float limitX = mapWidth * scalaX / 2 - Screen.width;  // Límite en el eje X
        float limitY = mapHeight * scalaY / 2 - Screen.height; // Límite en el eje Y


        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, -limitX, limitX + Screen.width); // Aplica restricciones en el eje X
        newPosition.y = Mathf.Clamp(newPosition.y, -limitY, limitY + Screen.height); // Aplica restricciones en el eje Y

        transform.position = newPosition; // Asigna la nueva posición al objeto del mapa
    }

    void Zoom(float deltaDistance)
    {
        // Ajusta la velocidad de zoom según tus preferencias
        float zoomSpeed = 0.001f;

        // Limita la escala mínima y máxima según tus preferencias
        float minScale = 1.5f;
        float maxScale = 5.0f;

        // Aplica el zoom multiplicando la escala actual por un factor de zoom

        if (transform.localScale.magnitude > minScale)
            transform.localScale += deltaDistance * zoomSpeed * Vector3.one;
        else
            transform.localScale = new Vector3(1, 1, 1);


        transform.localScale = Vector3.ClampMagnitude(transform.localScale, Mathf.Clamp(transform.localScale.magnitude, minScale, maxScale));
    }
}
