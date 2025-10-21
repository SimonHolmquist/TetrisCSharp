# 01 — Overview (v2): Tetris Retro · Clean Architecture (.NET 8)

Este proyecto implementa **Tetris** con estética retro (monocromo verde, consola) y un **core de dominio portable** para reusar en Web (Blazor WASM) y Mobile (MAUI). Prioriza **robustez**, **testabilidad** y **portabilidad**.

---

## 🎯 Objetivos (ordenados por prioridad)
1) **MVP consola** 1:1 con estética retro (ver sección UI): loop estable, colisiones correctas, puntaje clásico y ranking local.
2) **Core reusable**: dominio y aplicación agnósticos de UI (sin `Console.*` en Domain/Application). Inyección de dependencias limpia.
3) **Testing**: cobertura de reglas (rotación, colisiones, clear de líneas, scoring).
4) **CI**: build + tests en GitHub Actions.
5) **Escalabilidad**: mantener límites entre capas para Blazor/Mobile sin forks del core.

### No‑objetivos iniciales
- Multiplayer, seeds/replays, autenticación, skins, música, efectos gráficos avanzados.
- SRS completo oficial con todas las tablas de kicks (veremos una **SRS‑lite** acorde a consola).

---

## 🧱 Arquitectura (Clean)
**Capas**: Domain → Application → Infrastructure → UI(Console).  
**Interfaces clave** (definidas en Application): `IRenderer`, `IInputProvider`, `IClock`, `IRandomizer`, `IScoreStore`.
- **Domain**: entidades, value objects, servicios puros, reglas e invariantes.
- **Application** (CQRS + MediatR): orquesta casos de uso (p. ej. *Tick*, *Move*, *Rotate*, *Drop*, *Pause*), publica *Domain Events*.
- **Infrastructure**: ScoreStore (in‑memory / archivo JSON), Randomizer (7‑bag o RNG), persistencia simple.
- **UI(Console)**: Renderer ASCII, Input Provider (teclas), Bootstrap/DI y loop.

---

## 🎮 Mecánicas de juego (MVP)
- Tablero **10×20**. Siete tetrominós (I,O,T,S,Z,J,L). *Spawn* centrado arriba (fila oculta).
- **Movimiento**: izquierda/derecha, **soft‑drop**, **hard‑drop**, **rotación horaria** y **antihoraria**.
- **Rotación**: SRS‑lite con intentos de *wall‑kicks* simples: offsets `[(0,0), (-1,0), (1,0), (0,-1)]`.
- **Bloqueo**: cuando ya no puede bajar; luego `ClearLines()` y `NextPiece()`.
- **Game Over**: al no poder *spawnear* la pieza.
- **Puntaje**: clásico (40/100/300/1200 por 1/2/3/4 líneas) + **soft‑drop (+1/celda)**, **hard‑drop (+2/celda)**. Nivel sube cada **10 líneas**.
- **Velocidad**: `gravityMs = max(70, 800 - level * 60)`; *soft‑drop* acelera ×5.

---

## 🖥️ UI retro (consola)
- Paleta **verde brillante sobre negro**; borde y tablero delineados con ASCII (ej.: `|`, `=`, `^`, `.`).
- HUD a la izquierda: **FILAS**, **NIVEL**, **SCORE**; a la derecha **ayuda de controles**.
- Opción para **mostrar/ocultar** ayuda en runtime.
- Texto fijo ancho monoespaciado; evitar *flicker* (doble *buffer* o *diff rendering*).
- ASCII Art inicial `TETRIS`, menú: `START`, `RANKING`, `EXIT` (selección con parpadeo).

---

## 🧪 Tests (mínimos)
- Colisión contra paredes/suelo/piezas.
- Rotación válida con y sin *wall‑kick*.
- Clear de 1/2/3/4 líneas y scoring asociado.
- *Hard/soft drop* suman puntos por celda.
- *Game Over* al fallar el *spawn*.

---

## 🔧 Tooling y CI
- .NET 8, xUnit, FluentAssertions, FluentValidation, MediatR, Serilog (opc. consola).
- GitHub Actions: `dotnet restore/build/test`, artefacto zip de consola.
- `dotnet format` en CI; *editorconfig* y *stylecop* opcionales.

---

## 📦 Repositorio (propuesta)
```
TetrisGame/
  src/
    TetrisGame.Domain/
    TetrisGame.Application/
    TetrisGame.Infrastructure/
    TetrisGame.ConsoleUI/
  tests/
    TetrisGame.Tests/
  .github/workflows/ci.yml
  README.md
```

---

## 🔭 Roadmap corto
- v0.1: bucle, dibujo básico, movimiento y rotación, *hard/soft drop*.
- v0.2: clear de líneas + score + nivel.
- v0.3: ranking local, menú completo, ayuda runtime.
- v0.4: refactor de *render diff* y test suite amplio.