namespace NFTBlockchain.Infrastructure.Options;

public class NFTFileOptions
{
    public string NFTFileDirectory { get; init; }
    
    public string BlockchainFilename { get; init; }
    
    public string ArbiterPublicKey { get; set; }
}