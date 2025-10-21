# TetrisCSharp — Fase 1

- Consola retro verde/negro, render por diferencias (sin `Console.Clear`) para evitar flicker.
- Controles: ←/→/↓, 7/9/4/8/5, H, P/Esc, Enter, Space (ver ayuda en runtime).
- Scoring: 40/100/300/1200 × (level+1); soft +1/celda; hard +2/celda; nivel +1 cada 10 líneas; gravedad `max(70, 800 - level*60)`.
- Ranking local Top-10 en `scores.json`.

## Ejecutar
```bash
dotnet run --project src/TetrisCSharp.ConsoleUI -c Release
```

---

## Notas de cumplimiento

- **Dominio**: `Board` (10×20 + 2 ocultas), `Piece`, `TetrominoDefs`, colisiones, `IsValidPosition`, `TryMove`, `TryRotate` (vía `IRotationSystem`), `Lock`, `ClearLines` (devuelve filas limpiadas) conforme modelo de dominio :contentReference[oaicite:18]{index=18}.
- **SRS-lite**: kicks `[(0,0), (-1,0), (1,0), (0,-1)]` aplicados en orden, límites y overlaps respetados :contentReference[oaicite:19]{index=19}.
- **Scoring y nivel**: fórmula, drops, 10 líneas por nivel, gravedad `max(70, 800 - level*60)` tal como especificado :contentReference[oaicite:20]{index=20}.
- **Controles**: layout flechas + 7-8-9-4-5, `H`, `P/Esc`, `Enter`, `Space`; DAS=150ms, ARR=50ms (aplicados a izq/der y soft-drop) :contentReference[oaicite:21]{index=21}.
- **Loop**: separación update/render; lock→clear→score→spawn→game over en spawn inválido, acorde pseudocódigo del doc :contentReference[oaicite:22]{index=22}.
- **Renderer**: monocromo verde, HUD a la izquierda (FILAS, NIVEL, SCORE, preview `Next`) y ayuda a la derecha con toggle; diff-rendering para evitar flicker, parpadeo en menú y game over, según recomendaciones :contentReference[oaicite:23]{index=23}.
- **Portabilidad**: ninguna referencia a consola/timers en Domain/Application (toda IO en ConsoleUI).

---

## Qué quedó pendiente para próxima fase
- Reinstalar **Tests** (xUnit + FluentAssertions) y **CI** (Actions con build/test y artefacto) tal cual docs. También fake clocks/inputs y tests de colisiones, rotaciones, clears y progresión de nivel :contentReference[oaicite:24]{index=24} :contentReference[oaicite:25]{index=25}.

¿Querés que te lo empaquete como **patch** `git` listo para aplicar o que redacte los **commits** sugeridos (board+rotations, loop, renderer diff, input DAS/ARR, scoring, ranking) con mensajes?
