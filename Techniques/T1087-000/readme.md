
# T1087-000 - Account Discovery

## Description

  This module can enumerate local or domain users. At this point, it uses the native net utility to run.

## Execution Methods

  This module has 2 ways to work:

1. **Local** (Enumerate users on the local machine)
  - Parameters
    - local;
    - Additional args (Optional). Can be pass each username to get info about (see examples).
2. **Domain** (Enumerate domain users) 
  - Parameters
    - domain;
    - Additional args (Optional). Can be pass each username to get info about (see examples).

## Examples

- Local

  Can get all users on the local machine:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "local"
  ]
}
```

  Can get information about specific users on the local machine:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "local",
  "User 1",
  "User 2"
  ]
}
```

- Domain

  Can get all domain users:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "domain"
  ]
}
```

  Can get information about domain user:

```json
{
"Technique": "T1087-000",
"Parameters": [
  "domain",
  "User 1",
  "User 2"
  ]
}
```