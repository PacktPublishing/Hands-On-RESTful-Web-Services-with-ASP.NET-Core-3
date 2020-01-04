# Chapter 16 - Caching Web Service Responses

## Run steps

1. Run `catalog` service:

```
cd Catalog
docker-compose up --build
```

Runs the `catalog` service on port `5001`.
_________

## Verify the implementaiton

### Create a new item record

1. Create a new genre:
```
curl -X POST \
  https://localhost:5001/api/genre \
  -H 'Content-Type: application/json' \
  -d '{
    "genreDescription": "My Genre"
}'
```

2. Create a new artist:
```
curl -X POST \
  https://localhost:5001/api/artist \
  -H 'Content-Type: application/json' \
  -d ' {
            "artistName": "My Artist"
         }'
```
3. Create a new item:
```
curl -X POST \
  https://localhost:5001/api/items \
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
### Check the redis cache

1. Verify the redis cache instance:

```
redis-cli -h localhost ping
```

2. Fetch the detail of the created item:

```
curl -X GET https://localhost:5001/api/items/item_id
```

3. Scan the redis cache instance to verify the populated keys:

```
redis-cli -h localhost scan 0
```