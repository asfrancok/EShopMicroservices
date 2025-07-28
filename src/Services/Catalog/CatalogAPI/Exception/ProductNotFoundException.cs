namespace CatalogAPI.Exception;

using BuildingBlocks.Exceptions;
using System;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id) : base("Product", id)
    {        
    }
}