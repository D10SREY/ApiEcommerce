# fix: 403 en endpoints protegidos y validación de usuario único

**Rama sugerida:** `fix/roles-y-validacion-usuarios` → `main`
**Tipo:** 🐛 Bug fix

---

## 📋 Resumen

Este PR corrige tres cosas:

1. Los endpoints con `[Authorize(Roles = "Admin")]` respondían **403 Forbidden** aunque se usara un token válido.
2. `IsUniqueUser` buscaba usuarios en una tabla que ya no se usa.
3. La propiedad `Messege` del DTO de login tenía un error de tipeo.

---

## 🐛 Problema 1: 403 Forbidden con un token válido

### Síntoma
`GET /api/v1/Users` respondía `403 Forbidden` aunque se enviara un token obtenido en el login.

### Causa
Al decodificar el JWT se ve que el claim de rol tiene el valor **`"admin"`** (en minúscula):

```json
{ "id": "5b4aea64-...", "Username": "max-admin", "role": "admin" }
```

Pero los controladores pedían **`"Admin"`** (con mayúscula). En .NET, `IsInRole` **distingue mayúsculas de minúsculas**, así que `"admin"` y `"Admin"` cuentan como roles distintos.

El rol se guarda tal como llega en el JSON del registro (`createUserDto.Role`), así que quedó en la tabla `AspNetRoles` como `admin`.

> 💡 Un **401** quiere decir que el token es inválido o no existe. Un **403** quiere decir que el token es válido pero el usuario **no tiene permiso**. El 403 indicaba que el problema estaba en el rol, no en la configuración del JWT.

### Solución
Usar `"admin"` en minúscula en todos los controladores protegidos, igual que el rol guardado en la base de datos.

---

## 🐛 Problema 2: `IsUniqueUser` revisaba la tabla vieja

### Causa
Desde la migración `AddIdentitySupport`, los usuarios se guardan en `applicationUsers` (tabla `AspNetUsers` de Identity). Sin embargo, `IsUniqueUser` todavía buscaba en la tabla vieja `users`:

```csharp
// Antes
return !_db.users.Any(u => u.UserName.ToLower().Trim() == username.ToLower().Trim());
```

Por eso la validación "El usuario ya existe" del controlador **nunca encontraba a los usuarios nuevos**.

### Solución
```csharp
// Después
return !_db.applicationUsers.Any(u => u.UserName != null && u.UserName.ToLower().Trim() == username.ToLower().Trim());
```

Se agregó `u.UserName != null` porque en `ApplicationUser` (Identity) `UserName` puede ser null.

---

## ✏️ Cambio 3: renombrar `Messege` → `Message`

Error de tipeo en la propiedad del DTO de respuesta del login.

---

## 📁 Archivos modificados

| Archivo | Cambio |
|---|---|
| `Controllers/UsersController.cs` | `Roles = "Admin"` → `Roles = "admin"` |
| `Controllers/ProductsController.cs` | `Roles = "Admin"` → `Roles = "admin"` |
| `Controllers/V1/CategoriesController.cs` | `Roles = "Admin"` → `Roles = "admin"` |
| `Controllers/V2/CategoriesController.cs` | `Roles = "Admin"` → `Roles = "admin"` |
| `Repository/UserRepository.cs` | `IsUniqueUser` usa `applicationUsers`; `Messege` → `Message` (5 usos) |
| `Models/Dtos/UserLoginResponseDto.cs` | Propiedad `Messege` → `Message` |

No hay migraciones ni cambios en la base de datos.

---

## ⚠️ Breaking change

La respuesta de `POST /api/v1/Users/Login` cambia el nombre de un campo:

```diff
  {
    "user": { ... },
    "token": "eyJ...",
-   "messege": "Usuario logeado correctamente"
+   "message": "Usuario logeado correctamente"
  }
```

Cualquier cliente (Postman, frontend) que lea `messege` debe actualizarse.

---

## ✅ Cómo probar

1. `dotnet build` → **0 errores, 0 advertencias** ✔️
2. Ejecutar la API y abrir Swagger.
3. `POST /api/v1/Users/Login` con el usuario admin → copiar el `token` (comprobar que el campo ahora se llama `message`).
4. Hacer clic en **Authorize** y pegar el token.
5. `GET /api/v1/Users` → debe responder **200 OK** con la lista de usuarios.
6. `POST /api/v1/Users` con un `username` que ya existe → debe responder **400 "El usuario ya existe"**.

---

## 📝 Notas / pendientes

- **Los roles dependen de cómo se escriban al registrar.** Si alguien se registra con `"role": "Admin"`, se crea otro rol distinto y ese usuario recibirá 403. Mejora propuesta: aceptar solo valores fijos (`admin`, `user`) o guardar siempre el rol con el mismo formato en `Register`.
- **El token solo incluye un rol** (`roles.FirstOrDefault()`). Si un usuario tiene varios roles, los demás se pierden.
- La tabla vieja `users` (modelo `User`) ya no se usa en el flujo de Identity; se podría eliminar en un PR aparte.
