# Plan Detallado (v2) — Tetris Retro Clean Architecture

## Fase 1 — Consola (MVP)
- **Loop básico** con gravedad, input, rotación SRS‑lite, soft/hard‑drop.
- **Clear de líneas** y **score** completo (líneas + drops), **nivel** por 10 líneas.
- **HUD** con score, nivel y líneas; **ayuda** de controles con toggle.
- **Ranking local** (Top‑10, JSON).
- **Menús**: Inicio, Pausa (con confirmación), Game Over (alias).

### Criterios de aceptación
- No hay *flicker* perceptible; se usa render incremental o doble buffer.
- Colisiones correctas y kicks mínimos.
- Tests verdes: colisiones, rotación, clear 1/2/3/4, progresión nivel, scoring por drop.
- `dotnet test` en CI + artefacto del ejecutable.

## Fase 2 — Calidad
- Refactor de límites de capas (no UI en Domain/Application).
- Suite de tests ampliada. Coverage objetivo ≥ 80% Domain/Application.
- StyleCop/EditorConfig, `dotnet format` obligatorio en CI.

## Fase 3 — Web (Blazor WASM)
- Reutilizar **Domain/Application** al 100%.
- Renderer HTML/CSS con paleta retro. Deploy en Azure Static Web Apps.

## Fase 4 — Mobile (MAUI)
- Controles táctiles, vibración al *lock* y al *clear*.
- Build de APK en Actions.

## Repositorio
```
TetrisGame/
  src/ (Domain, Application, Infrastructure, ConsoleUI)
  tests/ (TetrisGame.Tests)
  .github/workflows/ci.yml
  README.md
  docs/ (estos .md)
```