# Build script - Find techniques and move them to the lib folder of the framework's path.
$techniques_folder = "../../../Techniques/"
$lib_folder = $PSScriptRoot + "\bin\debug\lib\"

New-Item -ItemType "directory" -Path $lib_folder 

write-host "

 █████╗ ██╗   ██╗████████╗ ██████╗    ██╗    ██╗ ██╗███╗   ██╗
██╔══██╗██║   ██║╚══██╔══╝██╔═████╗   ██║    ██║███║████╗  ██║
███████║██║   ██║   ██║   ██║██╔██║   ██║ █╗ ██║╚██║██╔██╗ ██║
██╔══██║██║   ██║   ██║   ████╔╝██║   ██║███╗██║ ██║██║╚██╗██║
██║  ██║╚██████╔╝   ██║   ╚██████╔╝██╗╚███╔███╔╝ ██║██║ ╚████║
╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝ ╚══╝╚══╝  ╚═╝╚═╝  ╚═══╝
                                Build Script - vs 0.0.1
"

write-host "Hello, I'm trying to build your project because your lazy ass don't want to move files around."

$techniques = Get-Childitem –Path $techniques_folder

ForEach ($technique in $techniques) {
    write-host "[!] Found: $technique"
    $binfolder = "$techniques_folder$technique\bin\Debug\$technique.exe"
    $csfolder = "$techniques_folder$technique\Program.cs"
    if(Test-Path($binfolder)) {
        Copy-Item -Path $binfolder -Destination $lib_folder
        write-host "[*] $technique - OK"
    } else {
        write-host "[!] Trying to compile technique ($technique)"
        if(Test-Path($csfolder)) {
            $binname = "$lib_folder$technique.exe"
            C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /out:$binname $csfolder
            if(test-path($lib_folder)) {
                write-host "[!] Technique $technique compiled!"
            }
        } else {
            write-host "[!] Brow, are u kidding? Can't even find the source! ($technique)"
        }        
        write-host "[*] $technique is not compiled ($binfolder)!"
    }
}