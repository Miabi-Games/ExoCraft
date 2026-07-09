' .godot-editor.vbs (forwards all args to .ps1)
Set sh = CreateObject("WScript.Shell")

' Base command: call the PS script hidden & detached
cmd = "powershell.exe -NoProfile -NonInteractive -ExecutionPolicy Bypass -File .godot-editor.ps1"

' Forward every argument to the PS script, with quote-escaping
For i = 0 To WScript.Arguments.Count - 1
  arg = WScript.Arguments(i)
  arg = """" & Replace(arg, """", """""") & """"  ' escape internal quotes
  cmd = cmd & " " & arg
Next

' 0 = hidden window, False = do not wait (detached)
sh.Run cmd, 0, False
