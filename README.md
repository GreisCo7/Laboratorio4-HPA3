# Laboratorio 4

## Objetivo
Implementar una interfaz gráfica en C# Windows Forms que permita capturar datos textuales y gráficos de un producto. 

## Requisitos Previos

| Tecnología | Versión / Tipo |
| :--- | :--- |
| **Lenguaje** | C# (.NET Framework / .NET) |
| **Entorno de Desarrollo** | Visual Studio 2026 |
| **Base de Datos** | MySQL Server 8.0 & Workbench (`productosdb`) |



## Estructura del Proyecto
* `Conexion.cs`: Gestión de conexiones y consultas parametrizadas
* `Producto.cs`: Modelo de entidad con soporte para bytes de imagen
* `Form1.cs`: Interfaz visual y lógica CRUD con validaciones

## Instalación y Ejecución
1. Clonar: `git clone https://github.com`
2. Configurar la base de datos ejecutando el esquema `productosdb` con la tabla `productos`.
3. Actualizar la cadena de conexión en `Conexion.cs` y ejecutar con **F5**.

## Resultados
* **Conexión y Consultas:** Uso de parámetros seguros para evitar Inyección SQL
* **Persistencia Binaria:** Conversión de imágenes a `byte[]` mediante `MemoryStream` y `Bitmap`.
* **Operaciones CRUD:** Validación de tipos de datos y sincronización en tiempo real con `DataGridView`.

## Fecha de Ejecución
24 de Septiembre de 2026

| Datos del Estudiante | Detalles |
| :--- | :--- |
| **Nombre** | Greisy Coronado |
| **Correo** | greisy.coronado@utp.ac.pa |
| **Curso** | Herramientas de Programación Aplicada III |
| **Instructor** | Ing. Irina Fong |
