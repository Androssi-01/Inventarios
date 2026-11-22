# 📦 Portafolio FullStack - Gestión de Inventarios

¡Bienvenido a mi proyecto de portafolio! Esta es una aplicación Full-Stack completa diseñada para demostrar mis habilidades en el desarrollo de software moderno, abarcando desde la base de datos hasta el despliegue en contenedores.

## Tecnologías Utilizadas

### Backend
*   **.NET 10 (Minimal APIs):** Framework principal para la construcción de la API.
*   **Entity Framework Core:** ORM para el acceso y modelado de datos.
*   **SQL Server:** Base de datos relacional.
*   **FluentValidation:** Para validaciones de datos robustas y desacopladas.
*   **JWT (JSON Web Tokens):** Autenticación y autorización segura.
*   **xUnit:** Pruebas unitarias para asegurar la lógica de negocio.

### Frontend
*   **React 19 + Vite:** Biblioteca para la interfaz de usuario rápida y moderna.
*   **React Router:** Navegación entre vistas.
*   **Axios:** Cliente HTTP con interceptores para el manejo automático de tokens.
*   **Bootstrap 5:** Diseño responsivo y componentes UI.
*   **Swagger UI React:** Documentación interactiva de la API integrada en el frontend.

### DevOps e Infraestructura
*   **Docker & Docker Compose:** Contenedores para el despliegue unificado de Backend, Frontend y Base de Datos.
*   **Nginx:** Servidor web para el Frontend y proxy inverso hacia la API.

##  Características Principales
*   **CRUD Completo:** Gestión de Clientes, Productos y Proveedores.
*   **Seguridad:** Rutas protegidas, autenticación con JWT y manejo de sesiones expiradas.
*   **Manejo Global de Errores:** Middleware personalizado para respuestas JSON limpias (ProblemDetails).
*   **Paginación en el Servidor:** Listados eficientes con filtros y ordenamiento.
*   **Documentación Viva:** Interfaz de Swagger integrada directamente en la app de React.
*   **Base de Datos Autoinicializable:** Scripts SQL que se ejecutan automáticamente al levantar el contenedor.

##  Cómo ejecutar el proyecto (Localmente)

El proyecto está completamente dockerizado, por lo que **no necesitas instalar .NET, Node.js ni SQL Server** en tu máquina.

1.  Clona este repositorio.
2.  Asegúrate de tener **Docker Desktop** instalado y corriendo.
3.  En la raíz del proyecto, ejecuta:
    ```bash
    docker compose up --build -d