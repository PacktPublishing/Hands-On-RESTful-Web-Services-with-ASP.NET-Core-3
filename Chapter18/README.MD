# Chapter 18 - Deploying Services on Azure

## Verify the implementaiton

### Push the image to the container registry

1. Login into azure:

```
az login
```
2. Create a resource group:

```
az group create --name handsOn --location westeurope
```

3. Create a registry:

```
az acr create --resource-group handsOn --name <container_registry_name> --sku Basic
```

4. Login into the registry:

```
az acr login --name <container_registry_name>
```

5. Run docker compose into the Catalog folder:

```
docker-compose build
```

6. Tag the image with the registry url and push the image:

```
docker tag catalog_api
<container_registry_name>.azurecr.io/catalog_api:v1

docker push <container_registry_name>.azurecr.io/catalog_api:v1
```

