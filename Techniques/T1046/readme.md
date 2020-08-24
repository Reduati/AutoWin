
# T1046 - Network Service Scanning

## Description

  This module performs a simple TCP scan into the provided IP range and port list and returns a list of open ports.

## Execution Methods

  This module has only one execution method.

## Examples

Performs TCP scan 

```json
{
"Technique": "T1046",
"Parameters": [
  "10.0.0.1/24",
   "21,22,80,443,8080,8443"
  ]
}
```

If you need to perform the scan into a single target, you must use the CIDR notation with full mask (/32):

```json
{
"Technique": "T1046",
"Parameters": [
  "10.5.7.11/32",
   "80,8080"
  ]
}
```
