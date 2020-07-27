[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$apimEnv,
    [Parameter(Mandatory = $true)][string]$certPassword
)

# pfx certification stored path
$certroopath = "$($PSScriptRoot)"

# Certification name - should match with FQDN of Windows Azure creating VM
$certname = "$($apimEnv)-api-customerengagement.platform.education.gov.uk"

# Certification password - should be match with password of Windows Azure creating VM

$cert=New-SelfSignedCertificate -DnsName "$certname" -CertStoreLocation cert:\LocalMachine\My -Subject "cn=$($apimEnv)-api-customerengagement.platform.education.gov.uk" -Type Custom -KeySpec Signature -KeyExportPolicy Exportable -HashAlgorithm sha256 -KeyLength 4096 -KeyUsageProperty Sign -KeyUsage CertSign

# Generate certificates from root (For Client Authentication only) (Not for web server)

$cert1=New-SelfSignedCertificate -Type Custom -KeySpec Signature -Subject "cn=$($apimEnv)-api-customerengagement.platform.education.gov.uk" -HashAlgorithm sha256 -KeyLength 2048 -CertStoreLocation cert:\LocalMachine\My -Signer $cert -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2")

$password = ConvertTo-SecureString -String $certpassword -Force -AsPlainText

$certwithThumb="cert:\localMachine\my\"+$cert1.Thumbprint

$filepath="$certroopath\$certname.pfx"

Export-PfxCertificate -cert $certwithThumb -FilePath $filepath -Password $password

'Thumbprint: '+$cert1.Thumbprint
