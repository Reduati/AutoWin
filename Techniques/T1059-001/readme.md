# T1059-001 - Powershell

This module has two types of execution:

- dll - Runs encoded base64 powershell code using MS System.Management.Automation DLL, without using powershell.exe binary.
- binary - Runs inline code or a file with the right attribute, using powershell.exe.

## Examples

Creates a result file in "c:\users\public":

```json
{
    "Technique": "T1059-001",
    "Parameters": [
    "dll",
    "YWRkLWNvbnRlbnQgImM6XHVzZXJzXHB1YmxpY1xyZXN1bHQudHh0IiAiSGV5IGZyb20gcG93ZXJzaGVsbCBETEwgO0Qi"
    ]
		}
```

Run file.ps1 using powershell.exe:

```json
{
    "Technique": "T1059-001",
    "Parameters": [
        "binary",
        "-executionpolicy unrestricted -file C:\\scripts\\file.ps1"
    ]
}
```

