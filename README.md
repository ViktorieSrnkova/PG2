# Semestrální práce PG2 2023
## Struktura
Vše začíná v `Game.cs`, který nastaví OpenGL, kameru atd. Dále vytvoří scénu.
Scény se nacházejí v `Scenes/` a dědí ze třídy `Scene`. Scéna je zodpovědná za vytvoření
objektů, které se v ní nacházejí a za nastavení uniformů shaderu. 

Meshe se nacházejí v `Objects/` ve formátu `.obj`. Objekt je potřeba registrovat v `Settings/Atlas/objects.json`, pak je získáváme podle klíče (jména).
Objekt načítá materiál `.mtl` (`Materials/`), je možné různým ploškám nastavit jiný materiál. Materiál načítá textury podle klíče (`Settings/Atlas/textures.json`). Pokud klíč existuje, ale textura nelze načíst, aplikuje se `global:missing_texture`.

Pro manipulaci s objekty ve scéně používáme Figure (`/Figure.cs`). Ty obsahují model matrix, pozici, velocity atd. a jsou zodpovědné za vykreslení objektu.
Pokud chceme custom chování Figure, vytvoříme novou třídu do `Entities/` a dědíme z `Figure`. Příklad `Entities/Ghost/*`. Pro figure je možné zapnout zobrazení bounding boxů (změní se konstanta `Figure.Debug` na true)

Projekt obsahuje i základní obsluhu kolizí a gravitace. Ta je implementována v `Managers/CollisionManager`. Pokud nechceme, aby byla na objekt aplikována gravitace, nastavíme `Figure.IsStatic` na true.

## Ovládání
- `WASD` - pohyb
- `Space` - nahoru
- `Shift` - dolů
- `G` - zapnutí/vypnutí ovládání kamery myší
- `Escape` - ukončení
- `F` - přepnutí fullscreen/windowed screen
- `V` - zapnutí/vypnutí VSYNC

## Splněné zadání
### Essentials
* [x] 3D GL Core profile + shaders
* [x] high performance => at least 60 FPS
* [x] allow VSync control, fullscreen vs. windowed switching
* [x] event processing (can control camera, object, app behaviour...): mouse (both axes, wheel), keyboard
* [x] multiple independently moving 3D models, at leats one loaded from file
* [x] at least three different textures (or subtextures from texture atlas etc.)
* [x] lighting model, at least 3 lights (1x ambient, 1x directional, 1x reflector: at least one is moving; +possible other lights)
* [ ] correct transparency (at least one transparent object)
  * Chybí

### Extras
* [ ] height map textured by height, proper player height coords
* [x] working collisions
  * pro objekty, které to mají zaplé a pohybují se přes velocity.
* [ ] particles
* [ ] scripting (useful)
* [x] audio

## Autoři
- [Viktorie Srnková](https://github.com/ViktorieSrnkova)
- [Vojtěch Voleman](https://github.com/vvoleman)