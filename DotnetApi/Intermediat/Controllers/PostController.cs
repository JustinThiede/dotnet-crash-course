using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public PostController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("Posts")]
    public IEnumerable<Post> GetPosts()
    {
        var sql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts";

        return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("PostSingle/{postId}")]
    public Post GetPostSingle(int postId)
    {
        var sql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                    WHERE PostId = " + postId.ToString();

        return _dapper.LoadDataSingle<Post>(sql);
    }

    [HttpGet("PostsByUser/{userId}")]
    public IEnumerable<Post> GetPostsByUser(int userId)
    {
        var sql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                    WHERE UserId = " + userId.ToString();

        return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> GetMyPosts()
    {
        var sql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                    WHERE UserId = " + User.FindFirst("userId")?.Value;

        return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("PostsBySearch/{searchParam}")]
    public IEnumerable<Post> PostsBySearch(string searchParam)
    {
        var sql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                    WHERE PostTitle LIKE '%" + searchParam + "%'" + " OR PostContent LIKE '%" + searchParam + "%'";

        return _dapper.LoadData<Post>(sql);
    }

    [HttpPost("Post")]
    public IActionResult AddPost(PostToAddDto postToAdd)
    {
        var sql = @"
            INSERT INTO TutorialAppSchema.Posts(
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated]) VALUES (" + User.FindFirst("userId")?.Value + ",'" + postToAdd.PostTitle + "','" + postToAdd.PostContent + "', GETDATE(), GETDATE() )";
        if (_dapper.ExecuteSql(sql)) return Ok();

        throw new Exception("Failed to create new post!");
    }

    // [AllowAnonymous]
    [HttpPut("Post")]
    public IActionResult EditPost(PostToEditDto postToEdit)
    {
        var sql = @"
            UPDATE TutorialAppSchema.Posts 
                SET PostContent = '" + postToEdit.PostContent + "', PostTitle = '" + postToEdit.PostTitle + @"', PostUpdated = GETDATE()
                    WHERE PostId = " + postToEdit.PostId.ToString() + "AND UserId = " + User.FindFirst("userId")?.Value;

        if (_dapper.ExecuteSql(sql)) return Ok();

        throw new Exception("Failed to edit post!");
    }

    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
        var sql = @"DELETE FROM TutorialAppSchema.Posts 
                WHERE PostId = " + postId.ToString() + "AND UserId = " + User.FindFirst("userId")?.Value;


        if (_dapper.ExecuteSql(sql)) return Ok();

        throw new Exception("Failed to delete post!");
    }
}