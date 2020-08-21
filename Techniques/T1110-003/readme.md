# T1110.003 - Brute Force

## Execution Methods

​	This technique can be used to achieve two sub techniques from brute force:

- Password Guessing ([T1110.001](https://attack.mitre.org/techniques/T1110/001/))
- Password Spraying ([T1110.003](https://attack.mitre.org/techniques/T1110/003/)) 

The way you can use one or another can be checked in the examples.

## Parameters

1. **Type** - local / domain
2. **Users** - Path of your TXT file containing the users
3. **Passwords** - Path of your TXT file containing the passwords

## Examples

This example shows how to use this technique to do a domain brute forcing, as you will see, there is no practical difference between the "Password Guessing" and "Password Spraying", since you should expect to use text files to control the input. 

```json

{
    "Technique": "T1110-003",
    "Parameters": [
    	"domain",
        "users.txt",
        "password.txt"
    ]
}

```

You could have a users.txt containing 1000 users and a password.txt with "Summer2020". There you go, Password Spraying using the same logic as Password Guessing. 

As you would expect, you can achieve the same for local by just changing the first argument to local:

```json

{
    "Technique": "T1110-003",
    "Parameters": [
    	"local",
        "users.txt",
        "password.txt"
    ]
}

```

One nice thing about the file parameter is that without passing the full file path the module will try to find the file inside the workfolder. Today, the default path is "c:\users\public". You can modify this value by enforcing a new value in your flow file or by just entering the full path of your file along with the name, ie: "c:\users\public\users.txt".