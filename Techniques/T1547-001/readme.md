# T1547-001 - Registry Run Keys / Startup Folder

This module has two types of execution:

- registry - Sets script path informed as parameter to the run keys (HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run).
- startup - Adds .bat file with command to be executed as parameter, to the user's startup folder (Environment.SpecialFolder.Startup).

## Examples

For both techniques you may add a third parameter (pos[2]) with the name of the file or registry. If not informed, default value is the ID.

```json
{
    "Technique": "T1547-001",
    "Parameters": [
        "startup",
        "cmstp.exe /s /ns C:\\Users\\public\\d12i.txt",
    ]
}
```

```json
{
    "Technique": "T1059-001",
    "Parameters": [
        "registry",
        "C:\\Users\\public\\d12i.vbs",
	]
}
```

