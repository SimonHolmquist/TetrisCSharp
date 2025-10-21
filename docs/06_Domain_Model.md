# 06 — Domain Model (v2)

## Entidades / VOs

### `Coord` (struct)
- `X:int (0..9)`, `Y:int (0..n)` — sistema de coordenadas: (0,0) tope‑izquierda.
- Inmutable. Métodos: `Translate(dx,dy)`.

### `CellState` (enum)
- `Empty`, `Locked(blockId)` (opcional: color/char). En consola se renderiza con un **carácter** configurable (`[]`, `::`, `██`, etc.).

### `TetrominoType` (enum)
- `I,O,T,S,Z,J,L`.

### `Rotation` (enum)
- `R0`, `R90`, `R180`, `R270`.

### `Piece`
- `Type:TetrominoType`
- `Rotation:Rotation`
- `Origin:Coord` (posición en tablero del punto de referencia)
- `Blocks: IReadOnlyList<Coord>` (coordenadas relativas al `Origin` para la rotación actual).

### `Board`
- `Width=10`, `Height=20`, con **2 filas ocultas** para *spawn/rotación*.
- `IsInside(Coord)`, `IsEmpty(Coord)`
- `TryMove(Piece, dx, dy)`
- `TryRotate(Piece, dir)` — usa `IRotationSystem`
- `Lock(Piece)` — fija bloques
- `ClearLines(): int` — devuelve cantidad de líneas limpiadas
- `IsValidPosition(Piece)`

### `GameState`
- `Board`, `CurrentPiece`, `NextPiece`, `Hold (opcional v2)`
- `Level:int`, `Score:int`, `LinesCleared:int`
- `IsPaused`, `IsOver`
- `Config: GameConfig`

### `ScoreEntry`
- `Alias`, `Points`, `Timestamp`

### `GameConfig`
- `GravityMsBase=800`, `GravityPerLevel=60`, `GravityMin=70`
- `SoftDropFactor=5`, `HardDropPerCell=2`, `SoftDropPerCell=1`
- `LinesPerLevel=10`
- `UseSevenBag:bool` (si false: RNG puro “retro”)
- `ShowHelp:bool` (HUD de controles).

---

## Servicios de Dominio

### `IRandomizer`
- `NextPiece()` — **7‑bag** por defecto; variante RNG simple disponible.

### `IRotationSystem` (SRS‑lite)
- `GetOffsets(type, fromRot, toRot): Coord[]` — offsets de *wall‑kicks* simples `[(0,0), (-1,0), (1,0), (0,-1)]`.

### `IScoring`
- `AddLineClear(lines:int, level:int)` — 40/100/300/1200 × nivel
- `AddSoftDrop(cells:int)` (+1/celda)
- `AddHardDrop(cells:int)` (+2/celda)

### `IScoreStore`
- `GetTop(int n)`, `TryAdd(ScoreEntry)` — persistencia local / memoria / JSON.

### `IInputProvider`, `IRenderer`, `IClock`
- Aislados de UI para testabilidad.

---

## Reglas e Invariantes
- Nunca se permite superposición ni salirse del borde.
- *Spawn* falla ⇒ `IsOver = true`.
- `ClearLines()` debe ejecutar **antes** de generar `NextPiece()`.
- Subir **nivel** cada `LinesPerLevel` (acumulativo).

---

## Eventos de Dominio (Application los publica)
- `PieceLocked`, `LinesCleared(count)`, `LevelUp(newLevel)`, `GameOver`, `ScoreChanged`.

---

## Datos estáticos (matrices de piezas)
Se definen por **rotación** en el origen (x,y):
- `I`: `[(−2,0),(−1,0),(0,0),(1,0)]` en R0
- `O`: `[(0,0),(1,0),(0,1),(1,1)]`
- `T`, `S`, `Z`, `J`, `L`: matrices estándar.
(Implementar como *lookups* inmutables).