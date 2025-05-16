using System;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Infrastructure.Services;

public class MongoImageService
{
    private readonly IMongoDatabase _database;
    private readonly IGridFSBucket _gridFs;
    private readonly IMongoCollection<GridFSFileInfo> _filesCollection;

    public MongoImageService(
        IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
        _gridFs = new GridFSBucket(_database);
        _filesCollection = _database.GetCollection<GridFSFileInfo>("fs.files");
    }

    public async Task<ObjectId> UploadImageAsync(Stream stream, string fileName)
    {
        return await _gridFs.UploadFromStreamAsync(fileName, stream);
    }

    public async Task<byte[]> DownloadImageAsync(ObjectId id)
    {
        var stream = new MemoryStream();
        await _gridFs.DownloadToStreamAsync(id, stream);
        return stream.ToArray();
    }

    public async Task<Stream> DownloadImageStreamAsync(ObjectId id)
    {
        var stream = new MemoryStream();
        await _gridFs.DownloadToStreamAsync(id, stream);
        stream.Position = 0;
        return stream;
    }

    public async Task<GridFSFileInfo> GetImageInfoAsync(ObjectId id)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
        return await _filesCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<string> GetImageContentTypeAsync(ObjectId id)
    {
        var fileInfo = await GetImageInfoAsync(id);
        return fileInfo?.Metadata?.GetValue("contentType")?.AsString ?? "application/octet-stream";
    }

    public async Task DeleteImageAsync(ObjectId id)
    {
        await _gridFs.DeleteAsync(id);
    }

    public async Task<bool> ImageExistsAsync(ObjectId id)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
        return await _filesCollection.Find(filter).AnyAsync();
    }

    public async Task<ObjectId> UploadImageWithMetadataAsync(
        Stream stream, 
        string fileName, 
        string contentType, 
        BsonDocument metadata = null)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = metadata ?? new BsonDocument()
        };
        
        options.Metadata.Add("contentType", contentType);
        options.Metadata.Add("uploadDate", DateTime.UtcNow);
        
        return await _gridFs.UploadFromStreamAsync(fileName, stream, options);
    }

    public async Task<IEnumerable<GridFSFileInfo>> GetAllImagesInfoAsync()
    {
        return await _filesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<(byte[] content, string contentType)> GetImageWithContentTypeAsync(ObjectId id)
    {
        var fileInfo = await GetImageInfoAsync(id);
        if (fileInfo == null)
            throw new FileNotFoundException("Image not found");

        var content = await DownloadImageAsync(id);
        var contentType = fileInfo.Metadata?.GetValue("contentType")?.AsString ?? "image/jpeg";
        
        return (content, contentType);
    }

    public async Task<Stream> GetImageStreamAsync(ObjectId id)
    {
        var memoryStream = new MemoryStream();
        await _gridFs.DownloadToStreamAsync(id, memoryStream);
        memoryStream.Position = 0; // Reset position for reading
        return memoryStream;
    }
}