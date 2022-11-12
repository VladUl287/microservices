using BasketApi.Dtos;

namespace BasketApi.Common;

internal static class Errors
{
    public static readonly Error ItemAlreadyExists = new("Товар уже в корзине.");
    public static readonly Error ItemNotExists = new("Такого товара не существует.");
}