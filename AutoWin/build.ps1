# Build script - Find techniques and move them to the lib folder of the framework's path.
$techniques_folder = "../../../Techniques/"
$lib_folder = $PSScriptRoot + "\bin\debug\lib\"

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
    if(Test-Path($binfolder)) {
        Copy-Item -Path $binfolder -Destination $lib_folder
        write-host "[*] $technique - OK"
    } else {
        write-host "[*] $technique is not compiled ($binfolder)!"
    }
}