# Chapter 15 - Securing Your Service

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

### Create a new user and authenticate

1. Create a new user:
```
curl -X POST \
  https://localhost:5001/api/user \
  -H 'Content-Type: application/json' \
  -d '{
    "email": "newuser@example.com",
    "password": "<my_secret_password>",
    "name": "Your name"
}'
```

2. Login using the new user:

```
curl -X POST \
  https://localhost:5001/api/user/auth \
  -H 'Content-Type: application/json' \
  -d '{
    "email": "newuser@example.com",
    "password": "<my_secret_password>"
}'

```

3. Fetch user information using the JWT Token:

```
curl -X GET https://localhost:5001/api/user -H 'Authorization: Bearer <my_token>'
```
