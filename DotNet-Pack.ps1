Param(
    [string]$directory,
    [string]$output,
    [string]$version
)

if($directory -eq [System.String]::Empty) {
    $directory = $PSScriptRoot
}

if ($output -eq [System.String]::Empty){
    $output = "$directory\nuget"
}

cd $directory

&"$directory\UpdateVersion-Powershell.ps1" -FileName $directory\Directory.Build.Props -Version $version  

$child_directories = dir -Filter "*.csproj" -Recurse -File


Foreach ($dir in $child_directories)
{
    "--------------- Processing $dir ---------------" 
    
     dotnet pack --include-symbols --include-source -o "$output" $dir.FullName
    
    "--------------- Processed $dir ---------------"
}