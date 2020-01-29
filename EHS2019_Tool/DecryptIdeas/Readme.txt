[Purpose]

Decrypt IDEAS response XML to read the returned card face data

[Prerequisite]

1. PC with IDEAS and eHS(S) eCert installed (or run in production server directly)
2. Login user with access right to access eCert primary key
3. Config "DecryptIdeas.exe.config" with correct IDEAS decrypt Cert Thumbprint

[How to use]

Copy Ideas Response [Encrypted XML] to this application and click <Decrypt>

[Location of Ideas Response [Encrypted XML]]

Database: 	[dbEVS_InterfaceLog]
Table and column:	[IDEAS_RA_SAML_RESPONSE].[response]
