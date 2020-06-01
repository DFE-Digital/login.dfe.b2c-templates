<#
    .SYNOPSIS
    Upload B2C Policy XML files
        
    .DESCRIPTION
    leveraging MS Graph API, uploads B2C Policies using Graph
    
    https://docs.microsoft.com/en-us/azure/active-directory-b2c/deploy-custom-policies-devops

#>

[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$ClientID,
    [Parameter(Mandatory = $true)][string]$ClientSecret,
    [Parameter(Mandatory = $true)][string]$TenantId,
    [Parameter(Mandatory = $true)][string[]]$PolicyNames,
    [Parameter(Mandatory = $true)][string[]]$FilePaths
)

try {
    $body = @{grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = $ClientID; client_secret = $ClientSecret }

    $response = Invoke-RestMethod -Uri https://login.microsoftonline.com/$TenantId/oauth2/v2.0/token -Method Post -Body $body
    $token = $response.access_token

    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $headers.Add("Content-Type", 'application/xml')
    $headers.Add("Authorization", 'Bearer ' + $token)


    for ($i=0; $i -eq $PolicyNames.Length; $i++) {
        $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $PolicyNames[$i] + '/$value'
        $policycontent = Get-Content $FilePaths[$i]
        $response = Invoke-RestMethod -Uri $graphuri -Method Put -Body $policycontent -Headers $headers
    }

    Write-Host "Policy" $PolicyId "uploaded successfully."
}
catch {
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__

    $_

    $streamReader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
    $streamReader.BaseStream.Position = 0
    $streamReader.DiscardBufferedData()
    $errResp = $streamReader.ReadToEnd()
    $streamReader.Close()

    $ErrResp

    exit 1
}

exit 0