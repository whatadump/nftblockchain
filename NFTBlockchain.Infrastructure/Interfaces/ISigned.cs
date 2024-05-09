namespace NFTBlockchain.Infrastructure.Interfaces;

internal interface ISigned<T>
{
    public string Sign { get; }
    public string PublicKey { get; }
    public T Data { get; }
}