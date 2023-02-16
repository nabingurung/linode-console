using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace linode_console.Services;

public class S3Service : IS3Service
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<S3Service> _serLogger;

    public S3Service(IConfiguration configuration, ILogger<S3Service> serLogger)
    {
        _configuration = configuration;
        _serLogger = serLogger;
    }
   /// <summary>
   /// List all the buckets name and their attributes
   /// </summary>
    public async Task ListS3BucketsAsync()
    {
        try
        {
            var config = new AmazonS3Config
            {
                ServiceURL = _configuration["S3Info:ServiceUrl"]
            };
            var amazonS3Client = new AmazonS3Client(_configuration["S3Info:AccessKey"],
                _configuration["S3Info:SecretAccessKey"], config);

            var listBucketResponse = await amazonS3Client.ListBucketsAsync();
            foreach (var bucket in listBucketResponse.Buckets)
            {
                _serLogger.LogInformation($"bucket {bucket.BucketName}, created at {bucket.CreationDate}");
            }

            if (listBucketResponse.Buckets.Count > 0)
            {
                var bucketName = listBucketResponse.Buckets[1].BucketName;
                var listObjectsResponse = await amazonS3Client.ListObjectsAsync(bucketName);
                foreach (var obj in listObjectsResponse.S3Objects)
                {
                    _serLogger.LogInformation(
                        $"key = {obj.Key} | size = {obj.Size} | tags = {obj.ETag} | modified = {obj.LastModified}");
                }
            }
        }
        catch (Exception exception)
        {
            _serLogger.LogError($"Error : {exception.Message}");
        }

    }
}