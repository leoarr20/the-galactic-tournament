# The Galactic Tournament

The Galactic Tournament es una aplicación web full-stack donde se pueden registrar especies, enfrentarlas en combates y visualizar un ranking basado en la cantidad de victorias.

Cada 500 años, las especies más poderosas del universo compiten por la supremacía galáctica.

El proyecto está desarrollado con **ASP.NET Core Web API**, **Entity Framework Core**, **SQL Server** y **Angular 10.2.2**.

---

## 1. Características principales

La aplicación cubre el flujo completo del torneo:

* Página de inicio con explicación del contexto y reglas.
* Registro de especies.
* Listado de especies registradas.
* Edición de especies.
* Eliminación de especies.
* Validaciones de formulario.
* Filtros por columna en la tabla de especies.
* Ordenamiento de columnas.
* Inicio de combates entre dos especies seleccionadas.
* Inicio de combates aleatorios.
* Historial de combates.
* Explicación del resultado del combate.
* Ranking basado en victorias.
* Reinicio del ranking.
* Interfaz disponible en inglés y español.

---

## 2. Validaciones aplicadas

### Especies

Cada especie tiene:

* Nombre.
* Nivel de poder.
* Habilidad especial.

Reglas aplicadas:

* El nombre de la especie es obligatorio.
* El nombre debe ser único.
* El nombre solo permite letras.
* El nombre no permite espacios, números ni caracteres especiales.
* El nombre tiene un máximo de 100 caracteres.
* El nivel de poder es obligatorio.
* El nivel de poder solo permite números enteros positivos.
* El nivel de poder tiene un máximo de 10 dígitos en el frontend.
* La habilidad especial es obligatoria.
* La habilidad especial solo permite letras y espacios.
* La habilidad especial no permite números ni caracteres especiales.
* La habilidad especial tiene un máximo de 100 caracteres.

Estas validaciones existen tanto en el frontend como en el backend.

El frontend mejora la experiencia del usuario evitando entradas inválidas.
El backend mantiene las reglas reales del negocio y protege la API frente a peticiones externas.

---

## 3. Reglas del torneo

El ganador de un combate se determina en el backend usando las siguientes reglas:

1. Gana la especie con mayor nivel de poder.
2. Si ambas especies tienen el mismo nivel de poder, gana la especie cuyo nombre aparezca primero alfabéticamente.
3. Cada resultado de combate se almacena en el historial.
4. El ranking se calcula a partir de la cantidad de victorias registradas en el historial de combates.

El frontend no calcula el ganador.
Esto es intencional, porque la regla de combate es lógica de negocio y debe tener una única fuente de verdad en la API.

---

## 4. Tecnologías utilizadas

### Backend

* ASP.NET Core Web API.
* .NET 8.
* Entity Framework Core 8.
* SQL Server.
* Swagger / OpenAPI.
* Arquitectura por capas:

  * Controllers.
  * Services.
  * DTOs.
  * Entities.
  * DbContext.

### Frontend

* Angular 10.2.2.
* TypeScript.
* SCSS.
* ngx-translate.
* Angular clásico con:

  * AppModule.
  * AppRoutingModule.
  * Componentes.
  * Servicios.
  * Modelos tipados.

---

## 5. Justificación técnica

### ASP.NET Core Web API

Se utilizó ASP.NET Core Web API para construir una API REST clara, mantenible y separada del frontend.

Los controllers reciben las peticiones HTTP, delegan la lógica a los servicios y retornan respuestas adecuadas como:

* `200 OK`
* `201 Created`
* `204 No Content`
* `400 Bad Request`
* `404 Not Found`
* `409 Conflict`

### Entity Framework Core

Entity Framework Core se usó como ORM para manejar el acceso a SQL Server mediante entidades, relaciones y migraciones.

Se configuraron reglas como:

* Campos obligatorios.
* Longitudes máximas.
* Relaciones entre especies y combates.
* Restricciones de eliminación.
* Índice único para evitar nombres duplicados de especies.

### SQL Server

SQL Server se utilizó como base de datos relacional para almacenar:

* Especies.
* Combates.
* Historial de resultados.
* Información necesaria para calcular el ranking.

### Angular 10.2.2

Angular 10.2.2 fue seleccionado para mantener una arquitectura tradicional de Angular empresarial.

Esta versión trabaja con:

* `AppModule`.
* `AppRoutingModule`.
* Componentes declarados en módulos.
* Servicios inyectables.
* Formularios con `ngModel`.
* Arquitectura clara y fácil de revisar.

No se usaron características modernas como standalone components, signals, `inject()`, `@if` o `@for`, porque no corresponden a Angular 10.

### Servicios en Angular

El acceso HTTP está centralizado en servicios:

* `SpeciesService`
* `BattleService`
* `RankingService`

Esto evita duplicar URLs de API dentro de los componentes y mantiene el código más ordenado.

### Modelos tipados

Se crearon interfaces en `src/app/models` para representar los contratos entre Angular y la API.

Esto mejora la mantenibilidad y reduce errores al consumir respuestas del backend.

### SCSS

Se utilizó SCSS para organizar mejor los estilos visuales de la aplicación.

La interfaz usa una temática galáctica con:

* Encabezados oscuros.
* Tonos morados.
* Tarjetas.
* Gradientes.
* Estados hover.
* Botones diferenciados.
* Tablas con filtros y ordenamiento.

### Internacionalización

Se utilizó `ngx-translate` para permitir el cambio de idioma entre inglés y español.

Los textos traducibles se encuentran en:

```txt
frontend/the-galactic-tournament/src/assets/i18n/en.json
frontend/the-galactic-tournament/src/assets/i18n/es.json
```

---

## 6. Estructura general del proyecto

```txt
theGalacticTournament/
├── backend/
│   └── TheGalacticTournament.Api/
│       ├── Controllers/
│       ├── Data/
│       ├── DTOs/
│       ├── Entities/
│       ├── Migrations/
│       ├── Services/
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── Program.cs
│       └── TheGalacticTournament.Api.csproj
│
├── frontend/
│   └── the-galactic-tournament/
│       ├── src/
│       │   ├── app/
│       │   │   ├── components/
│       │   │   │   ├── home/
│       │   │   │   ├── species-form/
│       │   │   │   ├── species-list/
│       │   │   │   ├── battle-form/
│       │   │   │   └── ranking/
│       │   │   ├── models/
│       │   │   ├── services/
│       │   │   ├── app-routing.module.ts
│       │   │   ├── app.component.html
│       │   │   ├── app.component.scss
│       │   │   ├── app.component.ts
│       │   │   └── app.module.ts
│       │   ├── assets/
│       │   │   └── i18n/
│       │   └── environments/
│       ├── angular.json
│       ├── package.json
│       └── tsconfig.json
│
├── README.md
└── .gitignore
```

---

## 7. Prerrequisitos

Antes de ejecutar la aplicación, asegúrate de tener instaladas las siguientes herramientas.

### Backend

* .NET 8 SDK.
* SQL Server.
* SQL Server Management Studio o algún cliente para SQL Server.
* Entity Framework Core CLI.

Instalar Entity Framework Core CLI:

```powershell
dotnet tool install --global dotnet-ef --version 8.0.0
```

Verificar instalación:

```powershell
dotnet --version
dotnet ef --version
```

### Frontend

Este proyecto utiliza Angular 10.2.2.

Versiones recomendadas:

```txt
Node.js: 12.x
npm: 6.x
Angular CLI: 10.2.2
```

Instalar Angular CLI 10:

```powershell
npm install -g @angular/cli@10.2.2
```

Verificar instalación:

```powershell
node -v
npm -v
ng version
```

---

## 8. Configuración de base de datos

El backend utiliza SQL Server.

Abre el archivo de configuración de desarrollo:

```txt
backend/TheGalacticTournament.Api/appsettings.Development.json
```

Ejemplo para SQL Server local usando autenticación de Windows:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TheGalacticTournamentDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:4200"
    ]
  },
  "AllowedHosts": "*"
}
```

Ejemplo para SQL Server usando usuario y contraseña:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TheGalacticTournamentDb;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:4200"
    ]
  },
  "AllowedHosts": "*"
}
```

Por seguridad, no se deben subir credenciales reales de producción al repositorio.

---

## 9. Instalación del backend

Ubícate en la carpeta del backend:

```powershell
cd backend/TheGalacticTournament.Api
```

Restaura las dependencias:

```powershell
dotnet restore
```

Crea o actualiza la base de datos usando las migraciones de Entity Framework:

```powershell
dotnet ef database update
```

Ejecuta la API:

```powershell
dotnet run
```

La API debería ejecutarse en una URL similar a:

```txt
http://localhost:5154
```

Swagger debería estar disponible en:

```txt
http://localhost:5154/swagger
```

---

## 10. Configuración del frontend

Abre el archivo de ambiente del frontend:

```txt
frontend/the-galactic-tournament/src/environments/environment.ts
```

Verifica que la URL del backend esté configurada correctamente:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5154/api'
};
```

Si el backend se ejecuta en otro puerto, debes actualizar ese valor.

---

## 11. Instalación del frontend

Ubícate en la carpeta del frontend:

```powershell
cd frontend/the-galactic-tournament
```

Instala las dependencias:

```powershell
npm install
```

Ejecuta la aplicación Angular:

```powershell
ng serve
```

El frontend debería estar disponible en:

```txt
http://localhost:4200
```

---

## 12. Cómo ejecutar la aplicación completa

Abre dos terminales.

### Terminal 1: Backend

```powershell
cd backend/TheGalacticTournament.Api
dotnet run
```

Backend:

```txt
http://localhost:5154
```

### Terminal 2: Frontend

```powershell
cd frontend/the-galactic-tournament
ng serve
```

Frontend:

```txt
http://localhost:4200
```

Luego abre en el navegador:

```txt
http://localhost:4200
```

---

## 13. Endpoints principales

### Especies

```http
GET    /api/species
POST   /api/species
PUT    /api/species/{id}
DELETE /api/species/{id}
```

### Combates

```http
GET    /api/battles
POST   /api/battles
POST   /api/battles/random
```

### Ranking

```http
GET    /api/ranking
POST   /api/ranking/reset
```

---

## 14. Detalle de endpoints

### GET /api/species

Obtiene todas las especies registradas.

### POST /api/species

Crea una nueva especie.

Ejemplo:

```json
{
  "name": "Saiyan",
  "powerLevel": 9500,
  "specialAbility": "Super transformation"
}
```

### PUT /api/species/{id}

Actualiza una especie existente.

Ejemplo:

```json
{
  "name": "Namekian",
  "powerLevel": 8700,
  "specialAbility": "Regeneration"
}
```

### DELETE /api/species/{id}

Elimina una especie existente.

Una especie no debería eliminarse si ya tiene historial de combates, para mantener la trazabilidad del torneo.

### POST /api/battles

Inicia un combate entre dos especies seleccionadas.

Ejemplo:

```json
{
  "speciesAId": 1,
  "speciesBId": 2
}
```

### POST /api/battles/random

Inicia un combate aleatorio entre dos especies registradas.

### GET /api/ranking

Obtiene el ranking actual ordenado por cantidad de victorias.

### POST /api/ranking/reset

Reinicia el ranking eliminando el historial de combates.

---

## 15. Reinicio del ranking

El ranking no se guarda como un valor fijo en la tabla de especies.

El ranking se calcula a partir de la tabla de combates.

Por lo tanto, reiniciar el ranking significa eliminar todos los registros de combates, pero conservar las especies registradas.

Endpoint:

```http
POST /api/ranking/reset
```

---

## 16. Internacionalización

La aplicación permite cambiar el idioma de la interfaz entre:

```txt
Inglés
Español
```

Los textos traducibles se encuentran en:

```txt
frontend/the-galactic-tournament/src/assets/i18n/en.json
frontend/the-galactic-tournament/src/assets/i18n/es.json
```

El cambio de idioma se realiza desde la interfaz usando los botones:

```txt
EN
ES
```

---

## 17. Detalles de implementación aplicados

* Página Home agregada para explicar el torneo antes de solicitar datos al usuario.
* Navegación implementada con `routerLink` y `routerLinkActive`.
* API URLs centralizadas en `environment.ts`.
* Componentes separados por responsabilidad.
* Servicios Angular para comunicación con la API.
* Modelos tipados para representar los contratos con el backend.
* Validaciones en frontend y backend.
* Mensajes de error del API mostrados al usuario cuando corresponde.
* Resultado de combate con explicación del ganador.
* Ranking calculado desde el historial real de combates.
* Reinicio de ranking eliminando combates, no especies.
* Tabla de especies con filtros por campo.
* Tabla de especies con ordenamiento.
* Traducción de interfaz con `ngx-translate`.
* Archivos `.spec.ts` mantenidos como fueron generados inicialmente por Angular.

---

## 18. Flujo sugerido para demo

1. Abrir la página Home y explicar el contexto del torneo.
2. Cambiar entre idioma inglés y español usando los botones `EN` y `ES`.
3. Ir a la sección Species.
4. Registrar al menos tres especies.
5. Mostrar las validaciones del formulario.
6. Filtrar y ordenar la tabla de especies.
7. Editar una especie.
8. Eliminar una especie sin historial de combates.
9. Ir a la sección Battles.
10. Iniciar un combate seleccionado.
11. Iniciar un combate aleatorio.
12. Mostrar el historial de combates.
13. Ir a Ranking.
14. Explicar cómo las victorias modifican el orden del ranking.
15. Reiniciar el ranking y explicar que se eliminan los combates, no las especies.

---

## 19. Configuración para producción

Para producción se puede usar el archivo:

```txt
appsettings.Production.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR_SQL;Database=TheGalacticTournamentDb;User Id=TU_USUARIO;Password=TU_PASSWORD;Encrypt=True;TrustServerCertificate=False;"
  },
  "Cors": {
    "AllowedOrigins": [
      "https://the-galactic-tournament.com"
    ]
  },
  "AllowedHosts": "*"
}
```

En un ambiente real de producción, se recomienda configurar las credenciales usando variables de entorno, secretos del servidor, Azure App Settings, Docker secrets u otro proveedor seguro de configuración.

La variable de entorno para producción debe ser:

```txt
ASPNETCORE_ENVIRONMENT=Production
```

---

## 20. Notas importantes

* Verificar que SQL Server esté ejecutándose antes de correr las migraciones.
* Verificar que la URL del backend en Angular coincida con la URL real de la API.
* Si aparecen errores de CORS, revisar que el origen del frontend esté configurado en la sección `Cors` del backend.
* Si Angular muestra advertencias de TypeScript, no agregar `ignoreDeprecations` al `tsconfig.json`, porque Angular 10 usa una versión antigua de TypeScript.
* Para desarrollo local, `UseHttpsRedirection()` puede estar comentado si la API se ejecuta solo por HTTP.
* Para producción, se recomienda habilitar HTTPS.

---

## 21. Posibles mejoras futuras

* Agregar pruebas unitarias completas para servicios Angular usando `HttpClientTestingModule`.
* Agregar pruebas unitarias para servicios del backend.
* Agregar paginación al historial de combates.
* Agregar autenticación y autorización.
* Agregar Docker para levantar frontend, backend y SQL Server.
* Agregar pipeline CI/CD.
* Agregar logs estructurados.
* Agregar manejo global de errores en backend.
* Agregar confirmaciones visuales personalizadas en lugar de `confirm()` nativo del navegador.

---

## 23. Autor

Leonardo Romero

Proyecto desarrollado como prueba técnica para demostrar el uso de ASP.NET Core Web API, Entity Framework Core, SQL Server y Angular.
