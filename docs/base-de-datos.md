# Base de datos de Kobra: instalación, acceso y uso

Cada integrante del equipo tiene **su propia base de datos local**. Nadie comparte servidor ni datos: lo único que se comparte por git es el **script que crea la base** (`Database\01_CrearBaseDatos.sql`) y la cadena de conexión por defecto (`Kobra.UI\App.config`). Si tu base se daña, la puedes borrar y volver a crear en un minuto.

Esta guía sirve para tres cosas: instalar SQL Server, encontrar tu base y conectarte a ella, y resolver los errores más comunes.

## 1. Instalar SQL Server (una sola vez)

Instala **SQL Server Express** (gratuito) desde la página de descargas de Microsoft. En el instalador:

| Paso | Qué elegir |
| --- | --- |
| Tipo de instalación | **Básica** es suficiente. Con la **Personalizada** elige solo "Motor de base de datos" |
| Nombre de la instancia | **`SQLEXPRESS`** (es el valor por defecto de Express). La cadena de conexión del proyecto depende de este nombre |
| Modo de autenticación | **Autenticación de Windows**. El modo mixto no es necesario |
| Administradores del motor | **Agregar usuario actual.** Es el paso más importante: sin él tu usuario de Windows no podrá crear la base |

No necesitas Full-Text, Reporting Services, Machine Learning ni Integration Services.

Alternativa más ligera: **LocalDB**, que viene con Visual Studio. Su servidor se llama `(localdb)\MSSQLLocalDB`. Si la usas, cambia `Server=` en `App.config` (ver sección 4).

### Comprobar que está funcionando

En PowerShell:

```powershell
Get-Service 'MSSQL$SQLEXPRESS'
```

Debe decir `Running`. Si dice `Stopped`, inícialo con `Start-Service 'MSSQL$SQLEXPRESS'` (en una consola como administrador) o desde `services.msc`.

## 2. Cómo encontrar tu servidor

El servidor se escribe como **`equipo\instancia`**. Para una instalación normal de Express hay tres formas equivalentes de nombrarlo:

- `localhost\SQLEXPRESS` ← la que usa el proyecto
- `.\SQLEXPRESS`
- `NOMBRE-DE-TU-PC\SQLEXPRESS`

Para saber qué instancias tienes instaladas:

```powershell
Get-Service 'MSSQL*' | Select-Object Name, Status
```

- `MSSQL$SQLEXPRESS` significa instancia `SQLEXPRESS`.
- `MSSQLSERVER` significa instancia **predeterminada** (servidor: solo `localhost`).
- Cualquier otro `MSSQL$NOMBRE` significa instancia `NOMBRE`.

## 3. Crear la base de datos

Se hace con un script que se puede ejecutar todas las veces que haga falta sin perder datos. Si una ejecución anterior quedó a medias, volver a ejecutarlo completa lo que falte.

### Con `sqlcmd` (terminal)

`sqlcmd` viene con SQL Server, pero normalmente **no está en el PATH**. Busca su ruta con:

```powershell
(Get-ChildItem 'C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\*\Tools\Binn\SQLCMD.EXE').FullName
```

Desde la carpeta del proyecto (`kobra`), ejecuta (cambia la ruta por la que te dio el comando anterior):

```powershell
& 'C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\180\Tools\Binn\SQLCMD.EXE' -S 'localhost\SQLEXPRESS' -E -C -b -f 65001 -i Database\01_CrearBaseDatos.sql
```

Qué significa cada opción:

| Opción | Para qué sirve |
| --- | --- |
| `-S 'localhost\SQLEXPRESS'` | Servidor |
| `-E` | Usa tu usuario de Windows (sin contraseña) |
| `-C` | Confía en el certificado del servidor. **Es obligatorio**: sin esta opción falla con "La cadena de certificación fue emitida por una entidad en la que no se confía" |
| `-b` | Si hay un error, termina con código distinto de 0 |
| `-f 65001` | Lee el archivo como UTF-8 (para que las tildes no se dañen) |
| `-i` | Archivo de script |

Si todo salió bien, solo verás `Changed database context to 'Kobra'.` y ningún error.

### Con SSMS, Azure Data Studio o Visual Studio

Abre `Database\01_CrearBaseDatos.sql` desde el programa, conéctate a tu servidor y ejecútalo completo (F5 en SSMS).

## 4. Configurar la aplicación

La cadena de conexión está en `Kobra.UI\App.config`:

```xml
<add name="Kobra"
     connectionString="Server=localhost\SQLEXPRESS;Database=Kobra;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=5"
     providerName="Microsoft.Data.SqlClient" />
```

- Si instalaste **SQL Express con la instancia `SQLEXPRESS`**, no tienes que cambiar nada.
- Si tu servidor es otro, cambia solo `Server=` (por ejemplo `(localdb)\MSSQLLocalDB`, o `localhost` si usas la instancia predeterminada).
- `Integrated Security=True` usa tu usuario de Windows, así que no hay contraseñas en el proyecto. **Nunca escribas contraseñas en `App.config`.**

**Para no subir tu cambio por accidente** (cada integrante puede tener un servidor distinto), después de editar `App.config` ejecuta una vez:

```powershell
git update-index --skip-worktree Kobra.UI/App.config
```

Git deja de ver tus cambios en ese archivo. Para volver a verlos: `git update-index --no-skip-worktree Kobra.UI/App.config`. Si alguien cambia el `App.config` en el repositorio, tendrás que deshacerlo antes de hacer `git pull`.

## 5. Cómo interactuar con la base

Hay tres formas de ver las tablas y ejecutar consultas. Usa la que prefieras.

### A. Visual Studio (no hay que instalar nada más si tienes la carga de trabajo de datos)

1. Menú **Ver**, **Explorador de objetos de SQL Server** (SQL Server Object Explorer).
2. En "SQL Server", clic derecho, **Agregar SQL Server**.
3. Elige tu servidor (`localhost\SQLEXPRESS`) con **Autenticación de Windows**.
4. Abre **Bases de datos**, **Kobra**, **Tablas**. Clic derecho en una tabla, **Ver datos** o **Nueva consulta**.

Si no aparece el menú, abre el **Instalador de Visual Studio**, elige Modificar, y marca la carga de trabajo **Almacenamiento y procesamiento de datos**.

### B. SSMS (SQL Server Management Studio)

Es una descarga aparte. Al conectarte:

- **Nombre del servidor:** `localhost\SQLEXPRESS`
- **Autenticación:** Autenticación de Windows
- **Cifrado:** si SSMS muestra un error de certificado, marca **Confiar en el certificado del servidor** (Trust server certificate) en las opciones de conexión.

### C. `sqlcmd` en la terminal

Abre una sesión interactiva (se sale con `exit`):

```powershell
& 'C:\...\SQLCMD.EXE' -S 'localhost\SQLEXPRESS' -E -C -d Kobra -I
```

O ejecuta una sola consulta:

```powershell
& 'C:\...\SQLCMD.EXE' -S 'localhost\SQLEXPRESS' -E -C -d Kobra -I -Q "SELECT TOP 10 * FROM dbo.Cliente"
```

La opción `-I` activa `QUOTED_IDENTIFIER`. **Úsala siempre que escribas datos con `sqlcmd`**: sin ella, los `INSERT`, `UPDATE` y `DELETE` sobre `Cliente` fallan con el error 1934 por el índice único filtrado de la cédula/RUC. SSMS, Visual Studio y la aplicación no tienen este problema.

### Consultas útiles

```sql
-- Clientes
SELECT TOP 50 * FROM dbo.Cliente ORDER BY Nombre;

-- Saldo y estado de cada factura (fuente única; no recalcules a mano)
SELECT * FROM dbo.vw_FacturaSaldo ORDER BY FechaVencimiento;

-- Facturas vencidas con saldo
SELECT Numero, Saldo, DiasVencidos FROM dbo.vw_FacturaSaldo WHERE EstaVencida = 1;

-- Cuántas filas hay en cada tabla
SELECT (SELECT COUNT(*) FROM dbo.Cliente) AS Clientes,
       (SELECT COUNT(*) FROM dbo.Factura) AS Facturas,
       (SELECT COUNT(*) FROM dbo.Pago)    AS Pagos;
```

### Empezar de cero (borra todos tus datos locales)

```sql
USE master;
ALTER DATABASE Kobra SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE Kobra;
```

Después vuelve a ejecutar `Database\01_CrearBaseDatos.sql`. Esto solo afecta a **tu** base local.

## 6. Cambios futuros en la base

- El script `01_CrearBaseDatos.sql` **no se edita** una vez que el equipo ya lo ejecutó, salvo para corregir errores.
- Una tabla o columna nueva va en un script nuevo: `Database\02_<descripción>.sql`, `03_...`, y así. Cada integrante ejecuta los scripts nuevos, en orden, después de hacer `git pull`.
- Cada script debe poder ejecutarse más de una vez sin errores (usar `IF NOT EXISTS`, `IF OBJECT_ID(...) IS NULL`, `CREATE OR ALTER`).
- Los scripts empiezan con `SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON;` y los índices se crean con su propia comprobación `IF NOT EXISTS`, como en el `01`.

## 7. Problemas frecuentes

| Síntoma | Causa probable | Qué hacer |
| --- | --- | --- |
| La aplicación avisa "No se pudo conectar con la base de datos" | El servicio está detenido, o el nombre del servidor en `App.config` no es el tuyo | Revisa la sección 1 (servicio) y la sección 2 (nombre de la instancia) |
| `sqlcmd`: "La cadena de certificación fue emitida por una entidad en la que no se confía" | Falta la opción `-C` | Agrégala al comando |
| `Login failed for user` (error 18456) o "no tiene permiso para crear la base de datos" | Tu usuario de Windows no es administrador de SQL Server | Vuelve a ejecutar el instalador (Modificar o Agregar características) y agrega tu usuario como administrador del motor |
| "Cannot open database Kobra" (error 4060) | Aún no ejecutaste el script, o la base se llama distinto | Ejecuta `01_CrearBaseDatos.sql` (sección 3) |
| Error 1934: "CREATE INDEX failed because the following SET options have incorrect settings: 'QUOTED_IDENTIFIER'" | Usaste `sqlcmd` sin `-I` para escribir datos | Agrega `-I` al comando (sección 5-C) |
| `sqlcmd` no se reconoce como comando | No está en el PATH | Usa la ruta completa de la sección 3 |
| La aplicación abre pero la lista de clientes está vacía | Es normal en una base nueva | Crea un cliente con **Nuevo** y **Guardar** |
| Tildes o "ñ" aparecen como símbolos raros al ejecutar el script | Falta `-f 65001` | Agrégalo al comando |
