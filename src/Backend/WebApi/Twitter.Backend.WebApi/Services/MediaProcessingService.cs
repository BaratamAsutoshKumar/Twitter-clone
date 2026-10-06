using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace TwitterClone.Application.Services;

public class MediaProcessingService
{
    public async Task<string> DownloadProfilePictureAsync(string userProvidedUrl)
    {
        // VULNERABILITY: SSRF (Server-Side Request Forgery) (CWE-918)
        // Untrusted URL is fetched by the server without validation (could access internal IPs)
        using var client = new HttpClient();
        var response = await client.GetStringAsync(userProvidedUrl); 
        return response;
    }

    public void ParseUserMetadata(string xmlContent)
    {
        // VULNERABILITY: XML External Entity (XXE) Injection (CWE-611)
        // Enabling DtdProcessing.Parse allows external entity resolution
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        
        using var stringReader = new System.IO.StringReader(xmlContent);
        using var xmlReader = XmlReader.Create(stringReader, settings);
        
        while (xmlReader.Read()) { }
    }
}