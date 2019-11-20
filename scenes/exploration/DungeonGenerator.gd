extends Node2D

class Rect:
	var width
	var height
	var pos

	func _init(x, y, w, h) -> void:
		self.pos = Vector2(x, y)
		self.width = x + w
		self.height = y + h

	func center() -> Vector2:
		var centerx = floor((self.pos.x + self.width) /2)
		var centery = floor((self.pos.y + self.height)/2)
		return Vector2(centerx, centery)

	func intersect(other: Rect) -> bool:
		return (self.pos.x <= other.width) and (self.width >= other.pos.x) and (self.pos.y <= other.height) and (self.height >= other.pos.y)

func create_room(room: Rect):
	for y in range(room.pos.y, room.height):
		for x in range(room.pos.x, room.width):
			$TileMap.set_cell(x, y, 0)

func create_random_rect(mapwidth, mapheight) -> Rect:
	var w = randi() % MAX_ROOM_WIDTH + MIN_ROOM_WIDTH
	var h = randi() % MAX_ROOM_HEIGHT + MIN_ROOM_HEIGHT
	var x = randi() % (mapwidth - w) + 1
	var y = randi() % (mapheight - h) + 1
	return Rect.new(x, y, w, h)

func v_tunnel(y1: int, y2: int, x: int):
	for y in range(int(min(y1, y2)), int(max(y1, y2))):
		$TileMap.set_cell(x, y, 0)

func h_tunnel(x1: int, x2: int, y: int):
	for x in range(int(min(x1, x2)), int(max(x1, x2))):
		$TileMap.set_cell(x, y, 0)

const FLOOR_WIDTH := 30
const FLOOR_HEIGHT := 25

const MAX_ROOM_WIDTH := 5
const MAX_ROOM_HEIGHT := 5

const MIN_ROOM_WIDTH := 3
const MIN_ROOM_HEIGHT := 3

const MAX_ROOM_COUNT := 20

func _ready() -> void:
	randomize()

	for y in range(FLOOR_HEIGHT):
		for x in range(FLOOR_WIDTH):
			$TileMap.set_cell(x, y, 1)

	var num_of_rooms = randi() % MAX_ROOM_COUNT + 5

	var rooms = []

	for i in range(MAX_ROOM_COUNT):
		var r: Rect = create_random_rect(FLOOR_WIDTH, FLOOR_HEIGHT)
		var failed = false;
		for j in rooms:
			if r.intersect(j):
				failed = true;
		if !failed:
			if len(rooms) > 1:
				var nr = r.center()
				var pr: Vector2 = rooms.back().center()
				if randf() > 0.5:
					h_tunnel(pr.x, nr.x, pr.y)
					v_tunnel(pr.y, nr.y, nr.x)
				else:
					v_tunnel(pr.y, nr.y, pr.x)
					h_tunnel(pr.x, nr.x, nr.y)
			rooms.append(r)

	for i in rooms:
		create_room(i)