# Chapter 19 - Documenting Your API Using Swagger

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

### Get the swagger documentation 

Fetch the swagger documentation for the catalog service:

```
curl -X GET https://localhost:5001/swagger
```
