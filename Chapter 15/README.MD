# Chapter 15 - Securing Your Service

## Run steps

1. Run `catalog` service:

```
cd Catalog
docker-compose up --build
```

Runs the `catalog` service on port `5000`.

2. Run `cart` service: 

```
cd Cart
docker-compose up --build
```

Runs the `cart` service on port `5002`.
_________

## Verify the implementaiton

### Create a new item record

1. Create a new genre:
```
curl -X POST \
  http://localhost:5000/api/genre \
  -H 'Content-Type: application/json' \
  -d '{
    "genreDescription": "My Genre"
}'
```

2. Create a new artist:
```
curl -X POST \
  http://localhost:5000/api/artist \
  -H 'Content-Type: application/json' \
  -d ' {
            "artistName": "My Artist"
         }'
```
3. Create a new item:
```
curl -X POST \
  http://localhost:5000/api/items \
  -H 'Content-Type: application/json' \
  -d '{
            "name": "Test",
            "description": "Description",
            "labelName": "Label",
            "price": {
               "currency": "EUR",
               "amount": 34.3
            },
            "pictureUri": "",
            "releaseDate": "2019-11-21T16:18:42+00:00",
            "format": "",
            "availableStock": 4,
            "genreId":"<genre_id>",
            "artistId":"<artist_id>"
         }'
```

### Create a new user and authenticate

1. Create a new user:
```
 curl -X POST "https://localhost:5001/api/users" "Content-Type:
   application/json" -d
   "{\"email\":\"newuser@example.com\",\"password\":\"<my_secret_password>\",\
   "name\":\"Your name\"}"
```

2. Login using the new user:

```
curl -X POST "https://localhost:5001/api/users/auth" -H "accept:
   application/octet-stream" -H "Content-Type: application/json" -d
   "{\"email\":\"newuser@example.com\",\"password\":\"<my_secret_password>\"}"

```

3. Fetch user information using the JWT Token:

```
curl -X GET \
    https://localhost:5001/api/users \
    -H 'Authorization: Bearer <my_token>'
```