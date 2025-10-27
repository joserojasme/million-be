# Million API - Bienes Raíces (.NET 8 + MongoDB)

API para gestión y consulta de propiedades inmobiliarias construida con ASP.NET Core (.NET 8) y MongoDB. La solución está organizada por capas (API, Application, DAL, DTO) y expone endpoints para consultar propiedades con filtros y explorar documentación vía Swagger.

## Contenido
- Descripción general
- Arquitectura del proyecto
- Requisitos
- Instalación y ejecución
- Configuración (MongoDB)
- Endpoints disponibles
- Ejemplos de uso (cURL)
- Estructuras de datos (DTOs)

## Descripción general
Esta API permite consultar propiedades almacenadas en MongoDB con la posibilidad de filtrar por nombre, dirección y rangos de precio. Las propiedades retornadas incluyen información relacionada: propietario, imágenes y trazas (histórico).

## Arquitectura del proyecto
- `Million.API`: capa de presentación (endpoints, Swagger, configuración de DI).
- `Million.Application`: servicios de aplicación (casos de uso). Ej.: `PropertyService`.
- `Million.Application.Interfaces`: contratos de la capa de aplicación. Ej.: `IPropertyService`.
- `Million.DAL`: acceso a datos (MongoDB) y repositorios. Ej.: `PropertyRepository`.
- `Million.DAL.Interfaces`: contratos de datos. Ej.: `IPropertyRepository`.
- `Million.DTO`: entidades/DTOs compartidas. Ej.: `Property`, `Owner`, `PropertyImage`, `PropertyTrace`.

## Requisitos
- .NET SDK 8.0+
- Acceso a una instancia de MongoDB (local o Atlas) (no hace falta restaurar backup pues en la cloud de mongo)

## Instalación y ejecución
Clona el repositorio y restaura dependencias:

```bash
cd a la raiz del proyecto y ejecuta
 dotnet restore               
 dotnet build
 dotnet run --project Million.API
```

Ejecuta la API (perfil Development recomendado):

Por defecto la API expone Swagger en Development. URL típica:
- Swagger UI: `https://localhost:5099/swagger` o `http://localhost:5000/swagger`

## Configuración (MongoDB)
La configuración vive en `Million.API/appsettings.json` y/o `appsettings.Development.json` bajo la sección `DatabaseSettings`:

```json
{
  "DatabaseSettings": {
    "ConnectionString": "<TU_CONNECTION_STRING>",
    "DatabaseName": "million",
    "PropertiesCollectionName": "Property",
    "OwnersCollectionName": "Owner",
    "PropertyImageCollectionName": "PropertyImage",
    "PropertyTraceCollectionName": "PropertyTrace"
  }
}
```

- `ConnectionString`: cadena de conexión de MongoDB (por ejemplo, Atlas).
- `DatabaseName`: nombre de la base de datos.
- Los nombres de colección pueden ajustarse según tu esquema.

En `Program.cs` se registra `IMongoClient` y `IMongoDatabase` usando estos valores y se configuran los repositorios/servicios.

## Endpoints disponibles
A continuación, los endpoints de la API detectados en el código:

### Propiedades
- GET `api/Property`
  - Parámetros de consulta (opcionales):
    - `name` (string): filtra por nombre (regex, case-insensitive)
    - `address` (string): filtra por dirección (regex, case-insensitive)
    - `minPrice` (decimal): precio mínimo
    - `maxPrice` (decimal): precio máximo
  - Respuesta: `200 OK` con lista de `Property` incluyendo relaciones (`Owner`, `PropertyImages`, `PropertyTraces`).

### Utilidad (plantilla)
- GET `/weatherforecast`
  - Endpoint de ejemplo generado por plantilla, útil para verificar que la API corre.

## Ejemplos de uso (cURL)
Reemplaza el puerto según tu ejecución local.

- Listar todas las propiedades:
```bash
curl -k "https://localhost:5001/api/Property"
```

- Filtrar por nombre (contiene "Loft") y precio máximo 300000:
```bash
curl -k "https://localhost:5001/api/Property?name=Loft&maxPrice=300000"
```

- Filtrar por dirección (contiene "Main St"), precio entre 100000 y 500000:
```bash
curl -k "https://localhost:5001/api/Property?address=Main%20St&minPrice=100000&maxPrice=500000"
```

- Probar endpoint de ejemplo:
```bash
curl -k "https://localhost:5001/weatherforecast"
```

### Ejemplo de respuesta de `GET api/Property`
```json
[
  {
    "id": "6640cbb47fd9a2f1d4b3e123",
    "name": "Loft céntrico",
    "address": "123 Main St",
    "price": 250000,
    "codeInternal": "INT-001",
    "year": 2018,
    "ownerId": "663ff0a97fd9a2f1d4b3e111",
    "owner": {
      "idOwner": "663ff0a97fd9a2f1d4b3e111",
      "name": "Jane Doe",
      "address": "456 Oak Ave",
      "photo": "https://example.com/jane.jpg",
      "birthday": "1985-06-15T00:00:00Z"
    },
    "propertyImages": [
      {
        "idPropertyImage": "6640d0f07fd9a2f1d4b3e222",
        "idProperty": "6640cbb47fd9a2f1d4b3e123",
        "file": "https://example.com/img1.jpg",
        "enabled": true
      }
    ],
    "propertyTraces": [
      {
        "idPropertyTrace": "6640d1a07fd9a2f1d4b3e333",
        "dateSale": "2024-01-10T00:00:00Z",
        "name": "Primera venta",
        "value": 240000,
        "tax": 12000,
        "idProperty": "6640cbb47fd9a2f1d4b3e123"
      }
    ]
  }
]
```

## Estructuras de datos (DTOs)
Campos relevantes según `Million.DTO/Entities`:

- `Property`
  - `id` (ObjectId string)
  - `name` (string)
  - `address` (string)
  - `price` (decimal)
  - `codeInternal` (string)
  - `year` (int)
  - `ownerId` (ObjectId string)
  - `owner` (objeto `Owner`, sólo en lectura)
  - `propertyImages` (lista `PropertyImage`, sólo en lectura)
  - `propertyTraces` (lista `PropertyTrace`, sólo en lectura)

- `Owner`
  - `idOwner` (ObjectId string)
  - `name` (string)
  - `address` (string)
  - `photo` (string)
  - `birthday` (DateTime)

- `PropertyImage`
  - `idPropertyImage` (ObjectId string)
  - `idProperty` (ObjectId string)
  - `file` (string)
  - `enabled` (bool)

- `PropertyTrace`
  - `idPropertyTrace` (ObjectId string)
  - `dateSale` (DateTime)
  - `name` (string)
  - `value` (decimal)
  - `tax` (decimal)
  - `idProperty` (ObjectId string)

## Notas
- La API actualmente expone lectura/consulta de propiedades. Crear/actualizar/eliminar no está implementado en el controlador mostrado.
- Swagger/OpenAPI está habilitado en entorno Development.
- Asegúrate de configurar correctamente `DatabaseSettings.ConnectionString` y que las colecciones existan con el esquema esperado.
