# Un Plataformas 3D
El objetivo del juego es aguantar vivo el másimo tiempo posible, durante la partida irán apareciendo zombies que te perseguirán para atacarte
y tienes que ir recolectando relojes, matando y evitando a los zombies, para resetear el tiempo de juego evitando que llegue a las 12pm para no morir.

## Controles
- Movimiento: [W][A][S][D]
- Salto: [Espacio]
- Disparo: [Click Izdo]
- Cambiar de arma: [1][2] Teclado alfánumérico

## Observaciones
- Los items de salud, escudo y munición sol límitados y siempre se encuentran en el mismo sitio
- Hay un límite de zombies en la escena pero vuelven a aparecer en un spawn aleatorio cuando no exceden el límite
- El disparo de las armas es siempre en línea recta desde la cabeza del Player y no se puede apuntar
- El juego está totalmente sonorizado

## Añadidos extra
- Skybox Procedural personalizado (Asesets/Materials/Sky.material)
- Daño por caída
- 2 Tipos de armas (automática y semiautomática)
- UI personalizada
- Un Main Menu en el que aparece un Zombie bailando y puedes mover la cámara a su alrededor
- La pantalla de Game Over te lleva automáticamente al Main Menu tras unos segundos
- Los zombies se escuchan más alto cuando están más cerca del Player

## Mejoras
- Scripts de disparo de la PEC 2 adaptados al Third Person Controller
- Ahora las armas tienen un rango de alcance al disparar (se puede visualizar en el Editor con un Gizmo desde la cabeza del personaje)
- Las luces de las farolas se apagan y enciende progresivamente
- El reloj no puede aparecer encima de un árbol, una valla, una farola, o un edificio al que nop hay acceso para el jugador
- Hay tres relojes en el mapa para que no sea demasiado complicado encontrarlos

## Cosas que faltan o se deberían mejorar
- Partículas cuando hieren a un zombie
- Las armas no he conseguido posicionarlas correctamente acorde con las animaciones de disparo del Player