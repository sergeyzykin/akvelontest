using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace AkvelonTest.ImageTransformer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageTransformController : Controller
    {
        private readonly ICosmosDbRepository<ImageTransform> _service;
        private readonly IStorageService _storageService;

        public ImageTransformController(ICosmosDbRepository<ImageTransform> service,
            IStorageService storageService)
        {
            _service = service;
            _storageService = storageService;
        }

        private IImageEncoder GetEncoder(string extension)
        {
            IImageEncoder encoder = null;
            extension = extension.Replace(".", "");

            var isSupported = Regex.IsMatch(extension, "gif|png|jpe?g", RegexOptions.IgnoreCase);
            if (isSupported)
            {
                encoder = extension switch
                {
                    "png" => new PngEncoder(),
                    "gif" => new GifEncoder(),
                    _ => new JpegEncoder(),
                };
            }

            return encoder;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var model = await _service.GetItemAsync(id);
            model.State = State.InProgress;

            await _service.UpdateItemAsync(model.Id, model);

            var file = await _storageService.GetFileAsync(model.OriginalFilePath);
            var modifiedPath = $"/modified_files/{id}/{model.FileName}";

            var extension = Path.GetExtension(model.FileName);
            var encoder = GetEncoder(extension);
            if (encoder != null)
            {
                using (var output = new MemoryStream())
                using (Image image = Image.Load(file))
                {
                    image.Mutate(x => x.Rotate(RotateMode.Rotate180));
                    image.Save(output, encoder);

                    output.Position = 0;
                    await _storageService.SaveFileAsync(modifiedPath, output);

                    model.ModifiedFilePath = modifiedPath;
                    model.State = State.Done;

                    await _service.UpdateItemAsync(model.Id, model);

                    return Ok();
                }
            }

            return BadRequest();

        }
    }
}
