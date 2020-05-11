dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true

# Unpacked versions for systems that block the executable from unpacking.
Get-ChildItem Multifile -Include *.dll -Recurse | Remove-Item
dotnet publish -r win-x64 -c Release /p:PublishTrimmed=true -o Multifile\win-x64
dotnet publish -r osx-x64 -c Release /p:PublishTrimmed=true -o Multifile\osx-x64
dotnet publish -r linux-x64 -c Release /p:PublishTrimmed=true -o Multifile\linux-x64