using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoryStore.Data;
using StoryStore.DataModels;
using StoryStore.Models;
namespace StoryStore.Controllers
{
    public class StoryController : Controller
    {

        private readonly StoryStoreDbContext _db;
        private readonly string StoryAudioFolder = "StoryAudios";
        private readonly string StoryPdfFolder = "StoryPdfs";
        private readonly string StoryImages = "StoryImages";
        private readonly UserManager<AppUser> _userManager;
        public StoryController(StoryStoreDbContext db,
             UserManager<AppUser> userManager,RoleManager<IdentityRole>roleManager)
        {
            _db = db;
            _userManager = userManager;
           
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAllStories([FromBody] RequestQuery requestQuery)
        {
            if (requestQuery.PageNumber > 0)
                requestQuery.PageNumber = (requestQuery.PageNumber - 1) * requestQuery.PageSize;
                var stories = _db.Stories.Where(x => x.StoryName.Contains(requestQuery.StoryName))
          .Skip(requestQuery.PageNumber).Take(requestQuery.PageSize).ToList();
                return Ok(stories);
           
        }

        [Authorize]
        public IActionResult GetStoryDetails(int  storyId)
        {
            var stories = from story in _db.Stories
                          from ageRange in _db.AgeRanges
                          where story.AgeRangeId == ageRange.Id && story.StoryId==storyId
                          select story;
            var storyModel = stories.FirstOrDefault();
            var FindedAgeRange = _db.AgeRanges.Where(x => x.Id == storyModel.AgeRangeId).FirstOrDefault();
            storyModel.AgeRange = FindedAgeRange;

            return View(storyModel);
        }


        [Authorize]
        public async Task<IActionResult> GetStoriesByAgeRange([FromBody] RequestQuery requestQuery)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            IQueryable<Story> stories;
            var AgeRanges = _db.AgeRanges.ToList();



            if (requestQuery.PageNumber > 0)
                requestQuery.PageNumber = (requestQuery.PageNumber - 1) * requestQuery.PageSize;

            foreach (var rol in roles)
            {

                if(rol== "Admin")
                {

                    if(requestQuery.AgeRangeId!=null)
                    {

                        stories = from story in _db.Stories
                                  from ageRange in _db.AgeRanges
                                  where story.AgeRangeId == ageRange.Id
                                  select story;


                        stories = stories.Where(x => x.StoryName.Contains(requestQuery.StoryName) &&
                        x.AgeRangeId == requestQuery.AgeRangeId)
                             .Skip(requestQuery.PageNumber).Take(requestQuery.PageSize);
                        return Ok(stories);
                    }
                    else
                    {
                        try
                        {
                            stories = from story in _db.Stories
                                    from ageRange in _db.AgeRanges
                                    where story.AgeRangeId == ageRange.Id
                                    select  story;

                            stories = stories.Where(x => x.StoryName.Contains(requestQuery.StoryName))
                                .Skip(requestQuery.PageNumber).Take(requestQuery.PageSize) ;
                            return Ok(stories);
                        }
                        catch (Exception ee)
                        {

                            throw ee;
                        }
                    }
                 
                }

            }

            stories = from story in _db.Stories
                      from ageRange in _db.AgeRanges
                      where story.AgeRangeId == ageRange.Id
                      select story;

            stories = stories.Where(x => x.StoryName.Contains(requestQuery.StoryName)&&x.AgeRangeId==user.AgeRangeId)
                                .Skip(requestQuery.PageNumber).Take(requestQuery.PageSize);
                return Ok(stories);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetStoriesCount()
        {
            return Ok(_db.Stories.Count());
        }
        [Authorize]
        [HttpGet]
        public IActionResult DeleteStory([FromQuery] int storyId)
        {

            var findedStory = _db.Stories.Where(x => x.StoryId == storyId).SingleOrDefault();
            if (findedStory != null)
            {
                _db.Remove(findedStory);
                _db.SaveChanges();
                return Ok("done");
            }
            return NotFound("story Not found");
        }

        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 4294967295)]
        public async Task<IActionResult> AddNewStory([FromForm]AddStoryModel addStoryModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    Story story = new Story();
                    story.StoryName = addStoryModel.storyName;
                    story.Description = addStoryModel.Description;
                    story.Author = addStoryModel.AuthorName;
                    story.AgeRangeId = addStoryModel.AgeRangeId;


                    if (addStoryModel.StoryDate == "" || addStoryModel.StoryDate==null)
                    {
                        story.StoryDate = DateTime.Now;

                    }
                    else
                    {
                        story.StoryDate = DateTime.ParseExact(addStoryModel.StoryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    await _db.Stories.AddAsync(story);
                    await _db.SaveChangesAsync();

                    // add, pdf,image and audio file
                    string pdfUrl = await AddFile(addStoryModel.PdfFile, addStoryModel.storyName,story.StoryId, StoryPdfFolder);
                    string audioUrl = await AddFile(addStoryModel.AudioFile, addStoryModel.storyName, story.StoryId, StoryAudioFolder);
                    string imageUrl = await AddFile(addStoryModel.StoryImage, addStoryModel.storyName, story.StoryId, StoryImages);
                    story.PdfUrl = pdfUrl;
                    story.AudioUrl = audioUrl;
                    story.ImageUrl = imageUrl;
                    await _db.SaveChangesAsync();
                    return Ok("done");
                }
                catch (Exception e)
                {
                    throw e;

                    return NotFound("some thing went wrong");
                }

            }
            return BadRequest();
        }
        public async Task<string> AddFile(IFormFile newFile, string storyName,int storyId, string folderName)
        {
            if (newFile != null && storyName != "")
            {
                var pathFile = Path.Combine(Directory.GetCurrentDirectory(), @$"wwwroot\{folderName}");
                System.IO.Directory.CreateDirectory(pathFile);
                string fileExtetion = Path.GetExtension(newFile.FileName);
                var fileName = $"{storyId}-{storyName}{fileExtetion}";
                string fullPath = Path.Combine($@"{pathFile}\{fileName}");
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    System.IO.Directory.CreateDirectory(pathFile);
                    await newFile.CopyToAsync(stream);
                }
                return $"/{folderName}/{fileName}";
            }
            return "";
        }

       
    }
}
