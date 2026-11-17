# API RESTful E-Commerce

Backend robusto y escalable para una plataforma de comercio electrónico,desarrollado bajo los principios de arquitectura limpia, inyección de dependencias y buenas prácticas de desarrollo en C#.

##  Tecnologías y Herramientas
- **Framework:** .NET 10 (LTS) / ASP.NET Core
- **Lenguaje:** C#
- **ORM & Seguridad:** Entity Framework Core y ASP.NET Core Identity
- **Base de Datos:** SQL Server (Azure SQL Database)
- **Autenticación:** JSON Web Tokens (JWT) con validación de roles
- **Mapeo de Objetos:** Mapster (Implementación para DTOs)
- **Despliegue en Producción:** Microsoft Azure App Service (Entorno Linux)
- **Documentación y Pruebas:** Swagger (OpenAPI) y Postman

##  Características Principales
- **Seguridad y Accesos:** Sistema de autenticación seguro con JWT. Gestión de usuarios y control de acceso estricto basado en roles (Administrador / Cliente) implementado con ASP.NET Core Identity.
- **Data Seeding Automatizado:** Inicialización automática de la base de datos en tiempo de ejecución para poblar roles por defecto y la cuenta maestra de administrador.
- **Gestión de Catálogo de Productos:** Endpoints CRUD completos para la administración de Categorías y Productos. Uso del patrón Repositorio y DTOs para abstraer la lógica de la base de datos y proteger las entidades internas.
- **Lógica de Carrito de Compras:** Endpoints estructurados para la adición de productos al carrito, cálculo de totales y gestión de sesiones de compra asociadas al usuario autenticado.
- **Manejo Seguro de Entornos:** Configuración de variables de entorno en la nube para proteger secretos de aplicación (SecretKey) y cadenas de conexión a base de datos.
- **Documentación Interactiva:** Interfaz de Swagger expuesta y configurada para permitir la prueba directa de los endpoints, incluyendo la inyección de tokens de autorización (Bearer) desde el navegador.