# AirTicketSystem (C# / .NET Console)

Sistema académico de **gestión de tiquetes aéreos** desarrollado como **aplicación de consola** con **.NET + EF Core + MySQL**, UI con **Spectre.Console** y una estructura basada en **arquitectura hexagonal (Ports & Adapters)**.

## Tecnologías

- **.NET** (Consola)
- **Entity Framework Core** (migraciones y persistencia)
- **MySQL**
- **Spectre.Console** (menús, tablas, formularios)

## Arquitectura (Hexagonal)

El proyecto está organizado por módulos bajo `src/modules/*` y cada módulo mantiene separación de responsabilidades:

- **Domain**: agregados + value objects + puertos (`IRepository`).
- **Application**: casos de uso (orquestación) que dependen solo de Domain.
- **Infrastructure**: adaptadores (EF Core) que implementan repositorios del Domain y mapean entidad ↔ agregado.
- **UI**: menús y flujos de consola (Portal Administrativo / Portal Clientes).
- **shared**: `DbContext`, DI, helpers y componentes UI reutilizables.

Reglas clave que se respetan:

- **Application/UseCases** solo importa Domain (agregado + repositorio).
- **Domain/Repositories** trabajan con agregados (no entities EF).
- **Infrastructure/Repository** implementa puertos del Domain y usa `MapToDomain()` / `MapToEntity()` privados.
- Agregados con `Crear()`, `Reconstituir()` y `EstablecerId()`.
- `CancellationToken` en casos de uso.
- **No se modifican** `entities` ni `configurations` (modelo persistente).

## Requisitos previos

- **MySQL** en ejecución
- Base de datos creada o permisos para crearla
- .NET SDK instalado

## Configuración

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=AirTicketSystemDb;User=paula;Password=paula;"
  }
}
```

## Migraciones y base de datos

El `Program.cs` intenta:

- Conectarse a la base de datos.
- Aplicar automáticamente migraciones pendientes (`MigrateAsync()`).

Si prefieres manejar migraciones manualmente, puedes usar EF Core Tools (según tu entorno):

```bash
dotnet ef migrations add "NombreMigracion"
dotnet ef database update
```

## Ejecución

```bash
dotnet run
```

## Seeds (datos iniciales)

Al iniciar, `Program.cs` ejecuta:

- **SeedAdminUseCase**: garantiza rol `ADMIN` / `CLIENTE` y crea un usuario administrador si no existe.
- **SeedFullDataUseCase**: carga datos de prueba (vuelos, reservas, pagos, etc.). Si falla, el sistema continúa sin datos precargados.

### Credenciales admin iniciales

El seed crea:

- **Usuario**: `admin`
- **Contraseña**: `admin123`

Archivo: `src/modules/user/Application/UseCases/SeedAdminUseCase.cs`.

## Portales y navegación (alto nivel)

El sistema ofrece dos portales:

- **Portal Administrativo**: CRUD amplio del sistema por fines académicos.
- **Portal de Clientes**: búsqueda/compra, reservas, check-in, equipaje y perfil.

En el portal de clientes, los flujos críticos están diseñados con selección interactiva (sin pedir códigos/IDs de memoria) cuando aplica:

- Selección de reservas desde lista.
- Selección de pasajeros por reserva.
- Consulta de tiquete / pase de abordar por selección.

## Buenas prácticas de UI (Spectre.Console)

Para evitar errores de markup (por ejemplo strings con `[` `]`), se centraliza el escape de texto dinámico en `SpectreHelper`.

## Estructura del repositorio

```text
AirTicketSystem/
  Program.cs
  appsettings.json
  Migrations/
  src/
    modules/
      <module>/
        Domain/
        Application/
        Infrastructure/
    shared/
      context/
      UI/
      helpers/
```

## Ejemplos de prompts usados

### ESPECIFICACIÓN COMPLETA DEL SISTEMA

1. AUTENTICACIÓN Y TIPOS DE USUARIO

El sistema debe implementar obligatoriamente:

Registro de usuario

Se deben solicitar y almacenar en base de datos (según el modelo ya definido):

* Usuario
* Contraseña
* Nombres
* Apellidos
* Tipo de documento
* Número de documento
* Fecha de nacimiento
* Género
* Nacionalidad
* Emails
* Teléfonos

Comportamiento del registro

* Todo usuario que se registre desde la aplicación será automáticamente:
    * Tipo: CLIENTE
    * Tendrá acceso únicamente al Portal de Clientes

Tipos de usuario existentes

1. Cliente
    * Se registra desde la aplicación
    * Accede al Portal de Clientes
2. Administrativo
    * NO se puede registrar desde la aplicación
    * Solo puede ser creado desde:
        * El sistema (código inicial / seed)
        * O desde el Portal Administrativo por otro administrador
    * Accede al Portal Administrativo

Requisito importante

Debe existir un usuario inicial con rol ADMINISTRADOR creado desde código para poder gestionar el sistema.

2. REGLAS GENERALES DE CRUD

* Un usuario administrativo puede hacer:
    * Crear
    * Leer
    * Modificar
    * Eliminar
* Esto aplica a casi todas las entidades.

Excepciones:

* Tablas intermedias (relaciones muchos a muchos)
* Tablas que se generan automáticamente

3. CAPACIDADES DEL USUARIO ADMINISTRATIVO

El usuario administrativo tiene acceso total al sistema (por fines académicos).

Puede gestionar (CRUD completo) las siguientes entidades:

* Tipos de direcciones
* Aviones
* Fabricantes de aviones
* Modelos de aviones
* Asientos de avión
* Aerolíneas
* Aeropuertos
* Reservas
* Check-in
* Ciudades
* Clientes
* Relación de contacto
* Continentes
* Países
* Departamentos
* Regiones
* Tipos de documento
* Tipos de email
* Tarifas
* Vuelos
* Tripulación
* Terminales
* Géneros
* Recibos / Facturas
* Equipajes
* Restricciones de equipaje
* Tipos de equipaje
* Pagos
* Métodos de pago
* Personas
* Tipos de teléfono
* Licencias de piloto
* Pilotos
* Rutas
* Asientos
* Clases de servicio
* Especialidades
* Tiquetes
* Usuarios
* Trabajadores
* Tipos de trabajadores

Ejemplos de acciones reales:

* Crear usuarios (cliente o administrativo)
* Crear/modificar vuelos
* Gestionar reservas
* Crear tripulación y asignarla a vuelos
* Crear pilotos con licencias
* Administrar clientes y sus datos

4. CAPACIDADES DEL USUARIO CLIENTE

El cliente puede:

Autenticación

* Registrarse
* Iniciar sesión

Vuelos

* Buscar vuelos con:
    * Origen
    * Destino
    * Fecha
    * Aerolínea (opcional)
* Ver resultados:
    * Precios por clase
    * Disponibilidad

Reservas

* Crear reservas
* Ver reservas
* Modificar ciertos datos
* Cancelar reservas

Compra

* Seleccionar:
    * Vuelo
    * Clase de servicio
    * Asiento (opcional, con costo adicional)
    * Aerolínea
* Registrar pasajeros
* Agregar equipaje
* Pagar

Post-compra

* Ver:
    * Detalles del vuelo
    * Aerolínea
    * Ruta
    * Avión
    * Terminal
    * Puerta de embarque
    * Tiquete

Check-in

* Hacer check-in
* Generar pase de abordar

Equipaje

* Ver equipaje
* Agregar equipaje adicional (con costo)
* Ver restricciones

Perfil

* Ver y modificar datos personales
* Gestionar:
    * Teléfonos
    * Emails
    * Direcciones
    * Contactos de emergencia

5. CONSIDERACIONES IMPORTANTES

* Aunque en la vida real los roles están separados, aquí:
    * El administrativo tiene acceso total
    * Esto es intencional por fines académicos

6. ORGANIZACIÓN DE MENÚS

Se deben implementar dos portales completamente separados:

Portal Administrativo

Estructura de menús — Portal Administrativo


╔══════════════════════════════════════════╗
║         PORTAL ADMINISTRATIVO            ║
╠══════════════════════════════════════════╣
║  1. Gestión Geográfica                   ║
║  2. Gestión de Aerolíneas y Flota        ║
║  3. Gestión de Personal                  ║
║  4. Gestión de Vuelos                    ║
║  5. Gestión de Clientes y Usuarios       ║
║  6. Gestión de Reservas y Pagos          ║
║  7. Gestión de Catálogos                 ║
║  8. Reportes LINQ                        ║
║  9. Cerrar sesión                        ║
╚══════════════════════════════════════════╝

SUBMENÚS:

1. GESTIÓN GEOGRÁFICA
   ├── 1.1 Continentes    (CRUD)
   ├── 1.2 Países         (CRUD)
   ├── 1.3 Regiones       (CRUD)
   ├── 1.4 Departamentos  (CRUD)
   └── 1.5 Ciudades       (CRUD)

2. GESTIÓN DE AEROLÍNEAS Y FLOTA
   ├── 2.1 Aerolíneas          (CRUD + teléfonos + emails)
   ├── 2.2 Aeropuertos         (CRUD)
   ├── 2.3 Terminales          (CRUD)
   ├── 2.4 Puertas de embarque (CRUD)
   ├── 2.5 Rutas               (CRUD)
   ├── 2.6 Fabricantes avión   (CRUD)
   ├── 2.7 Modelos de avión    (CRUD)
   ├── 2.8 Aviones             (CRUD)
   ├── 2.9 Asientos de avión   (CRUD)
   └── 2.10 Clases de servicio (CRUD)

3. GESTIÓN DE PERSONAL
   ├── 3.1 Tipos de trabajador (CRUD)
   ├── 3.2 Especialidades      (CRUD)
   ├── 3.3 Trabajadores        (CRUD)
   ├── 3.4 Licencias de piloto (CRUD)
   └── 3.5 Habilitaciones      (CRUD)

4. GESTIÓN DE VUELOS
   ├── 4.1 Vuelos              (CRUD + cambio estado)
   ├── 4.2 Tripulación         (asignar/remover por vuelo)
   ├── 4.3 Disponibilidad      (ver asientos por vuelo)
   └── 4.4 Historial de vuelos (READ)

5. GESTIÓN DE CLIENTES Y USUARIOS
   ├── 5.1 Clientes            (CRUD + ver reservas)
   ├── 5.2 Usuarios            (CRUD + cambiar rol)
   ├── 5.3 Personas            (CRUD)
   └── 5.4 Log de accesos      (READ)

6. GESTIÓN DE RESERVAS Y PAGOS
   ├── 6.1 Reservas            (CRUD + cambiar estado)
   ├── 6.2 Tiquetes            (CRUD + emitir + anular)
   ├── 6.3 Check-in            (CRUD + presencial)
   ├── 6.4 Equipaje            (CRUD + registrar en checkin)
   ├── 6.5 Pagos               (CRUD + aprobar/rechazar)
   ├── 6.6 Cargos adicionales  (CRUD)
   └── 6.7 Facturas            (CRUD + generar)

7. GESTIÓN DE CATÁLOGOS
   ├── 7.1 Géneros             (CRUD)
   ├── 7.2 Tipos de documento  (CRUD)
   ├── 7.3 Tipos de teléfono   (CRUD)
   ├── 7.4 Tipos de email      (CRUD)
   ├── 7.5 Tipos de dirección  (CRUD)
   ├── 7.6 Relaciones contacto (CRUD)
   ├── 7.7 Tipos de equipaje   (CRUD)
   ├── 7.8 Restricciones equip (CRUD)
   ├── 7.9 Métodos de pago     (CRUD)
   └── 7.10 Tarifas            (CRUD)

8. REPORTES LINQ
   ├── 8.1 Vuelos con mayor ocupación
   ├── 8.2 Vuelos con asientos disponibles
   ├── 8.3 Clientes con más reservas
   ├── 8.4 Destinos más solicitados
   ├── 8.5 Reservas por estado
   ├── 8.6 Ingresos por aerolínea
   └── 8.7 Tiquetes por rango de fechas

Portal Cliente

Estructura de menús — Portal Cliente

╔══════════════════════════════════════════╗
║           PORTAL DE CLIENTES             ║
╠══════════════════════════════════════════╣
║  1. Buscar y reservar vuelos             ║
║  2. Mis reservas                         ║
║  3. Mi perfil                            ║
║  4. Cerrar sesión                        ║
╚══════════════════════════════════════════╝

SUBMENÚS:

1. BUSCAR Y RESERVAR VUELOS
   ├── 1.1 Buscar vuelos
   │         → Seleccionar aerolínea (opcional)
   │         → Ingresar origen
   │         → Ingresar destino
   │         → Seleccionar fecha
   │         → Ver resultados con precio por clase
   │         → Seleccionar vuelo y clase de servicio
   │         → Seleccionar asiento (opcional, cobro adicional)
   │         → Ver resumen y confirmar
   │         → Registrar pasajeros
   │         → Agregar equipaje
   │         → Proceder al pago
   └── 1.2 Ver vuelos próximos disponibles

2. MIS RESERVAS
   ├── 2.1 Ver todas mis reservas
   ├── 2.2 Ver detalle de reserva
   │         → Datos del vuelo (aerolínea, ruta, fecha, avión)
   │         → Terminal y puerta de embarque
   │         → Clase de servicio
   │         → Pasajeros y asientos
   │         → Estado de pago
   ├── 2.3 Cancelar reserva
   ├── 2.4 Hacer check-in virtual
   │         → Solo si el vuelo está en ventana de check-in
   │         → Confirma/reasigna asiento
   │         → Genera pase de abordar
   ├── 2.5 Ver mi pase de abordar
   │         → Muestra el pase formateado en consola
   │         → Código QR en texto
   ├── 2.6 Gestionar equipaje
   │         → Ver equipaje registrado
   │         → Agregar equipaje adicional
   │         → Ver restricciones y costos
   └── 2.7 Ver mis tiquetes

3. MI PERFIL
   ├── 3.1 Ver mis datos personales
   ├── 3.2 Modificar mis datos
   ├── 3.3 Gestionar mis teléfonos
   ├── 3.4 Gestionar mis emails
   ├── 3.5 Gestionar mis direcciones
   └── 3.6 Gestionar contactos de emergencia

8. PLAN DE DESARROLLO (OBLIGATORIO)

Se debe definir una ruta basada en:

* Arquitectura hexagonal 
* Trata de dividir todo el trabajo en 8 bloques para poder ir monitoreando el trabajo y para que no se haga todo en una sola tarea
* Separación por capas:
    * Dominio
    * Aplicación
    * Infraestructura
    * UI
    * Carpeta shared con contracts, context, helpers, ui

9. REGLAS DE DEPENDENCIA (MUY IMPORTANTE)

El sistema debe respetar lógica del mundo real:

Ejemplos:

* No se puede crear un vuelo sin:
    * Avión
    * Ruta
    * Aerolínea
    * Aeropuerto
* No se pueden vender asientos si:
    * No se ha definido el avión
    * No se conocen los asientos del avión
* No hay reservas sin vuelos
* No hay check-in sin reserva

Todo debe respetar:

* Modelo de datos
* Lógica real del negocio

10. INTERFAZ

* Se debe usar la librería:
    Spectre.Console

Para:

* Menús interactivos
* Tablas
* Formularios
* Navegación clara

11. REGLAS FINALES

* No omitir funcionalidades
* No simplificar lógica
* No ignorar dependencias
* No cambiar estructura de menús
* Respetar TODO lo definido
* Mantener coherencia con base de datos
* Mantener lógica del mundo real
* Seguir arquitectura del proyecto

VALIDACIÓN COMPLETA DE LA ARQUITECTURA DEL PROYECTO

Objetivo

Se requiere realizar una revisión exhaustiva, detallada y crítica de todos los módulos del proyecto con el fin de garantizar que la base del sistema esté completamente sólida antes de avanzar hacia la implementación de la interfaz (UI), menús y la integración total del sistema.

Alcance de la revisión

* Se deben revisar TODOS los módulos del proyecto (48 en total).
* Cada módulo debe analizarse en sus tres capas:
    * Domain
    * Application
    * Infrastructure

Restricción CRÍTICA

Corrección de inconsistencias

* En caso de que durante la revisión se detecten:
    * Inconsistencias
    * Errores de diseño
    * Problemas de coherencia entre capas o módulos
* Se deben corregir obligatoriamente

Condiciones para realizar correcciones:

* Las correcciones deben:
    * Mantener la coherencia con el modelo de negocio definido
    * Respetar la arquitectura hexagonal
    * No generar nuevos acoplamientos indebidos
    * No romper la integración entre módulos
* NO se deben modificar bajo ninguna circunstancia:
    * Las entidades (Entities)
    * Las configuraciones de base de datos. 
    * Esto es obligatorio porque:
        * El modelo de datos ya está completamente definido desde el inicio
        * No debe alterarse la estructura persistente del sistema
* Las correcciones deben enfocarse en:
    * Lógica de aplicación
    * Casos de uso
    * Servicios de dominio (si aplica)
    * Adaptadores en infraestructura
    * Manejo de dependencias

Criterios de validación

Cada módulo debe ser evaluado bajo los siguientes criterios:

1. Coherencia interna del módulo

* Las capas Domain, Application e Infrastructure deben:
    * Estar correctamente separadas
    * Cumplir su responsabilidad dentro de la arquitectura
    * No tener dependencias indebidas entre capas

2. Coherencia entre módulos

* Todos los módulos deben:
    * Integrarse correctamente entre sí
    * No generar inconsistencias en relaciones
    * Mantener una comunicación clara y estructurada

3. Coherencia con el modelo de negocio

* El comportamiento del código debe reflejar fielmente:
    * Las reglas definidas previamente
    * Los flujos del sistema (clientes, reservas, vuelos, etc.)
    * La lógica del mundo real que se ha especificado desde el inicio

4. Coherencia con la arquitectura hexagonal

* Se debe validar que el proyecto respete correctamente:
    * Separación de responsabilidades
    * Uso adecuado de puertos y adaptadores
    * Independencia del dominio respecto a infraestructura
* Verificar que:
    * Domain NO depende de Infrastructure
    * Application orquesta correctamente los casos de uso
    * Infrastructure implementa adaptadores sin romper la arquitectura

Validación de integridad general

Se debe asegurar que:

* No existan:
    * Inconsistencias lógicas
    * Dependencias incorrectas
    * Acoplamientos innecesarios
    * Violaciones de la arquitectura
* Todo el sistema funcione como una unidad coherente y alineada

Importancia de esta fase

Esta validación es crítica y obligatoria porque:

* Es el paso previo a:
    * Implementación de UI
    * Construcción de menús
    * Integración total del sistema
* Si esta base no está bien:
    * Todo lo que se construya después será inestable
    * Se generarán errores difíciles de corregir

Resultado esperado

Al finalizar esta revisión:

* Todos los módulos deben estar:
    * Correctamente estructurados
    * Coherentes entre sí
    * Alineados con el negocio
    * Cumpliendo la arquitectura hexagonal
* El sistema debe estar listo para pasar a la fase de UI e integración final
 # Rol                      
  Sos un desarrollador de software senior especializado en C#, .NET y           
  arquitectura hexagonal (Ports & Adapters), con más de 10 años de experiencia
  en desarrollo backend y diseño de sistemas escalables. Tenés amplio dominio   
  en bases de datos relacionales (SQL Server, PostgreSQL), Entity Framework     
  Core, migraciones, y buenas prácticas de diseño (SOLID, Clean Architecture,   
  DDD).
  Además, sos un excelente mentor técnico, capaz de explicar conceptos
  complejos de forma clara, progresiva y didáctica, adaptándote a un
  desarrollador de nivel junior-avanzado.
  # Tarea
  Tu tarea es acompañarme como asistente técnico durante el desarrollo
  completo de un proyecto en C#. Debés ayudarme a:
  - Diseñar correctamente la arquitectura hexagonal del proyecto.
  - Definir la estructura de carpetas y responsabilidades de cada capa.
  - Implementar conexión a base de datos utilizando Entity Framework Core.
  - Configurar y ejecutar migraciones correctamente.
  - Desarrollar operaciones CRUD completas y funcionales.
  - Aplicar buenas prácticas de desarrollo (SOLID, separación de
  responsabilidades, inyección de dependencias).
  - Resolver errores, bugs o problemas técnicos que surjan durante el
  desarrollo.
  - Explicar cada decisión técnica de forma clara y justificada.
  # Detalles Específicos
  - Siempre explicá el "por qué" de cada decisión, no solo el "cómo".
  - Cuando propongas código, debe estar completo, bien estructurado y listo
  para usar.
  - Usá ejemplos concretos aplicados al proyecto.
  - Si hay múltiples formas de hacer algo, explicá las opciones y recomendá la
  mejor práctica.
  - Asegurate de respetar la arquitectura hexagonal:
    - Dominio independiente de frameworks.
    - Aplicación como orquestador de casos de uso.
    - Infraestructura como implementación de detalles técnicos.
  - Indicá claramente en qué carpeta o capa va cada archivo.
  - Ayudá a interpretar correctamente los requisitos del PDF del proyecto.
  - Detectá posibles errores de diseño antes de que ocurran.
  - Guiá paso a paso cuando sea necesario.
  # Contexto
  Estoy desarrollando un proyecto académico para la materia de C# dentro de mi
  formación como desarrollador de software. Tengo un nivel junior-avanzado y
  el proyecto exige:
  - Persistencia de datos
  - Conexión a base de datos
  - Uso de migraciones
  - Implementación de CRUD funcional
  - Aplicación de arquitectura hexagonal
  El proyecto está basado en un documento PDF con requisitos específicos
  definidos por el profesor, y una estructura de carpetas que debo respetar.
  Este proyecto es importante para mi formación, por lo que necesito no solo
  resolverlo, sino entender profundamente cada parte.
  # Ejemplos
  ## Ejemplo 1
  **Pregunta del usuario:**
  "¿Dónde debería ir el repositorio en arquitectura hexagonal?"
  **Respuesta esperada:**
  Explicación clara de:
  - Qué es un repositorio en este contexto
  - Diferencia entre interfaz y implementación
  - En qué capa va cada uno (Dominio vs Infraestructura)
  - Ejemplo de código en C#
  ## Ejemplo 2
  **Pregunta del usuario:**
  "No me funciona la migración en Entity Framework"
  **Respuesta esperada:**
  - Diagnóstico paso a paso
  - Posibles causas
  - Solución detallada
  - Comandos correctos
  - Explicación del proceso de migraciones
  ## Ejemplo 3                                                                  
  **Pregunta del usuario:**                                                     
  "¿Cómo estructuro un CRUD con arquitectura hexagonal?"                        
  **Respuesta esperada:**                                                       
  - Separación por capas                                                        
  - Caso de uso (Application)                                                   
  - Entidad (Domain)                                                            
  - Repositorio (Puerto + Adaptador)                                            
  - Controlador (entrada)                                                       
  - Implementación completa con código                                          
  # Notas                                                                       
  - Nunca des respuestas vagas o superficiales.                                 
  - Siempre priorizá buenas prácticas profesionales.                            
  - Explicá como si estuvieras formando a un desarrollador para el mundo        
  laboral.                                                                      
  - Si detectás malas decisiones, corregilas y explicá por qué.                 
  - Mantené consistencia con arquitectura hexagonal en todo momento.            
  - Pensá siempre en escalabilidad y mantenibilidad del proyecto.               
                                                                                
  Hasta que no te de mas contexto, no quiero que realices ninguna accion sobre  

```

