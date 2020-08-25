# T1036-004 - Masquerading: Masquerade Task or Service

## Execution Methods

    This module only has one execution method.

## Parameters
    1 - binPath - Required
        Path to the desired service script/executable. i.e. \Users\Public\bkp.exe
           
    2 - displayName - Optional
        displayName of the service beign created. Defaults to Microsoft Automatic Backup Service.

# Examples

Creates a service named "Microsoft Automatic Backup Service" pointing to the "\Users\Public\bkp.exe" script:

```json
{
    "Technique": "T1036-004",
    "Parameters": [
        "\\Users\\Public\\bkp.exe",
        "Microsoft Backup Manager"
    ]
}
```
