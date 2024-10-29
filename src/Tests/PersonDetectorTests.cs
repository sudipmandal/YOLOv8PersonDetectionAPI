using Moq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using YoloPersonDetectionAPI.Controllers;
using YoloPersonDetectionAPI.Detectors;
using YoloPersonDetectionAPI.Models;
using YoloPersonDetectionAPI.Services;

namespace YoloPersonDetectionAPI.Tests;

[TestFixture]
public class PersonDetectorTests
{
    private Mock<ILogger<PersonDetectionController>> _loggerMock;
    private Mock<ILogger<EnvDefaultService>> _loggerMockEnvDefault;
    private PersonDetectionController _controller;
    private PersonDetector _personDetector;
    private EnvDefaultService _envDefaultService;
    private LocalCacheService _localCacheService;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<PersonDetectionController>>();
        _loggerMockEnvDefault = new Mock<ILogger<EnvDefaultService>>();
        _controller = new PersonDetectionController(_loggerMock.Object);
        _personDetector = new PersonDetector(_loggerMock.Object);
        _envDefaultService = new EnvDefaultService(_loggerMockEnvDefault.Object);
        _localCacheService = new LocalCacheService();
        

    }

    private string GetBase64ImageFromFile(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        return Convert.ToBase64String(imageBytes);
    }

    [Test]
    public async Task Post_ValidImage_ReturnsOkWithHumanCount()
    {
        // Arrange
        var base64Image = GetBase64ImageFromFile("Assets/Test/test_img_2_ppl.jpg"); // Change the path
        var request = new ImageRequest { Base64Image = base64Image };
        var expectedHumanCount = 2; 
        
        var cancellationToken = new CancellationToken();
        await _envDefaultService.StartAsync(cancellationToken);
        await _localCacheService.StartAsync(cancellationToken);
        
        // Act
        var result = await _controller.Post(request) as OkObjectResult;
        
        
        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result.StatusCode.Value);
        ClassicAssert.AreEqual(expectedHumanCount, ((dynamic)result.Value).NumberOfHumans);
    }

    [Test]
    public async Task Post_NullRequest_ReturnsOkWithZeroHumans()
    {
        // Arrange
        ImageRequest request = null;
        var cancellationToken = new CancellationToken();
        await _envDefaultService.StartAsync(cancellationToken);
        await _localCacheService.StartAsync(cancellationToken);

        // Act
        var result = await _controller.Post(request) as OkObjectResult;

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result.StatusCode);
        ClassicAssert.AreEqual(0, ((dynamic)result.Value).NumberOfHumans);
        _loggerMock.Verify(logger => logger.LogWarning("Bad Request, No image data received"), Times.Once);
    }

    [Test]
    public async Task Post_EmptyBase64Image_ReturnsOkWithZeroHumans()
    {
        // Arrange
        var request = new ImageRequest { Base64Image = string.Empty };
        var cancellationToken = new CancellationToken();
        await _envDefaultService.StartAsync(cancellationToken);
        await _localCacheService.StartAsync(cancellationToken);

        // Act
        var result = await _controller.Post(request) as OkObjectResult;

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result.StatusCode);
        ClassicAssert.AreEqual(0, ((dynamic)result.Value).NumberOfHumans);
        _loggerMock.Verify(logger => logger.LogWarning("Bad Request, No image data received"), Times.Once);
    }

    [Test]
    public async Task Post_InvalidBase64Image_ReturnsBadRequest()
    {
        // Arrange
        var request = new ImageRequest { Base64Image = "invalid-base64" };
        var cancellationToken = new CancellationToken();
        await _envDefaultService.StartAsync(cancellationToken);
        await _localCacheService.StartAsync(cancellationToken);

        // Act
        var result = await _controller.Post(request) as BadRequestObjectResult;

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(400, result.StatusCode);
        ClassicAssert.AreEqual("An error occurred while processing the image.", ((dynamic)result.Value).Error);
        _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Post_ExceptionThrown_ReturnsBadRequest()
    {
        // Arrange
        var base64Image = GetBase64ImageFromFile("path/to/your/image.jpg"); // Change the path
        var request = new ImageRequest { Base64Image = base64Image };

        // Simulate an exception in the PersonDetector
        var personDetectorMock = new Mock<PersonDetector>(_loggerMock.Object);
        personDetectorMock.Setup(detector => detector.GetHumansInImage(It.IsAny<byte[]>())).ThrowsAsync(new Exception("Detection failed"));

        // Act
        var result = await _controller.Post(request) as BadRequestObjectResult;

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(400, result.StatusCode);
        ClassicAssert.AreEqual("An error occurred while processing the image.", ((dynamic)result.Value).Error);
        _loggerMock.Verify(logger => logger.LogError("Detection failed"), Times.Once);
    }
}
