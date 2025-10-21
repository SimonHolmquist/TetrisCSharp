# 07 — Input Mappings (v2)

Compatibilizá **teclas de flecha** (moderno) con un **layout numérico retro** inspirado en la captura (7‑8‑9‑4‑5).

## Controles de Juego

| Tecla                         | Acción                                         |
|------------------------------|-----------------------------------------------|
| ← / `7`                      | Mover a la izquierda                          |
| → / `9`                      | Mover a la derecha                            |
| ↓ / `4`                      | **Soft‑drop** (acelera caída)                 |
| ↑ / `8`                      | Rotar horario                                  |
| Ctrl + ↑ / `Shift+8`         | Rotar **antihorario**                          |
| `5`                          | **Hard‑drop** (caída instantánea)             |
| `H`                          | Mostrar/Ocultar ayuda de controles             |
| `P` / `Esc`                  | Pausa (abre menú)                              |
| `Enter`                      | Confirmar opción de menú                       |
| `Space`                      | Reiniciar partida desde el menú de pausa       |

> Nota: Las teclas `7/8/9/4/5` emulan el estilo de la versión retro de la imagen. En Windows Terminal / PowerShell funciona sin configuración adicional. En algunas consolas Linux puede requerir desactivar *NumLock* o capturar *escape sequences*.

## Navegación de Menú
- ↑/↓ para seleccionar, **Enter** confirma. La opción activa **parpadea**.

## Repetición (DAS/ARR simple)
- **DAS** (delay antes de repetición): 150 ms.
- **ARR** (repetición): 50 ms. (Aplicado a izquierda/derecha y soft‑drop).