# Notes

## Passing Data to the APi

`[FromBody]`
- Request Body

`[FromForm]`
- Form data in request body

`[FromHeader]`
- Request Header

`[FromQuery]`
- Query String parameters

`[FromRoute]`
- Route Data from the current request

`[FromService]`
- The service injected as action parameter

Using apicontroller attribute `[FromBody]` is inferred
`[FromForm]` inferred for action param types of `IFormFile` and `IformFileCollection`
`[FromRoute]` inferred for any action param name mathcing a param in the route template
`[FromQuery]` inferred for all the rest

## Validation
We have comments in the code for modelstate validation in the pointsofinterestforcreationdto and the post request in the controller

For more advanced 
- [https://fluentvalidation.net/](https://localhost:7182/api/cities/3/pointsofinterest/7)

## Logging
we are using Serilog to log to a file  
`Serilog.AspNetCore`  
**_sinks are how we log to certain places_**  
`serilog.sinks.console`  
`serilog.sinks.file` 

## Deferred Exection to the database
Construct the query, then execute it

Only executed when the query is ** **iterated** ** over
 - loop
 - to list
 - to array
 - to dictionary
 - singleton querys - average, count, first etc. 

A query variable stores the query commands, not the results
- `IQueryable<T>`: creates an expression Tree

## Returning paging metadata
### envelope 
```
{
"results": [{obj}, {obj}, ...],
"metadata": {"previousPage": "/api/...", pageSize, pageNumber, nextpage...}
}
```

### Use a location header
Location - url to next page

### 
Send envelope in a header
```bash
"X-Pagination": {"TotalItemCount":3,"TotalPageCount":3,"PageSize":1,"CurrentPage":2}

```
#  Securing an API
## Token Based Security
Only have to login with user/pass once

- Send a token on each request
- A token represents consent
- Validate token at the level of API

### Implementation
Create A token - `login` endpoint for user/pass
Token is in response

JWT
token 3 things - payload, signature, header

- Create /api/login endpoint
- Encure the api can only be accessed with a valid token.
- pass the token from the client to the API as a bearer token on each request
    `Authorization: Bearer <Token>`
