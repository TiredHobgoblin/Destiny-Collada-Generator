param($v, $p)

# Packed version
#dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
#Rename-Item -Path "bin\Release\netcoreapp3.0\win-x64\publish\DestinyColladaGenerator.exe" -NewName "DestinyColladaGenerator-$v.exe"

#dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
#Rename-Item -Path "bin\Release\netcoreapp3.0\osx-x64\publish\DestinyColladaGenerator" -NewName "DestinyColladaGenerator-$v"

#dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
#Rename-Item -Path "bin\Release\netcoreapp3.0\linux-x64\publish\DestinyColladaGenerator" -NewName "DestinyColladaGenerator-$v"


# Unpacked versions for systems that block the executable from unpacking.
Get-ChildItem bin\Release\netcoreapp3.0 -Include *.dll -Recurse | Remove-Item

dotnet publish -f netcoreapp3.0 -r win-x64 -c Release --self-contained true /p:PublishTrimmed=true
Rename-Item -Path "bin\Release\netcoreapp3.0\win-x64\publish\DestinyColladaGenerator.exe" -NewName "DestinyColladaGenerator-$v.exe"

dotnet publish -f netcoreapp3.0 -r osx-x64 -c Release --self-contained true /p:PublishTrimmed=true
Rename-Item -Path "bin\Release\netcoreapp3.0\osx-x64\publish\DestinyColladaGenerator" -NewName "DestinyColladaGenerator-$v"

dotnet publish -f netcoreapp3.0 -r linux-x64 -c Release --self-contained true /p:PublishTrimmed=true
Rename-Item -Path "bin\Release\netcoreapp3.0\linux-x64\publish\DestinyColladaGenerator" -NewName "DestinyColladaGenerator-$v"

If ($p)
{
	.\PackageAll.ps1 $v
}