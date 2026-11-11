# Comercio Multibodega

Tres proyectos independientes en carpetas separadas, como en ComunicacionSincronicaLibreria.

| Proyecto | Solución | Base de datos | HTTPS local |
|---|---|---|---|
| Catálogo | Catalogo/Catalogo.slnx | CatalogoDB | https://localhost:7264 |
| Inventario | Inventario/Inventario.slnx | InventarioDB | https://localhost:7056 |
| Pedidos | Pedidos/Pedidos.slnx | PedidosDB | https://localhost:7205 |

Cada solución abre una sola API. Configurar SQL Server local y ejecutar las migraciones de cada proyecto. No hay llamadas entre las APIs; IdProducto en Inventario y Pedidos es una referencia lógica.

Las solicitudes de Postman se organizan directamente en la aplicación por servicio y entidad.
