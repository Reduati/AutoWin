# T1059-005 - Visual Basic Script

This module can execute vbs script using cscript bin.
Scripts that require parameters, you can pass them through the "parameters" attribute in the execution flow.

## Examples

Running wget.vbs to get content from an external server and store it on the local disk.


```json
{
    "Technique": "T1059-005",
    "Parameters": [
        "C:\\Temp\\wget.vbs",
        "https://attack.mitre.org/theme/images/ATT&CK_red.png",
        "C:\\Temp\\logo.png"
    ]
}
```

Just running a vbs script, without parameters

```json
{
    "Technique": "T1059-005",
    "Parameters": [
        "C:\\Temp\\wget.vbs"
    ]
}
```