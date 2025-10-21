# 11 — Recomendaciones (v2)

## Diseño y mantenibilidad
- **Core puro**: Domain y Application sin dependencias de `Console.*` ni *timers* específicos.
- **Interfaces por borde**: `IRenderer`, `IInputProvider`, `IClock`, `IScoreStore`, `IRandomizer`, `IRotationSystem`.
- **Domain Events**: `PieceLocked`, `LinesCleared`, `LevelUp`, `GameOver`, `ScoreChanged`.
- **Configuración por archivo** (`appsettings.json` de ConsoleUI) para `GameConfig` y teclas.

## UX retro
- Monocromo verde, caracteres consistentes (`[]` para bloques, `.` para vacíos).
- **Ayuda contextual** (toggle `H`). **Parpadeo** en selección de menú.
- **Render por diff** para minimizar *flicker* en consolas lentas.

## Testing
- **xUnit** + **FluentAssertions**.
- Tests de propiedad (FsCheck opcional) para colisiones/rotaciones.
- Dado‑Cuando‑Entonces para *Game Over*, scoring, progresión de nivel.
- *FakeClock / FakeInput* para simular ticks y entradas.

## Observabilidad
- **Serilog** a consola/archivo para depurar estados.
- Métricas simples (contadores de líneas, tiempo por partida) opcional.

## CI/CD
- GitHub Actions: build/test y artefactos del binario de consola.
- Matriz de OS (Windows, Linux) para validar códigos de teclado.

## Roadmap sugerido
- v0.5: Blazor WASM reusando core.
- v0.6: MAUI (Android) con *touch controls*.
- v0.7: Persistencia Azure (Table/Cosmos) para ranking global.