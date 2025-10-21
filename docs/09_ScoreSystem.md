# 09 — Score System (v2)

## Puntos por líneas (clásico)
| Líneas | Base |
|-------:|-----:|
| 1      |  40  |
| 2      | 100  |
| 3      | 300  |
| 4      | 1200 |

**Fórmula**: `points = base * (level + 1)` (o `base * level` si preferís la variante anterior).

## Drop scoring
- **Soft‑drop**: `+1` por **celda** descendida manualmente.
- **Hard‑drop**: `+2` por **celda** descendida instantáneamente.

## Progresión de nivel
- **Subir nivel** cada **10 líneas** acumuladas.  
- **Velocidad**: `gravityMs = max(70, 800 - level * 60)`.

## Ejemplo
- Nivel 3 limpia un Tetris (4 líneas): `1200 * (3+1) = 4800` puntos.
- Luego hace hard‑drop de 7 celdas: `7 * 2 = 14` puntos adicionales.

## Persistencia y Ranking
- Top‑10 local (memoria / JSON). Se guarda al finalizar la partida si supera el último.
- `ScoreEntry { Alias, Points, Timestamp }`.
- Orden **descendente** por `Points` y **ascendente** por `Timestamp` como desempate estable.

## Validaciones
- Alias `[A-Z0-9]{2,10}` (normalizar a mayúsculas).
- Evitar *overflow* con `checked` o `long` si es necesario.