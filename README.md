# Gestión de Reservas de Salones
Una API RESTful para la gestión de reservas de salones de eventos, desarrollada con ASP.NET Core 8.0. La arquitectura sigue el patrón de Clean Architecture para una estructura limpia y desacoplada.

## 1. Estructura del Proyecto
El proyecto está dividido en capas para separar las responsabilidades:

Gestion.Api: La API y los controladores.

Gestion.Application: Contiene la lógica de negocio, servicios y DTOs.

Gestion.Domain: Las entidades de negocio.

Gestion.Infrastructure: El acceso a la base de datos (con SQLite).

Gestion.Tests: Las pruebas unitarias.

## 2. Configuración y Ejecución del Proyecto
Sigue estos pasos para ejecutar el proyecto en tu entorno local usando Visual Studio Community.

Abrir el Proyecto: Abre el archivo de solución Gestion.sln con Visual Studio.

Configurar Proyecto de Inicio: Haz clic derecho en el proyecto Gestion.Api y selecciona "Set as Startup Project".

Ejecutar la API: Presiona F5 o haz clic en el botón de Run en la barra de herramientas. Visual Studio restaurará automáticamente las dependencias, compilará el proyecto y lo ejecutará.

La API se abrirá en tu navegador. Puedes acceder a la documentación interactiva de Swagger en la URL proporcionada.

## 3. Pruebas Unitarias
Para ejecutar las pruebas del proyecto:

En Visual Studio, navega al menú Test > Test Explorer.

Haz clic en "Run All Tests" para ejecutar todas las pruebas unitarias y verificar la lógica de negocio.
