# Microservices Integration Guide

This guide will help you integrate a microservice into your application.

An internal client can either be a frontend, API or anything else that will consume your microservice. It must live in the same kubernetes cluster where your application is hosted.

Default ports:

- REST API: 80 (HTTP)
- gRPC: 443 (HTTPS)

## Configuration

### Local Clients

#### Setup (debug or release)

1. Run your microservice first. You can configure the port through the configuration or the default port will be assigned.
2. Configure your client application to use your localhost address and the selected microservice port. Format: 127.0.0.1:80 (local-ip-address:port-number)
3. Run your client application.
4. Query your microservice using your client application.

### Internal Clients

#### Setup (debug or release)

1. Your microservice must be deployed to the kubernetes cluster where your client application resides and must be running.
2. Configure your client application to use the Kubernetes Service's generated IP address or domain.
3. Query your microservice using your client application.

### External Clients

1. Your microservice must be deployed and running. It can or not be in a kubernetes cluster.
2. If it's not in Kubernetes you must open your ports and port-forward the microservice correctly. (ATTENTION: this is not recommended as it will open the doors to possible invaders)
3. If it's in Kubernetes you can keep it there and use a Kubernetes Service with an Ingress so it exposes the port with a specific domain you want.


## Troubleshooting

- Check local Windows System logs for errors.
- Check pod logs (and application logs) for any connection error messages.
- Check client environment configurations, you might be using the local appsettings.json while in Kubernetes we might be using a different one depending on the environment.

