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

## Requisitos y Configuración
### Ejecución del Script de la Base de Datos
Para configurar la base de datos necesaria para esta API, sigue los siguientes pasos:

1. **Descarga el Script SQL**: [ScriptDB.sql](ScriptDB.sql).
2. **Instalación de SQL Server Management Studio (SSMS)**:
   - Si aún no tienes SQL Server Management Studio instalado, puedes descargarlo desde el [sitio web oficial de Microsoft](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).
3. **Conexión a SQL Server**:
   - Abre SQL Server Management Studio (`SSMS`).
   - Conéctate a tu servidor loal o de SQL Server utilizando las credenciales adecuadas.
4. **Ejecución del Script**:
   - En SSMS, abre el archivo `ScriptDB.sql` que descargaste en el paso 1. Puedes hacerlo haciendo clic en **"Archivo"** > **"Abrir"** > **"Archivo..."** y seleccionando el archivo `ScriptDB.sql`.
   - Asegúrate de que estás conectado al servidor `SQL Server` donde deseas ejecutar el script.
   - Haz clic en el botón "`Ejecutar`" (o presiona `F5`) para ejecutar el script SQL. Esto creará la base de datos y las tablas necesarias para que la API funcione correctamente.
5. **Verificación de la Base de Datos**:
   - Puedes verificar que la base de datos se haya creado correctamente utilizando la opción "`Explorador de objetos`" en SSMS. Deberías poder ver la nueva base de datos en la lista de bases de datos.

Con estos pasos, habrás configurado la base de datos necesaria para que la API funcione correctamente. Ahora puedes proceder a ejecutar la API y comenzar a utilizar sus funcionalidades.

### Configuración de Framework y Repositorio
- [.NET Core SDK](https://dotnet.microsoft.com/download) es necesario para ejecutar el proyecto.
- Clonar el repositorio y navegar al directorio del proyecto.
- Ejecutar `dotnet run` desde un terminal o directamente correr el programa en Visual Studio para iniciar la API.

## Configuración Inicial Importante

Si tienes algún error al ejecutar la api, es probable que debas realizar primero los siguientes pasos...

### Configuración de la Conexión a la Base de Datos
Puedes configurar el proyecto para utilizar tu propia conexión a la base de datos, ya sea con Windows Authentication o con un usuario de SQL Server. Aquí te explicaremos cómo hacerlo:

#### Opción 1: Windows Authentication
1. Abre el archivo `appsettings.json`.
2. En la sección `"ConnectionStrings"`, encuentra la clave `"DefaultConnection"`.
3. Reemplaza el valor de `"DefaultConnection"` con la cadena de conexión adecuada para tu base de datos SQL Server con Windows Authentication. La cadena de conexión debería tener un formato similar a este:
   ```json
   "DefaultConnection": "Server=NombreDelServidor;Database=NombreDeLaBaseDeDatos;Trusted_Connection=True;MultipleActiveResultSets=true;" Actualmente se encuentra así:
   ```json
       "DefaultConnection": "Data Source=(localdb)\\localsql;Initial Catalog=XYZDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Compara para realizar el cambio correspondiente...
---

### Opción 2: Usuario de SQL Server
Para configurar la conexión a la base de datos utilizando un usuario específico de SQL Server, sigue estos pasos:
1. **Abre el archivo `appsettings.json`**:
   - Este archivo contiene las configuraciones clave para tu proyecto, incluyendo la cadena de conexión a la base de datos.
2. **Encuentra la sección `"ConnectionStrings"`**:
   - Aquí se define la cadena de conexión actual para tu base de datos.
3. **Modifica la clave `"DefaultConnection"`**:
   - Reemplaza la cadena de conexión existente con una que utilice credenciales específicas de SQL Server. Por ejemplo:
     ```json
     "DefaultConnection": "Server=NombreDelServidor;Database=NombreDeLaBaseDeDatos;User Id=NombreDeUsuario;Password=Contraseña;MultipleActiveResultSets=true;"
     ```
     - `NombreDelServidor`: Reemplaza esto con el nombre o dirección IP de tu servidor SQL Server.
     - `NombreDeLaBaseDeDatos`: El nombre de la base de datos a la que te estás conectando.
     - `NombreDeUsuario`: Tu nombre de usuario de SQL Server.
     - `Contraseña`: La contraseña asociada con tu nombre de usuario de SQL Server.
     Asegúrate de reemplazar estos valores con tu información real de conexión.
4. **Guarda los Cambios**:
   - Una vez que hayas actualizado la cadena de conexión, guarda el archivo `appsettings.json`.

### Configuración de la Autenticación de Usuarios
Para poder ejecutar los métodos http en swagger, se debe autenticar usuarios en la API, es necesario proporcionar credenciales válidas. El siguiente objeto JSON es un ejemplo de cómo se deben formatear estas credenciales para el método POST `/api/Users/authenticate` en el controlador de Usuarios:

```json
{
  "userID": 1,
  "username": "Usuario1",
  "password": "Contrasenna1",
  "email": "email1@ejemplo.com",
  "isActive": true
}
```

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


## Solución de Problemas y Errores Comunes
Durante el desarrollo y la ejecución de la API, podrías encontrar varios errores. A continuación, se presenta una guía paso a paso para diagnosticar y resolver los errores más comunes.

### Error al cargar la definición de la API
Si encuentras un error similar al que se muestra en la siguiente imagen, esto indica un problema con la conexión a la base de datos o con la configuración de Swagger.

![Error al cargar la definición de la API](/Errors/NoDBConnection.png)

#### Pasos para resolver el error:

1. **Verifica la Conexión a la Base de Datos:**
   - Asegúrate de que la cadena de conexión en tu archivo `appsettings.json` sea correcta y que el servidor de la base de datos esté en funcionamiento.
   - Si estás utilizando autenticación de Windows, verifica que tu usuario tenga los permisos necesarios.
   - Para las conexiones con credenciales de SQL, confirma que el `UserID` y `Password` sean correctos.

2. **Comprobación de Swagger:**
   - Revisa que la configuración de Swagger en `Program.cs` esté bien definida y que no haya errores en la ruta del archivo `swagger.json`.
   - Asegúrate de que los XML de comentarios estén habilitados y correctamente configurados para que Swagger pueda generar la documentación de la API.

3. **Revisión de Logs:**
   - Consulta los logs de la aplicación para obtener más detalles sobre el error. Esto puede darte pistas sobre la configuración que podría estar causando el problema.

4. **Recompilación del Proyecto:**
   - A veces, una simple recompilación del proyecto puede resolver problemas temporales o de configuración.

5. **Soporte de la Comunidad:**
   - Si el error persiste, considera buscar ayuda en foros de la comunidad como Stack Overflow o en la documentación oficial de ASP.NET Core y Swagger.

---

Documentación creada con Markdown para una presentación clara y profesional.
