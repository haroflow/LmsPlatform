using System.Text.Json.Serialization;

namespace LmsPlatform.Dtos
{
	public class GetMyLessonDto
	{
		public string? Title { get; set; }
		public string? Slug { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public bool? Completed { get; set; }
	}
}