
## Adjusting held objects distance VIA scroll wheel

> Moving the holding position back and forth based on the user using the scroll wheel

- Scroll Towards you to bring the object closer
- Scroll Away from you to push object farther

Obviously we have to clamp these values,

- Considering moving the Hold position closer to the player. Maybe like 1.5 units?
- Clamped at 2 units and .75 units away from player.

## Adjusting held objects rotation

> Rotating a held object by holding down right mouse and then tracking mouse x and y inputs from there

- Usually, holding the right mouse and then moving right will rotate Counter Clockwise (Relative to the y value)
- That means rotating up and down on the X? How do you track how you will rotate on the Z axis? IDK?

No clamping let them go ham.

