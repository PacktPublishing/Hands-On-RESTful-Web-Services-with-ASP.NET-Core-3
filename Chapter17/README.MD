# Chapter 17 - Logging and Health Checking

## Run steps

1. Run `catalog` service:

```
cd Catalog
docker-compose up --build
```

Runs the `catalog` service on port `5001`.

2. Run `cart` service: 

```
cd Cart
docker-compose up --build
```

Runs the `cart` service on port `5002`.
_________

## Verify the implementaiton

### Get the health status

Fetch health status of the catalog service:

```
curl -X GET https://localhost:5001/health
```
