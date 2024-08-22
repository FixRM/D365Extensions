﻿$url = "https://raw.githubusercontent.com/MicrosoftDocs/powerapps-docs/main/powerapps-docs/developer/data-platform/includes/data-service-error-codes.md"

$credits = @"
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by a tool. Please refer to original documentation from Microsoft:
//    $url
//
//    Changes to this file may cause incorrect behavior and will be lost if
//    the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
"@

$nameSpace = "Microsoft.Xrm.Sdk"
$enum = "ErrorCodes"
$tab = "    "
$outfile = "..\ErrorCodes.cs"

#get errors description from docs
$resp = Invoke-WebRequest -Uri $Url
$rawContent = $resp.Content

#generate enum values from description
$searchExp = "\|``(?<hex>.+)``<br\s+\/>``(?<code>.+)``\|Name:\s+\*\*(?<name>.+)\*\*<br\s+\/>Message:\s+``(?<comment>.+)``\|.?"
$replaceExp = "///<summary>`r`n///`${hex} - `${comment}`r`n///</summary>`r`n`${name} = `${code},"

$content = $rawContent -replace $SearchExp, $ReplaceExp

#write generated code to file
$writer = [System.IO.StreamWriter]::new($outfile)

$writer.WriteLine($credits)
$writer.WriteLine("namespace $nameSpace")
$writer.WriteLine("{")
$writer.WriteLine("${tab}public enum $enum")
$writer.WriteLine("$tab{")

$reader = [System.IO.StringReader]::new($content)

#skip table header
$line= $reader.ReadLine()
$line= $reader.ReadLine()

while ($line -ne $null) {
    $line = $reader.ReadLine()

    if([string]::IsNullOrEmpty($line))
    {
        continue
    }
    
    $writer.WriteLine("$tab$tab$line")
}

$writer.WriteLine("$tab}")
$writer.Write("}")
$writer.Close()

trap {
    $writer.Close()
    "writer closed"
    break
}