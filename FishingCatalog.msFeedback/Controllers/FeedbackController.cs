using FishingCatalog.Core;
using FishingCatalog.msFeedback.Contracts;
using FishingCatalog.msFeedback.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msFeedback.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController(FeedbackRepository feedbackRepository) : Controller
    {
        private readonly FeedbackRepository _feedbackRepository = feedbackRepository;

        [HttpGet]
        public async Task<ActionResult<List<Feedback>>> Get()
        {
            var dbResp = await _feedbackRepository.GetAll();
            return Ok(dbResp);
        }
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Feedback>> GetById(Guid id)
        {
            return Ok(await _feedbackRepository.GetById(id));
        }
        [HttpGet("byUser/{userId:Guid}")]
        public async Task<ActionResult<List<Feedback>>> GetByUser(Guid userId)
        {
            return Ok(await _feedbackRepository.GetByUserId(userId));
        }
        [HttpGet("byProduct/{productId:Guid}")]
        public async Task<ActionResult<List<Feedback>>> GetByProduct(Guid productId)
        {
            return Ok(await _feedbackRepository.GetByProductId(productId));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add([FromBody] FeedbackRequest feedbackRequest)
        {
            var feedback = new Feedback(
                Guid.NewGuid(),
                feedbackRequest.UserId,
                feedbackRequest.ProductId,
                DateTime.UtcNow,
                feedbackRequest.Comment
                );

            Guid resp = await _feedbackRepository.Add(feedback);
            return resp != Guid.Empty ? Ok(resp) : BadRequest(resp);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] FeedbackRequest feedbackRequest)
        {
            var feedback = new Feedback(
                id,
                feedbackRequest.UserId,
                feedbackRequest.ProductId,
                DateTime.UtcNow,
                feedbackRequest.Comment
                );

            var dbResp = await _feedbackRepository.Update(feedback, id);
            return Ok(dbResp);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Guid>> Delete(Guid id)
        {
            var dbResp = await _feedbackRepository.Delete(id);
            return Ok(dbResp);
        }

        [HttpDelete("byUser/{userId:Guid}")]
        public async Task<ActionResult<Guid>> DeleteByUser(Guid userId)
        {
            var dbResp = await _feedbackRepository.DeleteAllByUserId(userId);
            return Ok(dbResp);
        }

        [HttpDelete("byProduct/{productId:Guid}")]
        public async Task<ActionResult<Guid>> DeleteByProduct(Guid productId)
        {
            var dbResp = await _feedbackRepository.DeleteAllByProductId(productId);
            return Ok(dbResp);
        }
    }
}
