## Security Protocol Guide

### Storing Personal Data

All personal data must be stored after hashing and encrypting.

For personal data validation it must be deencrypted and validated by the hash value.

### Endpoint Security

SSL certificates and HTTPS connections are a requirement for endpoint security.

To fetch data with non-sensitive parameters you would use a GET method.
For sensitive parameters you would need to use a POST method and pass the parameters through the JSON body as it is more secure.

### Network Security

Subnet Masks should be applied for IP address ranges.

Kubernetes pods should not be exposed unless necessary. Likewise can be said for operating systems ports.

