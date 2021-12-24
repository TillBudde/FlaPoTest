This project implement a C# Restful API Service to analyze JSON product data.

As results of the queries for Articles, we return Result-objects, instead of simply returning the Article itself. This allows to return product- and query-information as well.
A result holds a list of products. These products do not contain all articles from the base Json-Data, but only those articles that are result of the query.

We consider the json data in the given url as static, so we read data from a given url only once at runtime and store it. This allows to simply access the stored data without the need to download the data for each query.
Assuming the data be dynamic, so it is possibly to change between to queries, we would need to remove the storage of the json data and request the data for each query.

Routes of the following form can be accessed:
/api/Products/{path}?url={pathToJsonData}

Replace {path} by:
{int}:				to get the Product with ID = {int}.

mostExpensive:		to get the article that is most expensive per liter.
cheapest:			to get the article that is cheapest per liter.
expensiveCheap:		to get the most expensive and cheapest article per liter.

exactPrice/{int}:	to get those articles that with price = {int}.
mostBottles:		to get those articles that come with the most bottles.

allQueries:			to get those articles from "mostExpensiveCheapest", "exactPrice/17.99" and "mostBottles". 