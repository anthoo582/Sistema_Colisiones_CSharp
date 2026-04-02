# Sistema de Colisiones en C# con MonoGame

Un proyecto de plataformas 2D con generación infinita de terreno, sistema de colisiones AABB y físicas realistas. Diseñado como demostración técnica para portafolio de desarrollo de videojuegos.

## Características

- **Sistema de Colisiones AABB**: Detección y resolución de colisiones Axis-Aligned Bounding Box
- **Físicas de Plataforma**: Gravedad, salto, aceleración y fricción realistas
- **Generación Infinita de Terreno**: Procedural con dificultad progresiva
- **Cámara Dinámica**: Seguimiento suave del jugador con sistema de viewport
- **Código Limpio**: Arquitectura modular y bien documentada

## Controles

| Tecla | Acción |
|-------|--------|
| A / ← | Mover izquierda |
| D / → | Mover derecha |
| Espacio / W / ↑ | Saltar |
| Esc | Salir |

## Estructura del Proyecto

```
InfinitePlatformer/
├── Game1.cs          - Clase principal del juego
├── Player.cs         - Jugador con físicas y colisiones
├── Terrain.cs        - Generador de terreno infinito
├── Camera.cs         - Sistema de cámara
├── Program.cs        - Punto de entrada
└── InfinitePlatformer.csproj
```

## Sistema de Colisiones

El motor de colisiones implementa:

1. **Detección AABB**: Intersección de rectángulos alineados a los ejes
2. **Resolución por Ejes**: Determina el lado de colisión usando overlaps mínimos
3. **Respuesta**: Ajuste de posición y velocidad según la normal de colisión

```csharp
// Pseudocódigo del algoritmo
float overlapLeft = playerRight - platformLeft;
float overlapRight = platformRight - playerLeft;
float overlapTop = playerBottom - platformTop;
float overlapBottom = platformBottom - playerTop;

// Resolver según el overlap mínimo
if (minOverlapX < minOverlapY) {
    // Colisión horizontal
} else {
    // Colisión vertical (suelo/techo)
}
```

## Requisitos

- .NET 8.0
- MonoGame 3.8.2+
- Visual Studio 2022 o VS Code

## Ejecución

```bash
dotnet restore
dotnet run
```

## Preview

El jugador (rectángulo azul/verde) avanza por plataformas generadas proceduralmente. La cámara sigue automáticamente y el terreno se genera/elimina dinámicamente para mantener rendimiento constante.

## Licencia

MIT License - Libre para usar en proyectos personales y comerciales.
