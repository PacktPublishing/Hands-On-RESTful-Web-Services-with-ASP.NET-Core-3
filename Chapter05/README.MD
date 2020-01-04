# Chapter 5 - Web Service Stack in ASP.NET Core

## Run the application

`dotnet run` into `SampleAPI` folder

_________

## Verify the implementation

1. Get the empty list of orders:

```
curl -X GET https://localhost:5001/api/orders
```

2. Create a new order using the `POST` verb:

```
curl -X POST \
  https://localhost:5001/api/order \
  -H 'Content-Type: application/json' \
  -d '{
	"itemsIds": ["1", "2"],
	"currency": "EUR"
}'
```

3. Get the detail of the created order:

```
curl -X GET http://localhost:5000/api/orders/order_id
```

4. Fetch the full list of orders:

```
curl -X GET http://localhost:5000/api/orders/
```

5. Partially udpate the order using `PATCH` verb:

```
curl -X PATCH \
  https://localhost:5001/api/order/order_id \
  -H 'Content-Type: application/json' \
  -d '[{
       "op": "replace", "path": "/itemsIds", "value" : ["1","5"]
     }
]'
```

6. Get the detail of the updated order:

```
curl -X GET http://localhost:5000/api/orders/order_id
```
