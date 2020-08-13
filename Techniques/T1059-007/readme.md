# T1059-007 - Javascript/JScript

This module has two types of execution:

- script - Runs JS file using 'cscript.exe'
- hta - Runs JS file using 'mshta.exe'

## Examples

You can find the demo files on the example folder inside this technique project.

```json
{
    "Technique": "T1059-007",
    "Parameters": [
    "hta",
    "c:\examples\demo.hta"
    ]
		}
```

```json
{
    "Technique": "T1059-007",
    "Parameters": [
        "script",
        "c:\examples\demo.js"
    ]
}
```

