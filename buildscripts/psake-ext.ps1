function Get-File-Exists-On-Path
{
	param(
		[string]$file
	)
	$results = ($Env:Path).Split(";") | Get-ChildItem -filter $file -erroraction silentlycontinue
	$found = ($results -ne $null)
	return $found
}

function Get-Git-Commit
{
	if ((Get-File-Exists-On-Path "git.exe")){
		$gitLog = git log --oneline -1
		return $gitLog.Split(' ')[0]
	}
	else {
		return "0000000"
	}
}

function Generate-Assembly-Info
{
param(
	[string]$title,
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
    [string]$revision,
	[string]$file = $(throw "file is a required parameter.")
)
  $coreVersion = New-Object -TypeName System.Version -ArgumentList $version
  
  $asmInfo = @"
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitleAttribute("$title")]
[assembly: AssemblyCompanyAttribute("$company")]
[assembly: AssemblyProductAttribute("$product")]
[assembly: AssemblyCopyrightAttribute("$copyright")]
[assembly: AssemblyVersionAttribute("$version")]
[assembly: AssemblyInformationalVersionAttribute("$coreVersion / $revision")]
[assembly: AssemblyFileVersionAttribute("$coreVersion / $revision")]
[assembly: AssemblyDelaySignAttribute(false)]
"@

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		[System.IO.Directory]::CreateDirectory($dir) | out-null
	}

    New-Item -type file $file -Force | out-null
	$asmInfo | Out-File -Encoding utf8 $file
}