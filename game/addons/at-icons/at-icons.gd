@tool
extends EditorPlugin

var dock : EditorDock

#func _enable_plugin() -> void:
	## Add autoloads here.
	#pass


#func _disable_plugin() -> void:
	## Remove autoloads here.
	#pass


func _enter_tree() -> void:
	# Initialization of the plugin goes here.
	dock = EditorDock.new()
	dock.title = "@icons"
	var dock_icon_path := "res://addons/at-icons/node/at.svg"
	if FileAccess.file_exists(dock_icon_path):
		var dock_icon := load(dock_icon_path)
		if dock_icon is Texture2D:
			dock.dock_icon = dock_icon
	dock.default_slot = EditorDock.DOCK_SLOT_RIGHT_UL
	if _has_complete_icon_browser_assets():
		var dock_content := load("res://addons/at-icons/icon_browser.tscn").instantiate()
		dock.add_child(dock_content)
	add_dock(dock)

func _has_complete_icon_browser_assets() -> bool:
	var scene_text := FileAccess.get_file_as_string("res://addons/at-icons/icon_browser.tscn")
	if scene_text.is_empty():
		return false

	for line in scene_text.split("\n"):
		if not line.begins_with("[ext_resource"):
			continue
		var path_start := line.find(" path=\"")
		if path_start == -1:
			return false
		path_start += 7
		var path_end := line.find("\"", path_start)
		if path_end == -1 or not FileAccess.file_exists(line.substr(path_start, path_end - path_start)):
			return false

	return true

func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	remove_dock(dock)
	dock.queue_free()
	dock = null
