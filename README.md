# Examen final - Ingeniería de Software Ciclo I-2025

**Estudiante:** Nathalia Rodríguez Alfaro - C16530

# Sistema de Máquina Expendedora de Refrescos

Este repositorio incluye una **API backend** en .NET y una **aplicación frontend** en Vue.js que simulan el funcionamiento de una máquina expendedora de refrescos.

## Prerrequisitos

- Visual Studio 2022 o superior (con carga de .NET Desktop Development)
- .NET 6 o superior
- Node.js v14 o posterior y npm
- Visual Studio Code

## Instalación y ejecución

### Backend

1. Clonar el repositorio.
2. Abrir la solución `backend.sln` en Visual Studio.
3. Ejecutar la solución.  
   La API quedará disponible swagger.

### Frontend

1. Navegar a la carpeta `frontend/` en Visual Studio Code.
2. Ejecutar:
   ```bash
   npm install
   ```
3. Iniciar el servidor de desarrollo:
   ```bash
   npm run serve
   ```
4. Acceder a la aplicación en `http://localhost:8080`.

## Guía de uso

1. **Listar refrescos**  
   - Frontend: visualizar inventario y precios en la página principal.

2. **Seleccionar refrescos**  
   - Indicar cantidad para tipos deseados.  
   - El costo total se actualiza automáticamente.

3. **Ingresar dinero**  
   - Monedas de 500, 100, 50 y 25 colones.  
   - Billetes de 1000 colones.

4. **Realizar compra**  
   - Hacer clic en **Comprar**.  
   - La respuesta incluye `status`, `totalCost`, `totalPayment` y `changeAmount`.  
   - En caso de fondos insuficientes, se devuelve un mensaje de error.  
   - Si no es posible proveer cambio, el estado será `error` o `out_of_service`.

## Pruebas unitarias

### En Visual Studio

1. Abrir **Test Explorer**.  
2. Ejecutar todas las pruebas.
- `BuyBeverageCommandTest`
- `GetBeveragesQueryTest`

## Estructura del repositorio

```
/
├── backend/
│   ├── backend/              # Proyecto API en .NET
│   └── VendingMachineTests/  # Pruebas unitarias
└── frontend/                 # Aplicación Vue.js

```

> **Nota:** Todos los datos se gestionan en memoria, no hay base de datos.