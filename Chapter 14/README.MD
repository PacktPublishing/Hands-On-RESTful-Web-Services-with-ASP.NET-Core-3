# Chapter 14 - Implementing Worker Services Using .NET Core

## Run steps

1. Run `catalog` service:

```
cd "Chapter 13/Catalog"
docker-compose up --build
```

Runs the `catalog` service on port `5000`.

2. Run `cart` service: 

```
cd "Chapter 13/Cart"
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

### Create a new cart and fetch the details

1. Create a new cart with the item id previosly created:
```
curl -X POST \
  http://localhost:5002/api/cart/ \
  -H 'Content-Type: application/json' \
  -d '{
	"ItemsIds": ["<item_id>"],
	"UserEmail":"youremail@gmail.com"
}'

```

2. Fetch the information of the cart:

```
curl -X GET http://localhost:5002/api/cart/item_id

```

### Trigger the soldout event

1. Perform the delete operation on an item id:
```
curl -X DELETE \
  http://localhost:5000/api/items/item_id \
  -H 'Content-Type: application/json'

```

2. Check the message triggered in the RabbitMQ dashboard [http://localhost:15672](http://localhost:15672)

3. Verify that the catalog item with `item_id` is correctly deleted form the cart:

```
curl -X GET http://localhost:5002/api/cart/item_id
```