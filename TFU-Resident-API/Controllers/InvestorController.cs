using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuperOwnerModels;
using TFU_Resident_API.Data;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Controllers
{
    [Route("api/investor")]
    [ApiController]
    public class InvestorController : ControllerBase
    {
        readonly AppDbContext superOwnerContext;
        readonly IMapper mapper;

        public InvestorController(AppDbContext superOwnerContext, IMapper mapper)
        {
            this.superOwnerContext = superOwnerContext;
            this.mapper = mapper;
        }

        [HttpPost("createInvestor")]
        public async Task<IActionResult> CreateInvestor(CreateInvestorDto createInvestorDto)
        {
            var investor = new Investor()
            {
                UserId = createInvestorDto.UserId,
            };
            this.superOwnerContext.Add(investor);
            this.superOwnerContext.SaveChanges();

            return Ok();
        }

        [HttpPost("createProject")]
        public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            var project = new Project();
            project.Permalink = createProjectDto.Permalink;
            project.Name = createProjectDto.Name;
            project.InvestorId = createProjectDto.InvestorId;
            var newProject = this.superOwnerContext.Add(project);
            this.superOwnerContext.SaveChanges();
            return Ok();
        }
    }
}
