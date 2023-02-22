using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AkvelonTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageTransformController : Controller
    {
        private readonly ICosmosDbRepository<ImageTransform> _service;
        private readonly IStorageService _storageService;
        private readonly IMessageBroker _messageBroker;

        public ImageTransformController(ICosmosDbRepository<ImageTransform> service, 
            IStorageService storageService, IMessageBroker messageBroker)
        {
            _service = service;
            _storageService = storageService;
            _messageBroker = messageBroker;
        }

        private bool IsImageContentType(string contentType)
        {
            return 
                string.Equals(contentType, "image/jpg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(contentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(contentType, "image/gif", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(contentType, "image/png", StringComparison.OrdinalIgnoreCase) ;
        }

        [HttpPost()]
        public async Task<IActionResult> IndexAsync(IFormFile file)
        {
            if (!IsImageContentType(file.ContentType))
                return BadRequest("file should be an image");

            var id = Guid.NewGuid();
            var originalPath = $"/original_files/{id}/{file.FileName}";

            if (!(await _storageService.SaveFileAsync(originalPath, file.OpenReadStream())))
                return StatusCode(500, "file wasn't upload");

            await _service.AddItemAsync(new ImageTransform
            {
                Id = id.ToString(),
                FileName = file.FileName,
                OriginalFilePath = originalPath
            });

            await _messageBroker.SendMessageAsync(id.ToString());

            return Ok(id.ToString());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var model = await _service.GetItemAsync(id);
            var path = string.IsNullOrEmpty(model.ModifiedFilePath) ? null :
                new Uri(_storageService.GetContainerUri(), $"images/{model.ModifiedFilePath}").ToString();

            return Json(JsonConvert.SerializeObject(new ImageTransformViewModel()
            {
                State = model.State,
                Path = path
            }));
        }
    }
}
