SISTEMA GESTOR DE CLIENTES
==========================

1. INTEGRANTES FINALES DEL GRUPO
   Herberth Masis Canales 
   Michael Castro Herrera
   Carlos Villalobos Picado

2. REPOSITORIO
   https://github.com/chrstation/G7PracticaProgramada1.git

3. ESPECIFICACION BASICA DEL PROYECTO

   a. Arquitectura (estructura por capas, .NET 10, ASP.NET Core MVC)
      - GestorClientes.Entidades : Class Library. Modelos del dominio (Cliente, Telefono).
      - GestorClientes.Datos     : Class Library. CAPA DE ACCESO A DATOS. DbContext de
                                   EF Core e implementacion del repositorio (SQLite).
      - GestorClientes.Negocio   : Class Library. CAPA LOGICA DE NEGOCIO. Servicio con
                                   validaciones y reglas (cedula unica, correo valido,
                                   telefonos validos).
      - GestorClientes.Web       : ASP.NET Core MVC (presentacion). Controlador, modelos
                                   de vista (ViewModels), vistas Razor y configuracion
                                   de la inyeccion de dependencias.

      Dependencias: Web -> Negocio -> Datos -> Entidades

      Vistas: Index (listado con telefonos), Details (detalle), Create (registrar),
      Edit (modificar), Delete (confirmar borrado; elimina tambien los telefonos en
      cascada).

   b. Paquetes NuGet y librerias
      - Microsoft.EntityFrameworkCore.Sqlite (10.0.x)
      - Bootstrap 5.3 (estilos, cargado desde CDN en _Layout.cshtml; requiere internet)

   c. Principios SOLID y patrones de diseño
      SOLID:
      - S (Responsabilidad unica): cada clase hace una sola cosa (repositorio = datos,
        servicio = reglas de negocio, controlador = flujo HTTP, vistas = presentación).
      - O (Abierto/cerrado): se puede cambiar la fuente de datos creando otra
        implementación de IClienteRepositorio sin modificar el servicio.
      - L (Sustitución de Liskov): cualquier implementación de IClienteRepositorio o
        IClienteServicio puede sustituir a otra sin romper el sistema.
      - I (Segregación de interfaces): interfaces pequeñas y especificas.
      - D (Inversion de dependencias): el controlador depende de IClienteServicio y el
        servicio de IClienteRepositorio (abstracciones), no de clases concretas.

      Patrones:
      - MVC (Model - View - Controller).
      - Repository (ClienteRepositorio).
      - Service Layer (ClienteServicio).
      - Dependency Injection / Inversion of Control (Program.cs).
      - ViewModel / DTO (ClienteViewModel, TelefonoViewModel).
      - Arquitectura en capas (Layered Architecture).
