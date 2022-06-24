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
