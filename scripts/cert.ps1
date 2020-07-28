[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$apimEnv,
    [Parameter(Mandatory = $true)][string]$certPassword
)

$certName = "$($apimEnv)-api-customerengagement.platform.education.gov.uk"


$cert = New-SelfSignedCertificate -CertStoreLocation cert:\currentuser\my -Subject "cn=$certName" -DnsName $certName 

$password = ConvertTo-SecureString -String $certPassword -Force -AsPlainText

$path = "cert:\currentuser\my\" + $cert.thumbprint

Export-PfxCertificate -cert $path -FilePath "$PSScriptRoot\$certName.pfx" -Password $password

write-host 'Thumbprint: ' $cert.Thumbprint