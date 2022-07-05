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