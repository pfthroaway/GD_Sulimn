extends Area2D
var tile_size = 64
var inputs = {"right": Vector2.RIGHT,
            "left": Vector2.LEFT,
            "up": Vector2.UP,
            "down": Vector2.DOWN}
var disabled = false

func _input(event):
    for dir in inputs.keys():
        if !disabled && event.is_action_pressed(dir):
            move(dir)

onready var ray = $RayCast2D

func move(dir):
    ray.cast_to = inputs[dir] * tile_size
    ray.force_raycast_update()
    if !ray.is_colliding():
        position += inputs[dir] * tile_size

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	disabled=false;
	global_position=global_position.snapped(Vector2.ONE * tile_size)
	#position = position.snapped(Vector2.ONE * tile_size)
	#position += Vector2.ONE * tile_size/2

func disablePlayer() -> void:
	disabled = true
#func _process(delta: float) -> void:

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass
