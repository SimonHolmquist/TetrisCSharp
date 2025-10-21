# TetrisCSharp — Fase 0 (Scaffold)

Tetris retro en consola (monocromo verde) sobre **.NET 8**, con **Clean Architecture** y **core portable** (Domain + Application). Esta fase deja el repo compilando, tests de humo pasando y CI publicando artefacto del ejecutable de consola.

## Estructura
```
TetrisCSharp.sln
.editorconfig
global.json
Directory.Build.props
README.md
LICENSE
.gitignore
docs/...
src/
  TetrisCSharp.Domain/
  TetrisCSharp.Application/
  TetrisCSharp.Infrastructure/
  TetrisCSharp.ConsoleUI/
tests/
  TetrisCSharp.Tests/
.github/workflows/ci.yml
scripts/
  build.sh, build.ps1, run.sh, run.ps1
```

## Build / Test / Run
```bash
# Linux/macOS
./scripts/build.sh          # restore + build + test
./scripts/run.sh            # ejecuta la consola

# Windows (PowerShell)
./scripts/build.ps1
./scripts/run.ps1
```

## Controles (MVP planificado)
- ←/→ o 7/9: mover
- ↓ o 4: soft‑drop
- ↑ o 8: rotar horario (Ctrl+↑ antihorario)
- 5: hard‑drop
- H: ayuda · P/Esc: pausa · Enter: confirmar · Space: reinicio (en pausa)
(Ver `docs/07_InputMappings.md`)
