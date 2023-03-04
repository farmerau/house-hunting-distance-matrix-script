namespace Austin.Farmer.Codes.HouseHunting.DistanceMatrix;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
    {
        int length = collection.Count();
        int pos = 0;
        do
        {
            yield return collection.Skip(pos).Take(batchSize);
            pos += batchSize;
        } while (pos < length);
    }
}