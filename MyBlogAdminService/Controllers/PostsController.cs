using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBlogAdminService.Data;
using MyBlogAdminService.Models;
using MyBlogAdminService.Models.dtos;

namespace MyBlogAdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly MyBlogAdminDbContext _context;

        public PostsController(MyBlogAdminDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromQuery] string? searchKey)
        {
            var query = _context.Posts
                                    .Include(p => p.Categories)
                                    .Include(p => p.Tags)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(searchKey))
                query = query.Where(p => p.Title.Contains(searchKey));

            return await query.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, UpdatePostDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }
            
            var post = await _context.Posts
                                         .Include(p => p.Categories)
                                         .Include(p => p.Tags)
                                         .FirstOrDefaultAsync(p => p.Id == dto.Id);
            
            if (post == null) { return NotFound(); }


            var foundCategories = await getCategoriesByIds(dto.CategoryIds);
            var foundTags = await getTagsByIds(dto.TagIds);

            if (!foundCategories.Count.Equals(dto.CategoryIds?.Count) && !foundTags.Count.Equals(dto.TagIds?.Count))
            {
                return BadRequest();
            }

            post.Title = dto.Title;
            post.Content = dto.Content;

            post.Categories?.Clear();
            foreach (var category in foundCategories)
                post.Categories?.Add(category);

            post.Tags?.Clear();
            foreach (var tag in foundTags)
                post.Tags?.Add(tag);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                    return NotFound("Bài viết không tồn tại hoặc đã bị xóa.");
                else
                    throw;
            }
            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(CreatePostDto dto)
        {
            var foundCategories = await getCategoriesByIds(dto.CategoryIds);
            var foundTags = await getTagsByIds(dto.TagIds);

            if (!foundCategories.Count.Equals(dto.CategoryIds?.Count) && !foundTags.Count.Equals(dto.TagIds?.Count))
            {
                return BadRequest();
            }

            // Tạo Post mới
            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                Created = DateTime.Now,
                Categories = foundCategories,
                Tags = foundTags
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }



        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private async Task<ICollection<Category>> getCategoriesByIds(ICollection<int>? CategoryIds)
        {
            if (CategoryIds == null || !CategoryIds.Any())
                return new List<Category>();

            return await _context.Categories
                .Where(c => CategoryIds.Contains(c.Id))
                .ToListAsync();
        }

        private async Task<ICollection<Tag>> getTagsByIds(ICollection<int>? TagIds)
        {
            if (TagIds == null || !TagIds.Any())
                return new List<Tag>();

            return await _context.Tags
                .Where(t => TagIds.Contains(t.Id))
                .ToListAsync();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
