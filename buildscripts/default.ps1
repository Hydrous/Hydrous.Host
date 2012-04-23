properties {
    $version = "1.1.0"
    $basePath = resolve-path ..\
    $tools = "$basePath\tools"
    $build = "$basePath\build"
    $release = "$basePath\release"
}

include .\psake-ext.ps1

task default

task Clean {
    rmdir $build -recurse -force -ErrorAction SilentlyContinue
    rmdir $release -recurse -force -ErrorAction SilentlyContinue
    
    mkdir $build | out-null
    mkdir $release | out-null
}

task Init {
    $env:revision = Get-Git-Commit
}

task Version -depends Init {
    dir "$basePath\src\" -filter "*.csproj" -recurse | foreach {
        $currentPath = $_.Directory.FullName;
        $target = "$currentPath\Properties\AssemblyInfo.cs"
        Generate-Assembly-Info `
            -title $_.Directory.Name `
            -file $target `
            -product "Hydrous.Host" `
            -version $version `
            -revision $env:revision `
            -copyright "Copyright Darren Kopp, Digital Business Integration 2011-2012" `
            -company "Digital Business Integration"
    };
}

task Build {
    exec { `
        msbuild /m "$basePath\src\Hydrous.Host.sln" `
            /v:m `
            /t:Build `
            /p:Configuration=Release `
            /p:OutputPath="$build"
    }
}

task Zip {
    [System.Reflection.Assembly]::LoadFrom("$tools\DotNetZip\Ionic.Zip.dll") | out-null;

    $zipfile =  new-object Ionic.Zip.ZipFile;
    $e= $zipfile.AddFile("$build\Hydrous.Host.exe","");
    $e = $zipfile.AddFile("$build\Hydrous.Host.pdb","");
    $e = $zipfile.AddFile("$build\Hydrous.Hosting.Core.dll","");
    $e = $zipfile.AddFile("$build\Hydrous.Hosting.Core.pdb","");
    $e = $zipfile.AddFile("$build\Hydrous.Hosting.dll","");
    $e = $zipfile.AddFile("$build\Hydrous.Hosting.pdb","");
    $e = $zipfile.AddFile("$build\log4net.config","");
    $zipfile.Save("$release\$version.$env:revision.zip");
    $zipfile.Dispose();
}

task Release -depends Clean, Version, Build, Zip {
    
}