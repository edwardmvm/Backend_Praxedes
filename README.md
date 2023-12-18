
# API Web ASP.NET Core - Documentación

## Descripción General
**Esta API Web desarrollada en ASP.NET Core está diseñada para proporcionar una plataforma robusta y segura para la gestión de datos y autenticación de usuarios. Aquí hay una descripción detallada de sus funcionalidades principales:**

1. **Gestión de Usuarios y Autenticación:** La API implementa sistemas de autenticación JWT y autenticación básica, ofreciendo métodos seguros para la verificación de usuarios. Esto permite a los usuarios acceder de forma segura a la API utilizando tokens JWT o mediante la autenticación básica, donde las credenciales del usuario se pasan en el encabezado de la solicitud.

2. **Validación de Datos con FluentValidation:** Utiliza FluentValidation para asegurar que los datos proporcionados a través de los endpoints cumplan con criterios específicos, mejorando así la integridad de los datos y la experiencia del usuario.

3. **Operaciones de Base de Datos con Dapper:** La API utiliza Dapper, un ORM ligero, para realizar operaciones de base de datos. Esto incluye la ejecución de consultas y comandos, lo que facilita el manejo de datos con un rendimiento optimizado.

4. **Registro y Manejo de Logs:** Implementa un sistema de registro de logs personalizado, permitiendo un seguimiento detallado de las solicitudes, respuestas y posibles errores que ocurren durante las operaciones de la API. Esto es crucial para la depuración y el mantenimiento de la API.

5. **Configuración de Swagger/OpenAPI para Documentación:** La API integra Swagger/OpenAPI para proporcionar una interfaz interactiva y documentada de sus endpoints, haciendo que sea más fácil para los desarrolladores y usuarios finales entender y utilizar la API.

6. **Manejo de Modelos de Datos Específicos:** Maneja varios modelos de datos, como usuarios, grupos familiares, comentarios y publicaciones, cada uno con sus propias validaciones y operaciones específicas.

7. **Configuración y Manejo de la Conexión a la Base de Datos:** Utiliza una clase `DatabaseConnection` para gestionar todas las conexiones y operaciones de la base de datos, asegurando así una gestión eficiente y segura de los recursos de la base de datos.

**En resumen, esta API está diseñada para ser una solución integral para la gestión de usuarios, validación de datos, operaciones de base de datos, manejo de autenticaciones y documentación de servicios, proporcionando así una plataforma segura y eficiente para el desarrollo de aplicaciones web.**

## Requisitos y Configuración Avanzada
- [.NET Core SDK](https://dotnet.microsoft.com/download) es necesario para ejecutar el proyecto.
- Clonar el repositorio y navegar al directorio del proyecto.
- Ejecutar `dotnet run` desde un terminal o directamente correr el programa en Visual Studio para iniciar la API.

## Detalles de Autenticación
La API soporta autenticación JWT y autenticación básica. 

### JWT Authentication
- **Configuración**: Utiliza `JwtBearerDefaults.AuthenticationScheme` para la configuración de JWT. La clave de firma (`SymmetricSecurityKey`) se genera a partir de una clave secreta definida en las configuraciones (`AppSettings:SecretKey`), asegurando una validación segura del token.
- **Validación de Token**: Verifica la firma del token, la ausencia de un emisor o destinatario específico (`ValidateIssuer` y `ValidateAudience` están en `false`), y la validez de la vida útil del token (`ValidateLifetime`).
- **Protección de Endpoints**: Los tokens JWT se usan para autenticar y autorizar el acceso a los endpoints, como se demuestra en los controladores `UsersController` y `FamilyGroupsController`, donde se aplica el esquema de autenticación JWT.
- **Generación y Autenticación de Tokens**: En `UsersController`, se implementa un endpoint `authenticate` para autenticar usuarios y generar tokens JWT. Utiliza `JwtSecurityTokenHandler` y `SecurityTokenDescriptor` para crear tokens con una duración específica y una firma basada en la clave secreta configurada.
- **Validación y Manejo de Usuarios**: Se valida y maneja la información del usuario mediante `UserValidator`, asegurando que los datos de usuario sean correctos antes de permitir operaciones como la creación o actualización de usuarios en la base de datos.

### Basic Authentication
- **Implementación**: Implementado a través de `BasicAuthenticationHandler`, que es una clase personalizada para manejar la lógica de autenticación básica.
- **Proceso de Autenticación**: Requiere un encabezado `Authorization` con credenciales del usuario codificadas en Base64. El handler descodifica las credenciales y las valida contra los registros en la base de datos.
- **Uso en Controladores**: Ejemplificado en los controladores `PostsController` y `CommentsController`, donde se especifica el esquema de autenticación `BasicAuthentication`. Esto significa que para acceder a los endpoints definidos en estos controladores, se requiere la autenticación básica.

## Uso Extendido de Endpoints
Los endpoints se detallan en la documentación Swagger de la API. Ejemplos:

### Usuarios

- **GET `/api/users`**: Retorna una lista de usuarios.

- **GET `/api/users/{id}`**: Retorna un usuario específico por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del usuario.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna el usuario correspondiente al ID proporcionado.
    - En caso de que el usuario no sea encontrado, se devuelve un mensaje de error con el código de estado 404 Not Found.

- **POST `/api/users/AutoCreate`**: Crea automáticamente usuarios.
  - *Parámetros de solicitud* (JSON):
    - `start` (entero): El número de inicio para la generación de usuarios.
    - `count` (entero): La cantidad de usuarios que se deben crear.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un objeto JSON que contiene dos listas: `CreatedUsers` (usuarios creados) y `ExistingUsers` (usuarios existentes).

- **PUT `/api/users/{id}`**: Actualiza un usuario existente por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del usuario que se va a actualizar.
  - *Cuerpo de la solicitud* (JSON):
    - Un objeto `User` con los campos a actualizar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el usuario ha sido actualizado con éxito.
    - Si el usuario no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.

- **DELETE `/api/users/{id}`**: Elimina un usuario por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del usuario que se va a eliminar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el usuario ha sido eliminado con éxito.
    - Si el usuario no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.

### Configuración de Servicios

Se han configurado varios servicios en la aplicación:

- Se agregan controladores y servicios de FluentValidation para validar modelos.
- Se configuran esquemas de autenticación `JwtBearer` y `BasicAuthentication`.
- Se agrega Swagger/OpenAPI para la documentación de API, incluyendo la autenticación JWT y la autenticación básica.

### Middleware de Registro

Se ha agregado un middleware de registro (`LoggingMiddleware`) para la gestión de registros.

### Posts

- **GET `/api/posts`**: Retorna una lista de posts.

- **POST `/api/posts`**: Crea un nuevo post.
  - *Cuerpo de la solicitud* (JSON):
    - Un objeto `Post` con los campos `UserId`, `Title` y `Body`.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna el ID del nuevo post creado.
    - Si la validación del post falla, se devuelve un mensaje de error con el código de estado 400 Bad Request y una lista de errores de validación.
    - Si no se puede insertar el post en la base de datos, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.

- **GET `/api/posts/{id}`**: Retorna un post específico por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del post.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna el post correspondiente al ID proporcionado.
    - En caso de que el post no sea encontrado, se devuelve un mensaje de error con el código de estado 404 Not Found.

- **POST `/api/posts/bulkInsert`**: Realiza una inserción masiva de posts desde JSONPlaceholder.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando cuántos posts han sido insertados con éxito.
    - En caso de error al obtener los datos desde JSONPlaceholder, se devuelve un mensaje de error con el código de estado correspondiente.
    - En caso de error al deserializar los datos JSON, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.

- **PUT `/api/posts/{id}`**: Actualiza un post existente por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del post que se va a actualizar.
  - *Cuerpo de la solicitud* (JSON):
    - Un objeto `Post` con los campos a actualizar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el post ha sido actualizado con éxito.
    - Si el post no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.
    - Si no se puede actualizar el post en la base de datos, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.

- **DELETE `/api/posts/{id}`**: Elimina un post por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del post que se va a eliminar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el post ha sido eliminado con éxito.
    - Si el post no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.
    - Si no se puede eliminar el post de la base de datos, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.

### Comentarios

- **GET `/api/comments`**: Retorna una lista de comentarios.

- **POST `/api/comments`**: Acepta un modelo `Comment` para crear un nuevo comentario.

- **GET `/api/comments/{id}`**: Retorna un comentario específico por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del comentario.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna el comentario correspondiente al ID proporcionado.
    - En caso de que el comentario no sea encontrado, se devuelve un mensaje de error con el código de estado 404 Not Found.

- **PUT `/api/comments/{id}`**: Actualiza un comentario existente por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del comentario que se va a actualizar.
  - *Cuerpo de la solicitud* (JSON):
    - Un objeto `Comment` con los campos a actualizar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el comentario ha sido actualizado con éxito.
    - Si el comentario no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.
    - Si la validación del comentario falla, se devuelve un mensaje de error con el código de estado 400 Bad Request y una lista de errores de validación.

- **DELETE `/api/comments/{id}`**: Elimina un comentario por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del comentario que se va a eliminar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el comentario ha sido eliminado con éxito.
    - Si el comentario no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.

### Family Groups

- **GET `/api/familygroups`**: Retorna una lista de grupos familiares.

- **GET `/api/familygroups/{id}`**: Retorna un grupo familiar específico por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del grupo familiar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna el grupo familiar correspondiente al ID proporcionado.
    - En caso de que el grupo familiar no sea encontrado, se devuelve un mensaje de error con el código de estado 404 Not Found.

- **POST `/api/familygroups`**: Crea un nuevo grupo familiar.
  - *Cuerpo de la solicitud* (JSON):
    - Un objeto `FamilyGroup` con los campos `UserID`, `Cedula`, `Nombres`, `Apellidos`, `Genero`, `Parentesco`, `Edad`, `MenorEdad`, y `FechaNacimiento`.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el grupo familiar ha sido agregado con éxito.
    - Si la validación del grupo familiar falla, se devuelve un mensaje de error con el código de estado 400 Bad Request y una lista de errores de validación.
    - Si no se puede insertar el grupo familiar en la base de datos, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.

- **PUT `/api/familygroups/{id}`**: Actualiza un grupo familiar existente por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del grupo familiar que se va a actualizar.
  - *Cuerpo de la solicitud* (JSON):
    - Un objeto `FamilyGroup` con los campos a actualizar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el grupo familiar ha sido actualizado con éxito.
    - Si el grupo familiar no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.
    - Si no se puede actualizar el grupo familiar en la base de datos, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.

- **DELETE `/api/familygroups/{id}`**: Elimina un grupo familiar por su ID.
  - *Parámetros de ruta*:
    - `id` (entero): ID único del grupo familiar que se va a eliminar.
  - *Respuesta exitosa* (código de estado 200 OK):
    - Retorna un mensaje indicando que el grupo familiar ha sido eliminado con éxito.
    - Si el grupo familiar no se encuentra, se devuelve un mensaje de error con el código de estado 404 Not Found.
    - Si no se puede eliminar el grupo familiar de la base de datos, se devuelve un mensaje de error con el código de estado 500 Internal Server Error.


## Configuración de Swagger/OpenAPI
La API utiliza Swagger para documentar y probar los endpoints. Se configura para soportar tanto JWT como autenticación básica.

## Integración de Base de Datos y Logs
- `DatabaseConnection` maneja las conexiones y operaciones de base de datos.
- Los logs de solicitudes se registran en la base de datos para monitoreo y depuración.

## Modelos de Datos
- `User`: Modelo para usuarios con propiedades como `Username`, `Password`, `Email`.

---

Documentación creada con Markdown para una presentación clara y profesional.
